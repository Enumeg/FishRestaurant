using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class Category
    {
        public Category()
        {
            Components = new List<Component>();
            Products = new List<Product>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryTypes Type { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Component> Components { get; set; }
    }
}
