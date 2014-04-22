using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Services
{
    public class SqlMigrator : MySql.Data.Entity.MySqlMigrationSqlGenerator
    {    
        protected override MigrationStatement Generate(DropForeignKeyOperation dropForeignKeyOperation)
        {
            dropForeignKeyOperation.Name = StripDbo(dropForeignKeyOperation.Name);
            dropForeignKeyOperation.DependentTable = StripDbo(dropForeignKeyOperation.DependentTable);
          return  base.Generate(dropForeignKeyOperation);
        }

        protected override MigrationStatement Generate(DropIndexOperation dropIndexOperation)
        {
            dropIndexOperation.Name = StripDbo(dropIndexOperation.Name);
            dropIndexOperation.Table = StripDbo(dropIndexOperation.Table);
            return base.Generate(dropIndexOperation);
        }

        protected override MigrationStatement Generate(DropPrimaryKeyOperation dropPrimaryKeyOperation)
        {
            dropPrimaryKeyOperation.Name = StripDbo(dropPrimaryKeyOperation.Name);
            dropPrimaryKeyOperation.Table = StripDbo(dropPrimaryKeyOperation.Table);
            return base.Generate(dropPrimaryKeyOperation);
        }

        private string StripDbo(string name)
        {
            return name.Replace("dbo.", "");
        }
    }
}


