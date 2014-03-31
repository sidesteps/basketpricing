using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Sales.Pricing.Domain
{
    public class SKU
    {
        private string normalizedId;

        public SKU(string id)
        {
            if (string.IsNullOrEmpty(id.Trim()))
                throw new ArgumentException("Item ID cannot be null, empty or whitespace");
            normalizedId = id.Trim().ToUpper();
        }

        public override string ToString()
        {
            return normalizedId;
        }

        public static implicit operator string(SKU id)
        {
            return id.normalizedId;
        }

        public static implicit operator SKU(string id)
        {
            return new SKU(id);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            SKU sku = obj as SKU;
            if ((System.Object)sku == null)
            {
                return false;
            }
            return this == sku;
        }

        public static bool operator == (SKU a, SKU b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.normalizedId == b.normalizedId;
        }

        public static bool operator !=(SKU a, SKU b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return normalizedId.GetHashCode();
        }
    }
}
