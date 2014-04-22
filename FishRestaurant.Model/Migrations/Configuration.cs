namespace FishRestaurant.Model.Migrations
{

    using System.Data.Entity.Migrations;
    using MySql.Data.Entity;

    public sealed class Configuration : DbMigrationsConfiguration<FishRestaurant.Model.Entities.FRContext>
    {
        public Configuration()
        {
            this.SetSqlGenerator("MySql.Data.MySqlClient", new FishRestaurant.Model.Services.SqlMigrator());
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FishRestaurant.Model.Entities.FRContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
