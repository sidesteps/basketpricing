using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acme.Sales.Pricing.Domain
{
    using DealFactory = Func<PurchaseBasket, PriceList, Deal>;
    /// <summary>
    /// Governs issuance of deals for a purchase
    /// </summary>
    public class DealDealer
    {
        public IEnumerable<Deal> GivePurchaseDeals(PurchaseBasket basket, PriceList priceList, IEnumerable<DealFactory> dealFactories)
        {
            var dealEligibleItems = basket;
            var deal = Deal.NoDeal;
            //TODO: implement optimal deal selection algorithm ala knapsack
            foreach (var dealFactory in dealFactories)
            {
                while ((deal = dealFactory(dealEligibleItems, priceList)) != Deal.NoDeal)
                {
                    dealEligibleItems = dealEligibleItems.Remove(deal.ForPurchase);
                    yield return deal;
                }
            }
        }
    }
}
