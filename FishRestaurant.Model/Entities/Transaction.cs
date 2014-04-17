using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public Transaction_Types Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Paid { get; set; }
        //[NotMapped]
        //public decimal Rest { get { return Math.Round(this.Total - this.Paid, 2); } }
    }
}
