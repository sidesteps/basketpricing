using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acme.Sales.Pricing.Domain
{
    using AggregatedBasket = Dictionary<SKU, uint>;
    /// <summary>
    /// Represents collection of purchase items.
    /// </summary>
    public class PurchaseBasket: IEnumerable<PurchaseItem>
    {
        private AggregatedBasket basketItems;

        private PurchaseBasket(AggregatedBasket aggregatedItems)
        {
            this.basketItems = aggregatedItems;
        }

        public PurchaseBasket(IEnumerable<SKU> ItemIDs)
        {
            if (ItemIDs == null || ItemIDs.Count() == 0)
                throw new ArgumentException("Need to provide purchase items for a basket");
            if (!ItemIDs.All(item => item != null))
                throw new ArgumentException("SKU cannot be null");
            basketItems = ItemIDs.GroupBy(sku => sku, (sku, grouping) => new KeyValuePair<SKU, uint>(sku, (uint)grouping.Count()))
                .ToDictionary(itm => itm.Key, itm => itm.Value);
        }

        /// <summary>
        /// Removes purchase item from basket
        /// </summary>
        /// <param name="item">Purchase item to remove</param>
        /// <returns>Modified purchase basket</returns>
        public PurchaseBasket Remove(PurchaseItem item)
        {
            return Remove(new PurchaseItem[] { item });
        }

        public PurchaseBasket Remove(IEnumerable<PurchaseItem> items)
        {
            if (!items.All(item => item != null))
                throw new ArgumentNullException("item to remove cannot be null");
            if (!Contains(items))
                throw new InvalidOperationException("Items to remove cannot exceed basket contents");
            var modifiedBasket = new AggregatedBasket(basketItems);
            foreach (var item in items)
            {
                SubstractFromBasket(item, modifiedBasket);
            }
            return new PurchaseBasket(modifiedBasket);
        }

        public bool Contains(IEnumerable<PurchaseItem> purchaseItems)
        {
            if (purchaseItems == null || purchaseItems.Count() == 0)
                throw new ArgumentException("Purchase items to check must be provided");
            return purchaseItems.All(Contains);
        }

        public bool Contains(PurchaseItem purchaseItem)
        {
            if (purchaseItem == null)
                throw new ArgumentNullException("Purchase item to check cannot be null");
            return basketItems.ContainsKey(purchaseItem.ItemId) && 
                basketItems[purchaseItem.ItemId] >= purchaseItem.Quantity;
        }

        private void SubstractFromBasket(PurchaseItem item, AggregatedBasket basket)
        {
            uint remainingQuantity = basket[item.ItemId] - item.Quantity;
            if (remainingQuantity <= 0)
            {
                basket.Remove(item.ItemId);
            }
            else
            {
                basket[item.ItemId] = remainingQuantity;
            }
        }

        public IEnumerator<PurchaseItem> GetEnumerator()
        {
            return basketItems.Select(keyVal => new PurchaseItem(keyVal.Key, keyVal.Value)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}