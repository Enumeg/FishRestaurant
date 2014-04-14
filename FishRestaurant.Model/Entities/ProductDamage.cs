using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class ProductDamage : Damage
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
