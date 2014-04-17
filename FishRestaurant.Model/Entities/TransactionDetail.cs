using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class TransactionDetail
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        [NotMapped]
        public decimal Total { get { return Math.Round(this.Amount * this.Price, 2); } }
    }
}
