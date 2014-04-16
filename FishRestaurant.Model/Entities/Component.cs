using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int Id { get; set; }
        [MaxLength(500), Column(TypeName = "varchar"), Required]
        public string Name { get; set; }
        public decimal AmountLimit { get; set; }
        public decimal Stock { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<PurchaseDetail> Buy_Details { get; set; }
    }
}
