using Asp.Versioning;
using CodingChallangeWebApi.BusinessLogic;
using CodingChallangeWebApi.DataAccess;
using CodingChallangeWebApi.Models.DataEntityModels;
using CodingChallangeWebApi.Models.DTOModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CodingChallangeWebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : Controller
    {
        [EnableCors("allowSpecificOriginsPolicy")]
        [HttpGet]
        public ActionResult GetProducts()
        {
            ApiResponse<List<ProductDetail>> apiResponse = new ApiResponse<List<ProductDetail>>();
            try
            {
                var OrderBusiensLogic = new ProductGetBusinessLogic();

                apiResponse.Data = OrderBusiensLogic.GetProducts();
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
    }
}
