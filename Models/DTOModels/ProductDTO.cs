namespace CodingChallangeWebApi.Models.DTOModels
{
    public class ProductDTO
    {
        public string ProductName { get; set; }
        public decimal PricePerUnit { get; set; }

        public int OrderQuantity { get; set; }
    }
}
