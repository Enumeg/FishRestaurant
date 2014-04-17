using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
   public class TransferDetail
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int? TransferId { get; set; }
        public virtual Transfer Transfer { get; set; }
        public int ComponentId { get; set; }
        public virtual Component Component { get; set; }
        public Units Unit { get; set; }  
    }
}
