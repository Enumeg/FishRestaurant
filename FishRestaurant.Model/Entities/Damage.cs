using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class Damage
    {      
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amonut { get; set; }
    }
    public class ComponentDamage : Damage
    {
        public int ComponentId { get; set; }
        public virtual Component Component { get; set; }
        public Units Unit { get; set; }
    }
    public class ProductDamage : Damage
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
