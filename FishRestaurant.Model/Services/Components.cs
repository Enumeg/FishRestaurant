using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishRestaurant.Model.Entities;

namespace FishRestaurant.Model.Services
{
   public class ComponentsService
    {
        public static decimal GetShopStock(Component component)
        {        
            decimal In = 0, Out = 0, stock = 0;
            try
            {              
                var Ins = component.TransferDetails.Where(t => t.Transfer.Type == TransactionTypes.Out);
                var Outs = component.TransferDetails.Where(t => t.Transfer.Type == TransactionTypes.In);
                var ProComs = component.ProductComponents;
                if (Ins.Count() > 0) { In = Ins.Sum(t => t.Amount * (t.Unit == Units.جرام ? 0.001m : 1)); }
                if (Outs.Count() > 0) { Out = Outs.Sum(t => t.Amount * (t.Unit == Units.جرام ? 0.001m : 1)); }
                foreach (var pc in ProComs)
                {
                    var amount = pc.Unit == Units.جرام ? pc.Amount * 0.001m : pc.Amount;
                    if (pc.Product.ProductsDamage.Count > 0) { Out += pc.Product.ProductsDamage.Sum(p => p.Amonut * amount); }
                    if (pc.Product.SaleDetails.Count > 0) { Out += pc.Product.SaleDetails.Where(s=>s.Transaction.Type== TransactionTypes.InHouse).Sum(s => s.Amount * amount); }
                }
                stock = Math.Round(In - Out, 3);
               
            }
            catch
            {

            }
            return stock;
        }
        public static decimal GetStock(Component component)
        {           
            decimal In = 0, Out = 0, stock = 0;
            try
            {
                var purchases = component.PurchaseDetails.Where(p => p.Transaction.Type == TransactionTypes.Buy);
                var repurchases = component.PurchaseDetails.Where(p => p.Transaction.Type == TransactionTypes.ReBuy);
                //var Ins = component.TransferDetails.Where(t => t.Transfer.Type == Transaction_Types.In);
                //var Outs = component.TransferDetails.Where(t => t.Transfer.Type == Transaction_Types.Out);
                var Coms = component.ComponentDamages;
                if (purchases.Count() > 0) { In = purchases.Sum(t => t.Amount * (t.Unit == Units.جرام ? 0.001m : 1)); }
                if (repurchases.Count() > 0) { Out = repurchases.Sum(t => t.Amount * (t.Unit == Units.جرام ? 0.001m : 1)); }
                //if (Ins.Count() > 0) { In += Ins.Sum(t => t.Amount * (t.Unit == Units.جرام ? 0.001m : 1)); }
                //if (Outs.Count() > 0) { Out += Outs.Sum(t => t.Amount * (t.Unit == Units.جرام ? 0.001m : 1)); }
                if (Coms.Count() > 0) { Out += Coms.Sum(t => t.Amonut * (t.Unit == Units.جرام ? 0.001m : 1)); }
                var ProComs = component.ProductComponents;              
                foreach (var pc in ProComs)
                {
                    var amount = pc.Unit == Units.جرام ? pc.Amount * 0.001m : pc.Amount;
                    if (pc.Product.ProductsDamage.Count > 0) { Out += pc.Product.ProductsDamage.Sum(p => p.Amonut * amount); }
                    if (pc.Product.SaleDetails.Count > 0) { Out += pc.Product.SaleDetails.Where(s => s.Transaction.Type != TransactionTypes.SellBack).Sum(s => s.Amount * amount); 
                        In += pc.Product.SaleDetails.Where(s => s.Transaction.Type == TransactionTypes.SellBack).Sum(s => s.Amount * amount); }
                }
                In += component.Stock;
                stock = Math.Round(In - Out, 3);
                component.StoreStock = stock;
            }
            catch
            {

            }
            return stock;
        }
    }
}
