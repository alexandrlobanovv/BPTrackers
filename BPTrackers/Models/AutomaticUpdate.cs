using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BPTrackers.Models
{
    public class AutomaticUpdate: IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            using (var db = new DataContext())
            {
                var parcels = db.Parcels.Where(p => p.Status != "Відправлення отримано").ToList();
                for (int i = 0; i < parcels.Count; i++)
                {
                    RootObject rootObject = NP.GetDataParcel(parcels[i].Track_number);
                    parcels[i] = Parcel.RefreshParcel(parcels[i], rootObject);
                    db.Entry(parcels[i]).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
        }
    }

    public class RefreshScheduler
    {
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<AutomaticUpdate>().Build();

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger1", "group1")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithIntervalInHours(4)          // через 1 минуту
                    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер

            await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}