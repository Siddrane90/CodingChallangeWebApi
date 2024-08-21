using System;
using System.Collections.Generic;

namespace CodingChallangeWebApi.Models.DataEntityModels;

public partial class Order
{
    public int? Id { get; set; }

    public int? UserId { get; set; }

    public string? OrderStatus { get; set; }

    public decimal? PaymentValuePreComission { get; set; }

    public int? PaymentProviderSelected { get; set; }

    public int? OptimalPaymentMethodSelected { get; set; }

    public decimal? PaymentValuePostComission { get; set; }

    public string? ProviderOrderId { get; set; }

    public string? ProviderOrderCreationDate { get; set; }

    public virtual PaymentTypeAndRule? OptimalPaymentMethodSelectedNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual PaymentProvider? PaymentProviderSelectedNavigation { get; set; }

    public virtual UserDetail? User { get; set; }
}
