namespace CodingChallangeWebApi.Models.DTOModels
{
    public class OrderDTO
    {
        public string PaymentMethod { get; set; }
        public int UserId { get; set; }
        public ICollection<ProductDTO> Products { get; set; }
    }

}
