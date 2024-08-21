namespace CodingChallangeWebApi.Models.DTOModels
{
    public class OrderDetailsDTO
    {
        public int? Id { get; set; }
        public string ProviderOrderId { get; set; }
        public decimal? TotalAmount {  get; set; }
       public string OrderStatus { get; set; }
        public string PaymentProvider {  get; set; }
        public string PaymentType { get; set; }

       public List<OrderProductItem> OrderProductItems { get; set; }

    }

    public class OrderProductItem
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }


    }
}
