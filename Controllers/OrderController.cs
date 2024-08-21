using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using CodingChallangeWebApi.BusinessLogic;
using CodingChallangeWebApi.Models.DataEntityModels;
using CodingChallangeWebApi.Models.DTOModels;
using AutoMapper;
using CodingChallangeWebApi.DataAccess;

namespace CodingChallangeWebApi.Controllers
{


    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrderController : Controller
    {


        [HttpPost]
        public ActionResult CreateOrder(OrderDTO orderInformationDTO)
        {

            ApiResponse<Order> apiResponse = new ApiResponse<Order>();
            try
            {
                var OrderBusiensLogic = new OrderCreateBusinessLogic();

                apiResponse.Data = OrderBusiensLogic.CreateOrder(orderInformationDTO);
                apiResponse.StatusSucess = true;

                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                apiResponse.StatusSucess = false;
                apiResponse.ErrorMessage = "An Error has Occured While Processing the Request";
                return BadRequest(apiResponse);
            }
        }

        [HttpGet]
        public ActionResult GetOrders()
        {
            ApiResponse<List<Order>> apiResponse = new ApiResponse<List<Order>>();
            try
            {
                var OrderBusiensLogic = new OrderGetBusinessLogic();
                apiResponse.Data = OrderBusiensLogic.GetOrders();

                if (apiResponse.Data != null)
                {
                    apiResponse.StatusSucess = true;
                    return Ok(apiResponse);
                }
                else
                {
                    apiResponse.StatusSucess = false;
                    apiResponse.ErrorMessage = "Orders Not Found";
                    return NotFound(apiResponse);
                }

            }
            catch (Exception ex)
            {
                apiResponse.StatusSucess = false;
                apiResponse.ErrorMessage = "An Error has Occured While Processing the Request";
                return BadRequest(apiResponse);
            }

        }

        [HttpGet("{orderId:int}")]
        public ActionResult GetOrder(int orderId)
        {
            ApiResponse<OrderDetailsDTO> apiResponse = new ApiResponse<OrderDetailsDTO>();
            try
            {
                var OrderBusiensLogic = new OrderGetBusinessLogic();
                apiResponse.Data = OrderBusiensLogic.GetOrder(orderId);
                if (apiResponse.Data != null)
                {
                    apiResponse.StatusSucess = true;
                    return Ok(apiResponse);
                }
                else
                {
                    apiResponse.StatusSucess = false;
                    apiResponse.ErrorMessage = "Orders Not Found";
                    return NotFound(apiResponse);
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusSucess = false;
                apiResponse.ErrorMessage = "An Error has Occured While Processing the Request";
                return BadRequest(apiResponse);
            }

        }

        [HttpPut("Cancel/{orderId:int}")]
        public ActionResult CancelOrder(int orderId)
        {
            ApiResponse<CancelOrderDTO> apiResponse = new ApiResponse<CancelOrderDTO>();
            try
            {
                var OrderBusiensLogic = new OrderCancelBusinessLogic();

                apiResponse.Data = OrderBusiensLogic.CancelOrder(orderId);
                if (apiResponse.Data != null)
                {
                    apiResponse.StatusSucess = true;
                    return Ok(apiResponse);
                }
                else
                {
                    apiResponse.StatusSucess = false;
                    apiResponse.ErrorMessage = "Orders Not Found or Cannot Be Cancelled";
                    return NotFound(apiResponse);
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusSucess = false;
                apiResponse.ErrorMessage = "An Error has Occured While Processing the Request";
                return BadRequest(apiResponse);
            }

        }

        [HttpPut("Pay/{orderId:int}")]
        public ActionResult PayOrder(int orderId)
        {

            ApiResponse<PayOrderDTO> apiResponse = new ApiResponse<PayOrderDTO>();
            try
            {
                var OrderBusiensLogic = new OrderPayBusinessLogic();

                apiResponse.Data = OrderBusiensLogic.PayOrder(orderId);
                if (apiResponse.Data != null)
                {
                    apiResponse.StatusSucess = true;
                    return Ok(apiResponse);
                }
                else
                {
                    apiResponse.StatusSucess = false;
                    apiResponse.ErrorMessage = "Orders Not Found or Cannot Be Paid";
                    return NotFound(apiResponse);
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusSucess = false;
                apiResponse.ErrorMessage = "An Error has Occured While Processing the Request";
                return BadRequest(apiResponse);
            }
        }
    }
}
