using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class Sale: Transaction
    {
        public Sale()
        {
            SaleDetails = new List<SaleDetail>();
        }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
