using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Sales.Pricing.Domain
{
    /// <summary>
    /// Represents a map of SKU prices
    /// </summary>
    public class PriceList : IDictionary<SKU, decimal>
    {
        private readonly Dictionary<SKU, decimal> priceList;

        public PriceList(IEnumerable<Tuple<SKU, decimal>> itemPrices)
        {
            if (itemPrices == null || itemPrices.Count() == 0)
                throw new ArgumentException("Cannot construct price list without item prices");
            if (!itemPrices.All(item => item != null))
                throw new ArgumentNullException("SKU cannot be null");
            if (!itemPrices.All(item => item.Item2 > 0))
                throw new ArgumentException("Item price must be more than zero");
             priceList = itemPrices.ToDictionary(item => item.Item1, item => item.Item2);
        }

        public decimal this[SKU sku]
        {
            get
            {
                return priceList[sku];
            }
            set
            {
                throw new InvalidOperationException("Price list is immutable");
            }
        }

        public void Add(SKU key, decimal value)
        {
            throw new InvalidOperationException("Price list is immutable");
        }

        public bool ContainsKey(SKU key)
        {
            return priceList.ContainsKey(key);
        }

        public ICollection<SKU> Keys
        {
            get { return priceList.Keys; }
        }

        public bool Remove(SKU key)
        {
            throw new InvalidOperationException("Price list is immutable");
        }

        public bool TryGetValue(SKU key, out decimal value)
        {
            return priceList.TryGetValue(key, out value);
        }

        public ICollection<decimal> Values
        {
            get { return priceList.Values; }
        }

        public void Add(KeyValuePair<SKU, decimal> item)
        {
            throw new InvalidOperationException("Price list is immutable");
        }

        public void Clear()
        {
            throw new InvalidOperationException("Price list is immutable");
        }

        public bool Contains(KeyValuePair<SKU, decimal> item)
        {
            return priceList.Contains(item);
        }

        public int Count
        {
            get { return priceList.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(KeyValuePair<SKU, decimal> item)
        {
            throw new InvalidOperationException("Price list is immutable");
        }

        public IEnumerator<KeyValuePair<SKU, decimal>> GetEnumerator()
        {
            return priceList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (System.Collections.IEnumerator)priceList.GetEnumerator();
        }

        public void CopyTo(KeyValuePair<SKU, decimal>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
    }
}
