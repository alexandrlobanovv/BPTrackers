using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BPTrackers.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base()
        {
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<DataContext>());
        }
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}