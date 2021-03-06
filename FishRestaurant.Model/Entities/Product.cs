﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class Product
    {
        public Product()
        {
            ProductComponents = new ObservableCollection<ProductComponents>();
            ProductsDamage = new List<ProductDamage>();
            SaleDetails = new List<SaleDetail>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte[] Image { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductComponents> ProductComponents { get; set; }
        public virtual ICollection<ProductDamage> ProductsDamage { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
