using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class PurchaseDetail : TransactionDetail
    {
        public int? TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public int ComponentId { get; set; }
        public virtual Component Component { get; set; }
        public Units Unit { get; set; }  
    }
}
