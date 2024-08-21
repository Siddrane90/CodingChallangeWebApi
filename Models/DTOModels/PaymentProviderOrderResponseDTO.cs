namespace CodingChallangeWebApi.Models.DTOModels
{
    public class PaymentProviderOrderResponseDTO
    {
        public string orderID { get; set; }
        public decimal amount { get; set; }
        public List<Fee> fees { get; set; }
        List<Product> products { get; set; }
        public string createdDate {  get; set; }
        public string createdBy { get; set; }

        public string status { get; set; }
    }

    public class Fee
    {
        public string name { get; set; }
        public decimal amount { get; set; }
    }
}
