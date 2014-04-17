using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class ProductComponents
    {
        [Key]
        public int ProductId { get; set; }
        public int ComponentId { get; set; }
        public decimal Amount { get; set; }
        public Units Unit { get; set; }
        public virtual Product Product { get; set; }
        public virtual Component Component { get; set; }

    }
}
