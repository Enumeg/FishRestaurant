using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class Transaction
    {
        public Transaction()
        {
            SaleDetails = new ObservableCollection<SaleDetail>();
            PurchaseDetails = new ObservableCollection<PurchaseDetail>();
        }      
        public int Id { get; set; }
        public int Number { get; set; }
        public TransactionTypes Type { get; set; }     
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Paid { get; set; }
        public decimal Delivery { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
 
    }
}
