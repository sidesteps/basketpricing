using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Sales.Pricing.Domain
{
    public class Deal
    {
        /// <summary>
        /// Serves as a token of no deal given.
        /// </summary>
        public static readonly Deal NoDeal = new Deal("No Deal", 0m, new PurchaseItem[] { new PurchaseItem("no deal item", 1) });

        public Deal(string name, decimal discount, IEnumerable<PurchaseItem> forPurchase)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("Nameless deal is not marketable");
            if (discount < 0)
                throw new ArgumentException("Discount cannot increase sales' price");
            if (forPurchase == null || forPurchase.Count() == 0)
                throw new ArgumentException("Must specify purchase items for which deal is given");
            this.Name = name;
            this.Discount = discount;
            this.ForPurchase = forPurchase;
        }

        public string Name
        {
            get;
            private set;
        }

        public decimal Discount
        {
            get;
            private set;
        }

        public IEnumerable<PurchaseItem> ForPurchase
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return string.Format("{0} -{1:C}", Name, Discount);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Deal pi = obj as Deal;
            if ((System.Object)pi == null)
            {
                return false;
            }
            return this == pi;
        }

        public static bool operator ==(Deal a, Deal b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Discount == b.Discount && a.Name == b.Name && a.ForPurchase.SequenceEqual(b.ForPurchase);
        }

        public static bool operator !=(Deal a, Deal b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (int)Discount;
        }
    }
}
