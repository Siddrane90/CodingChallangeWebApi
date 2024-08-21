using Asp.Versioning;
using CodingChallangeWebApi.DataAccess;
using CodingChallangeWebApi.Models.DataEntityModels;
using CodingChallangeWebApi.Models.DTOModels;
using System.Reflection.Metadata;



namespace CodingChallangeWebApi.BusinessLogic
{
    public class OrderCreateBusinessLogic
    {
        private const string efectivo = "Cash";
        private const string tarjetaDeCredito = "Card";
        private const string transferencia = "Transfer";
        private const string commissionTypeFlat = "Flat";
        private const string comissionTypePercent = "Percent";
        public Order CreateOrder(OrderDTO orderInformation)
        {
            Order createdOrder;

            PaymentInformationDTO? selectedPaymentInformation;
            decimal totalPaymentAmount;
            try
            {
                //Map OrderDTO To Order Entity Object
                using (var context = new CodingChallangeContext())
                {
                    totalPaymentAmount = CalculateTotalPaymentAmount(orderInformation.Products.ToList());
                    selectedPaymentInformation = SelectOptimumPaymentMethod(orderInformation.PaymentMethod,
                                                                            totalPaymentAmount,
                                                                            context.PaymentProviders.ToList(),
                                                                            context.PaymentTypeAndRules.ToList());
                    createdOrder = CreateOrderDetails(orderInformation,
                                                      selectedPaymentInformation,
                                                      context.ProductDetails.ToList());

                     var paymentProviderOrderResponseDTO = CallProviderAPI(selectedPaymentInformation.SelectedPaymentRule.Provider.Apiurl,
                                                                   selectedPaymentInformation.SelectedPaymentRule.Provider.ApiKeyEncrypted,
                                                                   createdOrder);
                    createdOrder.ProviderOrderId = paymentProviderOrderResponseDTO.orderID;
                    createdOrder.OrderStatus = paymentProviderOrderResponseDTO.status;
                    createdOrder.ProviderOrderCreationDate = paymentProviderOrderResponseDTO.createdDate;

                    context.Add(createdOrder);
                    context.SaveChanges();
                    return createdOrder;
                }

            }
            catch (Exception exception)
            {
                throw new Exception("Error Creating Order Request"); ;
            }
        }

        private PaymentProviderOrderResponseDTO CallProviderAPI(string baseurl, string apiKey, Order createdOrder)
        {
            PaymentProviderOrderDTO paymentProviderOrder;
            PaymentProviderOrderResponseDTO paymentProviderOrderResponseDTO;
            Product product;
            List<Product> lsproducts;
            try
            {

                WebAPICall webApiCall = new WebAPICall();
                paymentProviderOrder = new PaymentProviderOrderDTO();
                lsproducts = new List<Product>();

                paymentProviderOrder.method = createdOrder.OptimalPaymentMethodSelectedNavigation.Type;

                foreach (OrderDetail orderDetail in createdOrder.OrderDetails)
                {
                    product = new Product();
                    product.name = orderDetail.Product.ProductName;
                    product.unitPrice = orderDetail.Product.PricePerUnit;

                    lsproducts.Add(product);
                }
                paymentProviderOrder.products = lsproducts;

                paymentProviderOrderResponseDTO = (PaymentProviderOrderResponseDTO)(webApiCall.CreateNewOrder(baseurl, apiKey, paymentProviderOrder));

                return paymentProviderOrderResponseDTO;

            }
            catch (Exception exception)
            {
                throw new Exception("Error Calling Payment Provider API.");
            }
        }

        private decimal CalculateTotalPaymentAmount(List<ProductDTO> lsProductDetails)
        {
            decimal totalPaymentAmount = 0;
            try
            {
                foreach (ProductDTO product in lsProductDetails)
                {
                    totalPaymentAmount += product.PricePerUnit;
                }

                return totalPaymentAmount;
            }
            catch (Exception ex)
            {
                throw new Exception("Error calculating Toal Payment");
            }


        }

