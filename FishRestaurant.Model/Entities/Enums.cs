using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public enum CategoryTypes : sbyte { Compontent, Product }
    public enum TransactionTypes : sbyte { Buy, ReBuy, InHouse, TakeAway, Order, SellBack, In, Out }
    public enum Units : sbyte { كيلو, جرام }
    public enum PersonTypes : sbyte { Customer, Supplier }

    public enum Groups : sbyte { Cashier ,  Admin }

}
