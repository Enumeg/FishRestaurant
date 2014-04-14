using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class SaleDetail : TransactionDetail
    {
        public int SaleId { get; set; }
        public virtual Sale Sale { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
