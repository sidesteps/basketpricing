using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acme.Sales.Pricing.Domain;
using Acme.Sales.Pricing.Application;

namespace Acme.StoreOperations.UI.ConsoleTill
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var sale = new Merchant().MakeSale(args);
                Console.Write(FormatReceipt(sale));
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
                if (e.InnerException != null)
                {
                    System.Console.Error.WriteLine(e.InnerException.Message);
                }
            }
        }

        private static string FormatReceipt(Sale sale)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Subtotal: {0:C}\n", sale.Price);
            if (sale.Deals == null || sale.Deals.Count() == 0)
            {
                sb.AppendLine("(No offers available)");
            }
            else
            {
                foreach (var deal in sale.Deals)
                {
                    sb.AppendFormat("{0}: -{1:C}\n", deal.Name, deal.Discount);
                }
            }
            sb.AppendFormat("Total: {0:C}", sale.Total);
            return sb.ToString();
        }
    }
}
