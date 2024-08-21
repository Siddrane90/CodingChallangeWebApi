namespace CodingChallangeWebApi.Models.DTOModels
{
    public class PaymentProviderOrderDTO
    {
        public string? method { get; set; }
        public List<Product>? products { get; set; }
    }

    public class Product
    {
        public string? name { get; set; }
        public decimal? unitPrice { get; set; }

    }

}
