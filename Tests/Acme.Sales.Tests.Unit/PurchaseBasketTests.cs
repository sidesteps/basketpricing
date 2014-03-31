using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acme.Sales.Pricing.Domain;

namespace Acme.Sales.Tests.Unit
{
    [TestClass]
    public class PurchaseBasketTests
    {
        [TestMethod]
        public void WhenBasketContainsOneSKUThenBasketFindsPurchaseItemWithQuantityOne()
        {
            PurchaseBasket pb = new PurchaseBasket(new SKU[] { "apples" });
            Assert.IsTrue(pb.Contains(new PurchaseItem("apples", 1)));
        }

        [TestMethod]
        public void WhenPurchaseItemQuantityExceedsSKUsThenBasketContainsReturnsFalse()
        {
            PurchaseBasket pb = new PurchaseBasket(new SKU[] { "apples" });
            var apples = new PurchaseItem("apples", 2);
            Assert.IsFalse(pb.Contains(apples));
        }
        
        [TestMethod]
        public void WhenRemovingItemsThenOriginalBasketRemainsUnmodified()
        {
            PurchaseBasket pb = new PurchaseBasket(new SKU[] { "apples" });
            var apples = new PurchaseItem("apples", 1);
            pb.Remove(apples);
            Assert.IsTrue(pb.Contains(apples));
        }

        [TestMethod]
        public void WhenRemovingItemsThenNewBasketDoesNotContainItems()
        {
            PurchaseBasket pb = new PurchaseBasket(new SKU[] { "apples" });
            var apples = new PurchaseItem("apples", 1);
            var newBasket = pb.Remove(apples);
            Assert.IsFalse(newBasket.Contains(apples));
        }
    }
}
