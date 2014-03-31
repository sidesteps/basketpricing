using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Acme.Sales.Pricing.Domain;

namespace Acme.Sales.Tests.Unit
{
    using DealFactory = Func<PurchaseBasket, PriceList, Deal>;
    using ItemPrice = Tuple<SKU, decimal>;

    [TestClass]
    public class DealDealerTests
    {
        private static readonly PriceList priceList = new PriceList(new ItemPrice[]
                {
                    new ItemPrice("soup", 0.65m),
                    new ItemPrice("bread", 0.8m),
                    new ItemPrice("milk", 1.3m),
                    new ItemPrice("apples", 1m),
                });

        private DealFactory apples10pc = (basket, priceList) =>
                {
                    var dealItems = new PurchaseItem[] { new PurchaseItem("apples", 1) };
                    if (basket.Contains(dealItems))
                    {
                        var discount = priceList["apples"] / 10m;
                        return new Deal("10% off apples", discount, dealItems);
                    }
                    return Deal.NoDeal;
                };

        private DealFactory bread50pcFor2Soups = (basket, priceList) =>
        {
            var dealItems = new PurchaseItem[] { new PurchaseItem("soup", 2), new PurchaseItem("bread", 1) };
            if (basket.Contains(dealItems))
            {
                var discount = priceList["bread"] / 2m;
                return new Deal("50% off bread", discount, dealItems);
            }
            return Deal.NoDeal;
        };

        [TestMethod]
        public void WhenApplesPurchasedThenDealForApplesGiven()
        {
            var purchaseBasket = new PurchaseBasket(new SKU[] { "apples" });
            var dealSpecs = new DealFactory[] { this.apples10pc };

            var deals = new DealDealer().GivePurchaseDeals(purchaseBasket, priceList, dealSpecs);
            var expectedDeals = new Deal[]
            { 
                new Deal("10% off apples", 0.1m, new PurchaseItem[] { new PurchaseItem("apples", 1) }) 
            };
            Assert.IsTrue(deals.SequenceEqual(expectedDeals));
        }

        [TestMethod]
        public void When2ApplesPurchasedThen2AppleDealsGiven()
        {
            var purchaseBasket = new PurchaseBasket(new SKU[] { "apples", "apples" });
            var dealSpecs = new DealFactory[] { this.apples10pc };

            var deals = new DealDealer().GivePurchaseDeals(purchaseBasket, priceList, dealSpecs);
            var expectedDeals = new Deal[]
            { 
                new Deal("10% off apples", 0.1m, new PurchaseItem[] { new PurchaseItem("apples", 1) }),
                new Deal("10% off apples", 0.1m, new PurchaseItem[] { new PurchaseItem("apples", 1) })
             };
            Assert.IsTrue(deals.SequenceEqual(expectedDeals));
        }

        [TestMethod]
        public void When2SoupsAndBreadPurchasedThenGiveDiscountForBread()
        {
            var purchaseBasket = new PurchaseBasket(new SKU[] { "soup", "soup", "bread" });
            var dealSpecs = new DealFactory[] { this.bread50pcFor2Soups };

            var deals = new DealDealer().GivePurchaseDeals(purchaseBasket, priceList, dealSpecs);
            var expectedDeals = new Deal[]
            { 
                new Deal("50% off bread", 0.4m, new PurchaseItem[] { new PurchaseItem("soup", 2), new PurchaseItem("bread", 1) }) 
            };
            Assert.IsTrue(deals.SequenceEqual(expectedDeals));
        }

        [TestMethod]
        public void WhenNoMatchForDealSpecThenNoDealsGiven()
        {
            var purchaseBasket = new PurchaseBasket(new SKU[] { "soup", "soup", "bread" });
            var dealSpecs = new DealFactory[] { this.apples10pc };

            var deals = new DealDealer().GivePurchaseDeals(purchaseBasket, priceList, dealSpecs);
            Assert.IsTrue(deals.Count() == 0);
        }

        [TestMethod]
        public void WhenAppleAndBreadSpecsMatchThenGiveDealsForBoth()
        {
            var purchaseBasket = new PurchaseBasket(new SKU[] { "soup", "soup", "bread", "apples" });
            var dealSpecs = new DealFactory[] { this.apples10pc, this.bread50pcFor2Soups };

            var deals = new DealDealer().GivePurchaseDeals(purchaseBasket, priceList, dealSpecs);
            var expectedDeals = new Deal[]
            { 
                new Deal("10% off apples", 0.1m, new PurchaseItem[] { new PurchaseItem("apples", 1) }),
                new Deal("50% off bread", 0.4m, new PurchaseItem[] { new PurchaseItem("soup", 2), new PurchaseItem("bread", 1) })
             };
            Assert.IsTrue(deals.SequenceEqual(expectedDeals));
        }
    }
}
