using CodingChallangeWebApi.Models.DataEntityModels;
using CodingChallangeWebApi.Models.DTOModels;

namespace CodingChallangeWebApi.BusinessLogic
{
    public class OrderGetBusinessLogic
    {
        public List<Order> GetOrders()
        {
            try
            {
                using (var context = new CodingChallangeContext())
                {

                    return context.Orders.ToList();

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured while getting order list");
            }

        }

        public OrderDetailsDTO GetOrder(int orderId)
        {
            OrderDetailsDTO orderDetailsDTO;
            List<OrderProductItem> lsOrderProductItem;
            OrderProductItem productDetail;

            Order orderFromEntity;
            PaymentProvider orderPaymentProvider;
            PaymentTypeAndRule paymentTypeSelected;

            try
            {
                using (var context = new CodingChallangeContext())
                {
                    orderFromEntity = context.Orders.FirstOrDefault(i => i.Id == orderId);

                    if (orderFromEntity != null)
                    {
                        orderDetailsDTO = new OrderDetailsDTO();
                        lsOrderProductItem = new List<OrderProductItem>();

                        orderDetailsDTO.Id = orderFromEntity.Id;
                        orderDetailsDTO.OrderStatus = orderFromEntity.OrderStatus;
                        orderDetailsDTO.TotalAmount = orderFromEntity.PaymentValuePostComission;

                        orderPaymentProvider = context.PaymentProviders.FirstOrDefault(i => i.Id == orderFromEntity.PaymentProviderSelected.Value);
                        orderDetailsDTO.PaymentProvider = orderPaymentProvider.ProviderName;

                        paymentTypeSelected = context.PaymentTypeAndRules.FirstOrDefault(i => i.Id == orderFromEntity.OptimalPaymentMethodSelected);
                        orderDetailsDTO.PaymentType = paymentTypeSelected.Type;

                        orderDetailsDTO.ProviderOrderId = orderFromEntity.ProviderOrderId;
                        foreach (OrderDetail orderDetailFromEntity in context.OrderDetails.Where(i => i.OrderId == orderDetailsDTO.Id))
                        {
                            productDetail = new OrderProductItem();
                            var productInformation = context.ProductDetails.FirstOrDefault(i => i.Id == orderDetailFromEntity.ProductId);
                            productDetail.ProductName = productInformation.ProductName;
                            productDetail.ProductDescription = productInformation.ProductDescription;
                            lsOrderProductItem.Add(productDetail);
                        }
                        orderDetailsDTO.OrderProductItems = lsOrderProductItem;

                        return orderDetailsDTO;

                    }
                    else
                    {

                        return null;
                    }



                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured while getting order list");
            }

        }
    }
}
