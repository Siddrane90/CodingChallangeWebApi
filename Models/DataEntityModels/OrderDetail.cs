using System;
using System.Collections.Generic;

namespace CodingChallangeWebApi.Models.DataEntityModels;

public partial class OrderDetail
{
    public int? Id { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? OrderQuantity { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ProductDetail? Product { get; set; }
}
