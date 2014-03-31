using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acme.Sales.Pricing.Domain;

namespace Acme.Sales.Infrastructure.Repositories
{
    using DealFactory = Func<PurchaseBasket, PriceList, Deal>;
    public class DealRepository
    {
        private struct DealSpec
        {
            public string Name;
            public IEnumerable<PurchaseItem> For;
            public Func<PriceList, decimal> Discount;
        }

        private DealFactory DealFactoryConstructor(DealSpec dealSpec)
        {
          return (dealEligibleItems, priceList) =>
                 {
                    if (dealEligibleItems.Contains(dealSpec.For))
                    {
                        return new Deal(dealSpec.Name, dealSpec.Discount(priceList), dealSpec.For);
                    }
                    return Deal.NoDeal;
                 };
        }       

        private static readonly DealSpec[] specs = new DealSpec[]
        {
            new DealSpec()
            {
                Name = "10% off apples",
                For = new PurchaseItem[] {new PurchaseItem(new SKU("apples"), 1)},
                Discount = priceList => priceList["apples"] / 10m
            },
            new DealSpec()
            {
                Name = "Buy 2 tins of soup and get a loaf of bread for half price",
                For = new PurchaseItem[] {new PurchaseItem("soup", 2), new PurchaseItem("bread", 1)},
                Discount = priceList => priceList["bread"] / 2
            }
         };

        public IEnumerable<DealFactory> GetDealSpecs()
        {
            return specs.Select<DealSpec, DealFactory>(DealFactoryConstructor);
        }
    }
}
