using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Entity;
namespace FishRestaurant.Model.Entities
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
   public class FrContext : DbContext
    {
       
        public FrContext()
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
        public DbSet<ProductComponents> ProductsComponents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<TransferDetail> TransferDetails { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<Outcome> Outcomes { get; set; }
        public DbSet<OutcomeType> OutcomeTypes { get; set; }

        public DbSet<ComponentDamage> ComponentsDamage { get; set; }
        public DbSet<ProductDamage> ProductsDamage { get; set; }


        public DbSet<User> Users { get; set; }
        

    }
}
