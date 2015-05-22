using System.Linq;

namespace Marketplace.Interview.Business.Basket
{
    public class AddToBasketCommand : BasketOperationBase, IAddToBasketCommand
    {
        public AddToBasketResponse Invoke(AddToBasketRequest request)
        {
            var basket = GetBasket();
            request.LineItem.ShippingAmount = request.LineItem.Shipping.GetAmount(request.LineItem, basket);
            string nameShippingOption = request.LineItem.Shipping.ToString();
            if (nameShippingOption.Contains("FedExShipping"))
            {
                bool exist = basket.LineItems.Any(item => (item.DeliveryRegion.Equals(request.LineItem.DeliveryRegion))
                        && (item.SupplierId.Equals(request.LineItem.SupplierId)));
                if(exist)
                    request.LineItem.ShippingAmount = request.LineItem.Shipping.GetAmount(request.LineItem, basket) - LineItem.deductPrice;               
            }              
            request.LineItem.Id = basket.LineItems.MaxOrDefault(li => li.Id) + 1;

            basket.LineItems.Add(request.LineItem);

            SaveBasket(basket);

            return new AddToBasketResponse(){LineItemCount = basket.LineItems.Count};
        }
    }

    public class AddToBasketRequest
    {
        public LineItem LineItem { get; set; }
    }

    public class AddToBasketResponse
    {
        public int LineItemCount { get; set; }
    }
}