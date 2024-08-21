using CodingChallangeWebApi.DataAccess;
using CodingChallangeWebApi.Models.DataEntityModels;
using CodingChallangeWebApi.Models.DTOModels;

namespace CodingChallangeWebApi.BusinessLogic
{
    public class OrderCancelBusinessLogic
    {
        private const string cancelledStatus = "Cancelled";
        public CancelOrderDTO CancelOrder(int orderId)
        {
            WebAPICall webApiCall;
            Order? order = null;
            PaymentProvider? paymentPorvider = null;
            CancelOrderDTO cancelOrderDTO = null;

            try
            {

                try
                {
                    webApiCall = new WebAPICall();
                    cancelOrderDTO = new CancelOrderDTO();

                    using (var context = new CodingChallangeContext())
                    {
                        order = context.Orders.FirstOrDefault(i => i.Id == orderId);
                        paymentPorvider = context.PaymentProviders.FirstOrDefault(i => i.Id == order.PaymentProviderSelected);
                        if (order == null)
                        {
                            return null;
                        }
                        else
                        {
                            cancelOrderDTO.cancelationStatus = webApiCall.CancleOrderApiCall(paymentPorvider.Apiurl,
                                                                                              paymentPorvider.ApiKeyEncrypted,
                                                                                              order.ProviderOrderId);

                            if (cancelOrderDTO.cancelationStatus == true)
                            {
                                order.OrderStatus = cancelledStatus;
                                context.Update(order);
                                context.SaveChanges();

                            }
                            return cancelOrderDTO;
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Error Occured while getting order list");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured while Deleting Data");
            }

        }
    }
}
