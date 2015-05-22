using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class FedExShipping : ShippingBase
    {
        public IEnumerable<RegionShippingCost> FedExCosts { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Shipping to {0}", lineItem.DeliveryRegion);
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            return
                (from c in FedExCosts
                 where c.DestinationRegion == lineItem.DeliveryRegion
                 select c.Amount).Single();
        }
    }
}