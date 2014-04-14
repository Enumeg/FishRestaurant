using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class Purchase : Transaction
    {
        public Purchase()
        {
            Purchase_Details = new List<PurchaseDetail>();
        }
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseDetail> Purchase_Details { get; set; }

    }
}
