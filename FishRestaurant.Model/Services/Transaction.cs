using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishRestaurant.Model.Entities;

namespace FishRestaurant.WPF.Services
{
   public class TransactionsService
    {
        public static string GetNumber(DateTime date, TransactionTypes Type)
        {
            FrContext DB = new FrContext();

            if (Type == TransactionTypes.In || Type == TransactionTypes.Out)
            {
                var num = DB.Transfers.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month && p.Type == Type);
                return num.Count() != 0 ? (num.Max(p => p.Number) + 1).ToString() : string.Format("{0}001", date.ToString("yyMM"));
            }
            else
            {
                var num = DB.Transactions.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month && p.Type == Type);
                return num.Count() != 0 ? (num.Max(p => p.Number) + 1).ToString() : string.Format("{0}001", date.ToString("yyMM"));
            }
        }
    }
}
