using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Acme.Sales.Pricing.Domain
{
    /// <summary>
    /// Represents a sale of purchase items.
    /// </summary>
    public class Sale
    {
        private PurchaseBasket purchaseBasket;
        private PriceList priceList;

        public Sale(PurchaseBasket basket, PriceList priceList, IEnumerable<Deal> deals = null)
        {
            if(basket == null)
                throw new ArgumentNullException("Cannot make sale without items to sell");
            if(priceList == null)
                throw new ArgumentNullException("Need item prices to make sale");
            this.priceList = priceList;
            this.purchaseBasket = basket;
            this.Deals = deals;
            if (Total <= 0)
                throw new ArgumentException("Discounts given to the sale cannot exceed sale price");

        }

        /// <summary>
        /// Deals given to the sale
        /// </summary>
        public IEnumerable<Deal> Deals
        {
            get;
            private set;
        }

        /// <summary>
        /// Cost of purchase items for this sale
        /// </summary>
        public decimal Price
        {
            get
            {
                return (from purchaseItem in purchaseBasket
                        select purchaseItem.Quantity * priceList[purchaseItem.ItemId]).Sum();
            }
        }

        /// <summary>
        /// Discount given to the sale
        /// </summary>
        public decimal Discount
        {
            get
            {
                return Deals == null ? 0 : Deals.Sum(deal => deal.Discount);
            }
        }

        /// <summary>
        /// Grand total of the sale after discounts
        /// </summary>
        public decimal Total
        {
            get
            {
                return Price - Discount;
            }
        }
    }
}