        private PaymentInformationDTO? SelectOptimumPaymentMethod(string paymentMethod,
                                                                 decimal paymentAmount,
                                                                List<PaymentProvider> paymentProviders,
                                                                List<PaymentTypeAndRule> paymentTypesRules)
        {
            PaymentInformationDTO? paymentInformationDTO = new PaymentInformationDTO();
            try
            {
                switch (paymentMethod)
                {
                    case efectivo:
                        paymentInformationDTO = OptimalSelection(paymentAmount,
                                                                         paymentTypesRules.Where(i => i.Type == efectivo).ToList(),
                                                                         paymentProviders);
                        break;
                    case tarjetaDeCredito:
                        paymentInformationDTO = OptimalSelection(paymentAmount,
                                                                         paymentTypesRules.Where(i => i.Type == tarjetaDeCredito).ToList(),
                                                                         paymentProviders);
                        break;
                    case transferencia:
                        paymentInformationDTO = OptimalSelection(paymentAmount,
                                                                         paymentTypesRules.Where(i => i.Type == transferencia).ToList(),
                                                                         paymentProviders);
                        break;
                    default:
                        paymentInformationDTO = null;
                        break;
                }

                return paymentInformationDTO;
            }
            catch (Exception exception)
            {
                throw new Exception("Error Selecting Optimal Paymment method");
            }

        }

        private PaymentInformationDTO? OptimalSelection(decimal paymentAmount,
                                                               List<PaymentTypeAndRule> paymentTypesRulesEfectivo,
                                                               List<PaymentProvider> paymentProviders)
        {
            List<PaymentInformationDTO> lsPaymentAmountsCalculated;
            PaymentInformationDTO? optimalPaymentInformationSelectedDTO;
            PaymentInformationDTO? paymentDataWithAssignedProviders;
            PaymentProvider? paymentProviderInfo;
            try
            {
                lsPaymentAmountsCalculated = new List<PaymentInformationDTO>();
                paymentProviderInfo = null;
                optimalPaymentInformationSelectedDTO = null;


                foreach (PaymentTypeAndRule paymentRule in paymentTypesRulesEfectivo)
                {
                    paymentProviderInfo = paymentProviders.FirstOrDefault(i => i.Id == paymentRule.ProviderId);
                    paymentDataWithAssignedProviders = AssignPaymentData(paymentAmount, paymentRule, paymentProviderInfo);
                    lsPaymentAmountsCalculated.Add(paymentDataWithAssignedProviders);

                }

                optimalPaymentInformationSelectedDTO = FindOptimalPaymentMethod(lsPaymentAmountsCalculated);

                return optimalPaymentInformationSelectedDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error OptimalPayment Method");
            }
        }


        private PaymentInformationDTO? AssignPaymentData(decimal paymentAmount, PaymentTypeAndRule? paymentRule, PaymentProvider? paymentProvider)
        {
            PaymentInformationDTO paymentInformationCalculatedDTO;
            try
            {
                paymentInformationCalculatedDTO = new PaymentInformationDTO();
                paymentInformationCalculatedDTO.PaymentAmountAfterComission = PaymentAmountCalculationLogic(paymentAmount, paymentRule);
                paymentInformationCalculatedDTO.PaymentAmountBeforeComission = paymentAmount;
                paymentInformationCalculatedDTO.SelectedPaymentRule = paymentRule;

                return paymentInformationCalculatedDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Assigning Payment Data");
            }
        }


        private PaymentInformationDTO? FindOptimalPaymentMethod(List<PaymentInformationDTO> lsPaymentAmountsCalculated)
        {
            PaymentInformationDTO? optimalPaymentInformationSelectedDTO;
            try
            {
                optimalPaymentInformationSelectedDTO = null;

                if (lsPaymentAmountsCalculated.Any())
                {
                    var listOfNonNullPayments = lsPaymentAmountsCalculated
                                                .Where(i => i.PaymentAmountAfterComission != null);
                    if (listOfNonNullPayments.Any())
                    {
                        optimalPaymentInformationSelectedDTO = listOfNonNullPayments.OrderBy(o => o.PaymentAmountAfterComission).FirstOrDefault();
                    }
                    else
                    {
                        optimalPaymentInformationSelectedDTO = null;
                    }
                }

                return optimalPaymentInformationSelectedDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Finding Optimal Payment Method");
            }

        }

