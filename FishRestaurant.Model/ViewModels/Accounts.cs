using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.ViewModels
{
   public class Accounts
    {
        public decimal Debtor { get; set; }
        public decimal Creditor { get; set; }
        public decimal balance { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }        
    }
}
