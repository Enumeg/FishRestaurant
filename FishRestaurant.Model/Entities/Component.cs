using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishRestaurant.Model.Services;

namespace FishRestaurant.Model.Entities
{
    public class Component
    {
        public Component()
        {
            PurchaseDetails = new List<PurchaseDetail>();
            ProductComponents = new List<ProductComponents>();
            TransferDetails = new List<TransferDetail>();
            ComponentDamages = new List<ComponentDamage>();
        }
        public int Id { get; set; }
        [MaxLength(500), Column(TypeName = "varchar"), Required]
        public string Name { get; set; }
        public decimal AmountLimit { get; set; }        
        public decimal Stock { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual ICollection<TransferDetail> TransferDetails { get; set; }
        public virtual ICollection<ProductComponents> ProductComponents { get; set; }
        public virtual ICollection<ComponentDamage> ComponentDamages { get; set; }
        [NotMapped]
        public decimal StoreStock { get; set; }
        //[NotMapped]
        //public decimal ShopStock { get { return ComponentsService.GetShopStock(this); } }
        //[NotMapped]
        //public int Status { get { return StoreStock > AmountLimit ? 1 : StoreStock == AmountLimit ? 0 : -1; } }
    }
}
