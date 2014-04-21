using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FishRestaurant.Model.Entities
{

   public class FRContext : DbContext
    {
        public FRContext()
            : base("Con")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Person> People { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }       
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<ComponentDamage> ComponentsDamage { get; set; }
        public DbSet<ProductDamage> ProductsDamage { get; set; }       

    }
}
