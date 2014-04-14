using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class Component
    {
        public Component()
        {
            Buy_Details = new List<PurchaseDetail>();
        }
        public int ComponentId { get; set; }
        public string Name { get; set; }
        public int AmountLimit { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<PurchaseDetail> Buy_Details { get; set; }
    }
}
