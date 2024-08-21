using CodingChallangeWebApi.Models.DTOModels;
using Newtonsoft.Json;
using System.Text;


namespace CodingChallangeWebApi.DataAccess
{
    public class WebAPICall
    {

        public PaymentProviderOrderResponseDTO CreateNewOrder(string baseUrl, string apiKey, PaymentProviderOrderDTO paymentProviderOrder)
        {
            PaymentProviderOrderResponseDTO paymentProviderResult;
            string? serializedJsonData;
            try
            {
                serializedJsonData = (CallWebAPI(baseUrl,
                                                 "order",
                                                 apiKey,
                                                 HttpMethod.Post,
                                                 JsonConvert.SerializeObject(paymentProviderOrder).ToString()
                                                 )).Result.ToString();

                paymentProviderResult = JsonConvert.DeserializeObject<PaymentProviderOrderResponseDTO>(serializedJsonData);

                return paymentProviderResult;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool CancleOrderApiCall(string baseUrl, string apiKey, string providorOrderId)
        {
            string cancleResult;
            try
            {
                cancleResult = (string)(CallWebAPI(baseUrl,
                                                    String.Format("cancel?id={0}", providorOrderId),
                                                            apiKey,
                                                            HttpMethod.Put,
                                                            string.Empty
                                                            )).Result;

                if (cancleResult == string.Empty)
                {
                    return true;
                }
                else
                {
                    return false;

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool PayOrderApiCall(string baseUrl, string apiKey, string providorOrderId)
        {
            string payResult;
            try
            {
                payResult = (string)(CallWebAPI(baseUrl,
                                                    String.Format("pay?id={0}", providorOrderId),
                                                            apiKey,
                                                            HttpMethod.Put,
                                                            string.Empty
                                                            )).Result;
                if (payResult == string.Empty)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<Object> CallWebAPI(string baseUrl, string route, string apiKey, HttpMethod httpMethod, string jsonContent)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var httprequest = new HttpRequestMessage(httpMethod, string.Join("", baseUrl, route));
                    httprequest.Headers.Add("x-api-key", apiKey);
                    httprequest.Headers.Add("Accept", "application/json");
                    httprequest.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    try
                    {
                        using (HttpResponseMessage response = client.SendAsync(httprequest).Result)
                        {
                            if (!response.IsSuccessStatusCode)
                            {
                                throw new Exception(response.Content.ToString());
                            }

                            string? responseContent = await response.Content.ReadAsStringAsync();

                            return responseContent;
                        }

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /*
                public async Task<PaymentProviderOrderResponseDTO> CreateNewOrder(string baseUrl, string apiKey, PaymentProviderOrderDTO paymentProviderOrder)
                {
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(baseUrl);
                        client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                        var json = JsonConvert.SerializeObject(paymentProviderOrder);
                        var content = new StringContent(json,Encoding.UTF8, "application/json");
                        var response = client.PostAsync("order",content).Result;
                        if(response.IsSuccessStatusCode)
                        {
                            return null;
                        }
                        else
                        {

                            return null;
                        }
                    }

                    catch (Exception ex)
                    {
                        return null;
                    }

                }
        */
    }
}
