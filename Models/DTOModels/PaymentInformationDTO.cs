using CodingChallangeWebApi.Models.DataEntityModels;

namespace CodingChallangeWebApi.Models.DTOModels
{
    public class PaymentInformationDTO
    {
        public PaymentTypeAndRule SelectedPaymentRule { get; set; }

        public decimal PaymentAmountBeforeComission { get; set; }
        public decimal? PaymentAmountAfterComission { get; set; }


    }
}
