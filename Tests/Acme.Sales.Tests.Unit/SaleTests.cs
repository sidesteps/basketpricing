using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Acme.Sales.Pricing.Domain;

namespace Acme.Sales.Tests.Unit
{
    using ItemPrice = Tuple<SKU, decimal>;

    [TestClass]
    public class SaleTests
    {
        private static readonly PriceList priceList = new PriceList(new ItemPrice[]
                {
                    new ItemPrice("soup", 0.65m),
                    new ItemPrice("bread", 0.8m),
                    new ItemPrice("milk", 1.3m),
                    new ItemPrice("apples", 1m),
                });

        [TestMethod]
        public void SaleTotalPriceEqualsPurchaseItemsPriceSum()
        {
            var purchaseBasket = new PurchaseBasket(new SKU[] { "apples", "bread", "soup" });
            Assert.AreEqual(new Sale(purchaseBasket, priceList).Price, 2.45m);
        }

        [TestMethod]
        public void SaleDiscountEqualsDealDiscountSum()
        {
            var purchaseBasket = new PurchaseBasket(new SKU[] { "apples", "apples" });
            var deals = new Deal[]
                {
                    new Deal("deal1", 0.2m, new PurchaseItem[]{new PurchaseItem("apples", 1)}),
                    new Deal("deal2", 0.3m, new PurchaseItem[]{new PurchaseItem("apples", 1)})
                };
            Assert.AreEqual(new Sale(purchaseBasket, priceList, deals).Discount, 0.5m);
        }

        [TestMethod]
        public void SalePriceEqualsPurchaseItemsPriceSum()
        {
            var purchaseBasket = new PurchaseBasket(new SKU[] { "apples", "bread" });
            var expectedSum = purchaseBasket.Select(item => item.Quantity * priceList[item.ItemId]).Sum();
            Assert.AreEqual(new Sale(purchaseBasket, priceList).Price, 1.8m);
        }

        [TestMethod]
        public void SaleTotalEqualsSalePriceMinusSaleDiscount()
        {
            var purchaseBasket = new PurchaseBasket(new SKU[] { "apples", "bread" });
            var expectedSum = purchaseBasket.Select(item => item.Quantity * priceList[item.ItemId]).Sum();
            var deals = new Deal[]
                {
                    new Deal("deal1", 0.2m, new PurchaseItem[]{new PurchaseItem("apples", 1)}),
                    new Deal("deal2", 0.3m, new PurchaseItem[]{new PurchaseItem("apples", 1)})
                };
            var expectedDiscount = deals.Sum(deal => deal.Discount);
            var expectedTotal = expectedSum - expectedDiscount;
            Assert.AreEqual(new Sale(purchaseBasket, priceList, deals).Total, expectedTotal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCannotExceedPrice()
        {
            var purchaseBasket = new PurchaseBasket(new SKU[] { "apples", "apples" });
            var deals = new Deal[]
                {
                    new Deal("deal1", 1m, new PurchaseItem[]{new PurchaseItem("apples", 1)}),
                    new Deal("deal2", 1.1m, new PurchaseItem[]{new PurchaseItem("apples", 1)})
                };
            new Sale(purchaseBasket, priceList, deals);
        }
    }
}
