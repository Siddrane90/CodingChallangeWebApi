using System;
using System.Collections.Generic;

namespace CodingChallangeWebApi.Models.DataEntityModels;

public partial class ProductDetail
{
    public int Id { get; set; }

    public string? ProductName { get; set; }

    public string? ProductDescription { get; set; }

    public string? ProductStatus { get; set; }

    public decimal? PricePerUnit { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
