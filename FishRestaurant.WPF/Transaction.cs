using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishRestaurant.Model.Entities;

namespace FishRestaurant.WPF
{
   public class TransactionsService
    {
        public static string GetNumber(DateTime date, Transaction_Types Type)
        {
            FRContext DB = new FRContext();

            if (Type == Transaction_Types.In || Type == Transaction_Types.Out)
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
