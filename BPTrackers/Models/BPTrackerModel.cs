using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BPTrackers.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Invoice_number { get; set; }
        public DateTime Date { get; set; }
        public string Responsible { get; set; }
        public Parcel Parcel { get; set; }

    }

    public class Carrier
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Parcel
    {
        public int Id { get; set; }
        public Carrier Carrier { get; set; }
        public DateTime? Date_of_delivery { get; set; }
        public DateTime? Date_of_receiving { get; set; }
        public string Track_number { get; set; }
        public string Status { get; set; }
        public Department Department_sender { get; set; }
        public Department Department_destination { get; set; }

        public static Parcel RefreshParcel(Parcel parcel, RootObject rootObject)
        {
            using (var db = new DataContext())
            {
                if (rootObject.data[0].CitySender != null)
                {
                    string name_dest = rootObject.data[0].CitySender;
                    Department department_dest = db.Departments.FirstOrDefault(d => d.Name == name_dest);
                    if (department_dest == null)
                    {
                        department_dest = new Department();
                        department_dest.Name = name_dest;
                        db.Departments.Add(department_dest);
                        db.SaveChanges();
                    }
                    parcel.Department_destination = department_dest;
                }

                if (rootObject.data[0].CityRecipient != null)
                {
                    string name_send = rootObject.data[0].CityRecipient;
                    Department department_send = db.Departments.FirstOrDefault(d => d.Name == name_send);
                    if (department_send == null)
                    {
                        department_send = new Department();
                        department_send.Name = name_send;
                        db.Departments.Add(department_send);
                        db.SaveChanges();
                    }
                    parcel.Department_sender = department_send;
                }

                if (rootObject.data[0].Status != null)
                {
                    parcel.Status = rootObject.data[0].Status;
                }

                if (rootObject.data[0].RecipientDateTime != null)
                {
                    parcel.Date_of_receiving = DateTime.Parse(rootObject.data[0].RecipientDateTime);
                }
                if (rootObject.data[0].DateCreated != null)
                {
                    parcel.Date_of_delivery = DateTime.Parse(rootObject.data[0].DateCreated);
                }

            }
            return parcel;
        }
    }

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class InvoiceView
    {
        public int Id { get; set; }
        public string Invoice_number { get; set; }
        public DateTime Date { get; set; }
        public string Responsible { get; set; }
        public int Name { get; set; }
        public DateTime? Date_of_delivery { get; set; }
        public DateTime? Date_of_receiving { get; set; }
        public string Track_number { get; set; }
        public string Status { get; set; }
        public string Department_sender { get; set; }
        public string Department_destination { get; set; }

    }



}