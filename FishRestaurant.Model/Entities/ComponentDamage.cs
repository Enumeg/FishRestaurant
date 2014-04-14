using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class ComponentDamage : Damage
    {
        public int ComponentId { get; set; }
        public virtual Component Component { get; set; }
        public Units Unit { get; set; }
    }
}
