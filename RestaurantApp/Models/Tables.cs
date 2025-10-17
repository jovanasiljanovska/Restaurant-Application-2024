using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantApp.Models
{
    public class Tables
    {
        [Key]
        public  int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public double Bill { get; set; }
        public Waiters waiter { get; set; }
        public ICollection<Items> items { get; set; }
        public Tables()
        {
            items = new List<Items>();
            //waiter = new Waiters();
            Bill = 0.0;
            for(int i = 0; i < items.Count; i++)
            {
                Bill += items.ElementAt(i).Price;
            }
        }
    }
}