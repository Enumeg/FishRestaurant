using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class SaleDetail : TransactionDetail
    {
        public int? TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
