using System;
using System.Collections.Generic;

namespace CodingChallangeWebApi.Models.DataEntityModels;

public partial class PaymentTypeAndRule
{
    public int Id { get; set; }

    public int? ProviderId { get; set; }

    public string? Type { get; set; }

    public decimal? MinimumAmount { get; set; }

    public decimal? MaximumAmount { get; set; }

    public decimal? ComissionValue { get; set; }

    public string? ComissionType { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual PaymentProvider? Provider { get; set; }
}
