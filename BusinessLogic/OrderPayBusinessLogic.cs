using CodingChallangeWebApi.DataAccess;
using CodingChallangeWebApi.Models.DataEntityModels;
using CodingChallangeWebApi.Models.DTOModels;

namespace CodingChallangeWebApi.BusinessLogic
{
    public class OrderPayBusinessLogic
    {
        private const string paymentStatus = "Paid";

        public PayOrderDTO PayOrder(int orderId)
        {
            WebAPICall webApiCall;
            Order? order = null;
            PaymentProvider? paymentPorvider = null;
            PayOrderDTO payOrderDTO = null;
            try
            {
                webApiCall = new WebAPICall();
                payOrderDTO = new PayOrderDTO();

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
                        payOrderDTO.paymentStatus = webApiCall.PayOrderApiCall(paymentPorvider.Apiurl,
                                                                                          paymentPorvider.ApiKeyEncrypted,
                                                                                          order.ProviderOrderId);

                        if (payOrderDTO.paymentStatus == true)
                        {
                            order.OrderStatus = paymentStatus;
                            context.Update(order);
                            context.SaveChanges();

                        }
                        return payOrderDTO;
                    }



                }


            }
            catch (Exception ex)
            {
                throw new Exception("Error Processing Payment");
            }
        
        }
    }
}
