using System;
using System.Collections.Generic;

namespace CodingChallangeWebApi.Models.DataEntityModels;

public partial class PaymentProvider
{
    public int Id { get; set; }

    public string? ProviderName { get; set; }

    public string? Description { get; set; }

    public string? Apiurl { get; set; }

    public string? ApiKeyEncrypted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PaymentTypeAndRule> PaymentTypeAndRules { get; set; } = new List<PaymentTypeAndRule>();
}