        private decimal? PaymentAmountCalculationLogic(decimal paymentAmount, PaymentTypeAndRule paymentRule)
        {
            decimal? paymentAmountAfterComission = null;
            try
            {
                //Null Min or Max Amount means its infinate
                if (paymentRule.MinimumAmount != null && paymentRule.MaximumAmount != null)
                {
                    if (paymentAmount >= paymentRule.MinimumAmount && paymentAmount <= paymentRule.MaximumAmount)
                    {
                        paymentAmountAfterComission = CalculateAmountAfterComission(paymentAmount, paymentRule);
                    }
                    else
                    {
                        //logic Not applicable
                        paymentAmountAfterComission = null;
                    }
                }
                else if (paymentRule.MinimumAmount == null && paymentRule.MaximumAmount != null)
                {
                    if (paymentAmount <= paymentRule.MaximumAmount)
                    {
                        paymentAmountAfterComission = CalculateAmountAfterComission(paymentAmount, paymentRule);
                    }
                    else
                    {
                        //Logic Not applicable
                        paymentAmountAfterComission = null;
                    }
                }
                else if (paymentRule.MinimumAmount != null && paymentRule.MaximumAmount == null)
                {
                    if (paymentAmount >= paymentRule.MinimumAmount)
                    {
                        paymentAmountAfterComission = CalculateAmountAfterComission(paymentAmount, paymentRule);
                    }
                    else
                    {
                        //Logic Not applicable
                        paymentAmountAfterComission = null;
                    }

                }
                else
                {
                    paymentAmountAfterComission = CalculateAmountAfterComission(paymentAmount, paymentRule);
                }

                return paymentAmountAfterComission;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Performing Payment Calculation Logic");
            }

        }

        private decimal? CalculateAmountAfterComission(decimal paymentAmount, PaymentTypeAndRule paymentRule)
        {
            decimal? paymentAfterComission = null;
            try
            {
                if (paymentRule.ComissionType == commissionTypeFlat)
                {
                    paymentAfterComission = paymentAmount + paymentRule.ComissionValue;
                }
                else if (paymentRule.ComissionType == comissionTypePercent)
                {
                    paymentAfterComission = paymentAmount + (paymentAmount * (paymentRule.ComissionValue / 100));
                }
                else
                {
                    paymentAfterComission = null;
                }

                return paymentAfterComission;
            }
            catch (Exception excpetion)
            {

                throw new Exception("Error Calculating Amount After Comission");
            }

        }


        private Order CreateOrderDetails(OrderDTO orderInformation,
            PaymentInformationDTO paymentInformation,
            List<ProductDetail> lsProductDetails)
        {
            Order newOrder;
            OrderDetail newOrderDetail;
            List<OrderDetail> lsNewOrderDetails;
            try
            {
                newOrder = new Order();
                lsNewOrderDetails = new List<OrderDetail>();
                newOrder.Id = null;
                newOrder.UserId = orderInformation.UserId;
                newOrder.OrderStatus = "Created";
                newOrder.PaymentProviderSelectedNavigation = paymentInformation.SelectedPaymentRule.Provider;
                newOrder.OptimalPaymentMethodSelectedNavigation = paymentInformation.SelectedPaymentRule;
                newOrder.PaymentValuePreComission = paymentInformation.PaymentAmountBeforeComission;
                newOrder.PaymentValuePostComission = paymentInformation.PaymentAmountAfterComission;

                foreach (ProductDTO productInfo in orderInformation.Products)
                {

                    newOrderDetail = new OrderDetail();

                    newOrderDetail.Id = null;
                    newOrderDetail.Product = lsProductDetails.FirstOrDefault(i => i.ProductName == productInfo.ProductName);
                    newOrderDetail.OrderQuantity = productInfo.OrderQuantity;
                    newOrderDetail.Order = newOrder;
                    lsNewOrderDetails.Add(newOrderDetail);
                }
                newOrder.OrderDetails = lsNewOrderDetails;

                return newOrder;

            }
            catch (Exception ex)
            {
                throw new Exception("Error Creating Order Details Object");
            }

        }
    }
}

