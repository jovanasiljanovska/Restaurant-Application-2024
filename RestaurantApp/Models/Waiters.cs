using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantApp.Models
{
    public class Waiters
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int countTables { get; set; }
        public virtual ICollection<Tables> tables { get; set; }
        public Waiters() { 
            tables = new List<Tables>();
            countTables = 0;
        }
    }
}