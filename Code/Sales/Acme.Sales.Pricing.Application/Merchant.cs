using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acme.Sales.Infrastructure.Repositories;
using Acme.Sales.Pricing.Domain;

namespace Acme.Sales.Pricing.Application
{
    public class Merchant
    {
        public Sale MakeSale(IEnumerable<string> purchaseItems)
        {
            var dealSpecs = new DealRepository().GetDealSpecs();
            var priceList = new PriceRepository().GetPriceList();
            var basket = new PurchaseBasket(purchaseItems.Select(item => new SKU(item)));
            var deals = new DealDealer().GivePurchaseDeals(basket, priceList, dealSpecs);
            return new Sale(basket, priceList, deals);
        }
    }
}
