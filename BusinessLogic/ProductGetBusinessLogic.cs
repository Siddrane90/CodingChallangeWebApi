using CodingChallangeWebApi.Models.DataEntityModels;
using CodingChallangeWebApi.Models.DTOModels;

namespace CodingChallangeWebApi.BusinessLogic
{
    public class ProductGetBusinessLogic
    {

        public List<ProductDetail> GetProducts()
        {
            try
            {
                using (var context = new CodingChallangeContext())
                {

                    return context.ProductDetails.ToList();
                }
                    
            }
            catch (Exception ex)
            {
                throw new Exception("An Error Occured While Getting Products");
            }
        }
    }
}
