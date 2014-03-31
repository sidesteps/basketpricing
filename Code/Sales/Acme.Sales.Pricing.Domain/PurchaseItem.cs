using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Sales.Pricing.Domain
{
    /// <summary>
    /// Represents purchase item
    /// </summary>
    public class PurchaseItem
    {
        public PurchaseItem(SKU sku, uint quantity)
        {
            if (sku == null)
                throw new ArgumentNullException("SKU must be provided");
            if(quantity == 0)
                throw new ArgumentException("Cannot purchase zero units");
            this.ItemId = sku;
            this.Quantity = quantity;
        }

        public SKU ItemId
        {
            get;
            private set;
        }

        public uint Quantity
        {
            get;
            private set;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            PurchaseItem pi = obj as PurchaseItem;
            if ((System.Object)pi == null)
            {
                return false;
            }
            return this == pi;
        }

        public static bool operator ==(PurchaseItem a, PurchaseItem b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ItemId == b.ItemId && a.Quantity == b.Quantity;
        }

        public static bool operator !=(PurchaseItem a, PurchaseItem b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (int)Quantity;
        }
    }
}
