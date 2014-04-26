using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public enum CategoryTypes : sbyte { Compontent, Product }
    public enum Transaction_Types : sbyte { Buy, ReBuy, SellIn, SellOut, In, Out, Order }
    public enum Units : sbyte { كيلو, جرام }
    public enum PersonTypes : sbyte { Customer, Supplier }

}
