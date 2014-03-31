using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acme.Sales.Pricing.Domain;

namespace Acme.Sales.Infrastructure.Repositories
{
    using ItemPrice = Tuple<SKU, decimal>;
    public class PriceRepository
    {
        public PriceList GetPriceList()
        {
            return new PriceList(new ItemPrice[]
                {
                    new ItemPrice("soup", 0.65m),
                    new ItemPrice("bread", 0.8m),
                    new ItemPrice("milk", 1.3m),
                    new ItemPrice("apples", 1m),
                }); ;
        }
    }
}
