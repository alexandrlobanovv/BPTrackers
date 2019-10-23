using BPTrackers.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace BPTrackers.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //DataContext db = new DataContext();
            //db.Carriers.Add(new Carrier() { Name = "Nova Poshta" });
            //db.SaveChanges();
            //var invoices = db.Invoices.Include("Parcel.Carrier").ToList();


            return View();
        }

        public string GetData()
        {
            string formatString = string.Empty;
            using (DataContext db = new DataContext())
            {
                var invoices = db.Invoices.Include("Parcel.Carrier").Include("Parcel.Department_sender").Include("Parcel.Department_destination").ToList();
               
                formatString = JsonConvert.SerializeObject(invoices); 
            }

            return formatString;
        }
        
        public string GetCarrier()
        {
            string formatString = "";
            using (DataContext db = new DataContext())
            {
                var invoices = db.Carriers.ToList();
                foreach (var item in invoices)
                {
                    formatString += $"{item.Id}:{item.Name};";
                }
            }

            return formatString.Remove(formatString.Length - 1);
        }

        [HttpPost]
        public string Edit(FormCollection input)
        {
            using (DataContext db = new DataContext())
            {
                string str = input["Id"];
                // действия по редактированию
                int id = Int32.Parse(input["Id"].Substring(0, input["Id"].IndexOf(',')));
                int id_carrier = Int32.Parse(input["Parcel.Carrier.Name"]);
                int id_parcel = Int32.Parse(input["Parcel.Id"]);
                string track = input["Parcel.Track_number"];
                string invoice_num = input["Invoice_number"];
                DateTime date = DateTime.Parse(input["Date"].Replace(':','/'));
                string responsible = input["Responsible"];
                


                var carrier = db.Carriers.FirstOrDefault(c => c.Id == id_carrier);
                var parcel = db.Parcels.FirstOrDefault(p => p.Id == id_parcel);
                parcel.Track_number = track;
                parcel.Carrier = carrier;
                db.Entry(parcel).State = EntityState.Modified;
                db.SaveChanges();
                var invoice = new Invoice() { Id = id, Invoice_number = invoice_num, Date = date, Responsible = responsible, Parcel = parcel };
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
            }
            return "hello";
            
        }

        [HttpPost]
        public void Create(FormCollection input)
        {
            using (DataContext db = new DataContext())
            {
                // действия по добавлению
                int id_carrier = Int32.Parse(input["Parcel.Carrier.Name"]);
                string track = input["Parcel.Track_number"];
                string invoice_num = input["Invoice_number"];
                DateTime date = DateTime.Parse(input["Date"]);
                string responsible = input["Responsible"];
                
                var carrier = db.Carriers.FirstOrDefault(c => c.Id == id_carrier);
                RootObject novaPochta = NP.GetDataParcel(track);
                Parcel parcel = new Parcel() { Track_number = track, Carrier = carrier };
                if (parcel.Carrier.Name == "Nova Poshta")
                {
                    parcel = Parcel.RefreshParcel(parcel, novaPochta);
                }
                

                db.Parcels.Add(parcel);
                db.SaveChanges();
                db.Invoices.Add(new Invoice() { Invoice_number = invoice_num, Date = date, Responsible = responsible, Parcel = parcel });
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void Delete(int id)
        {
            // действия по удалению
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}