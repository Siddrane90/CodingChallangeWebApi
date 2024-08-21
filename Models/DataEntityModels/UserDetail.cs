using System;
using System.Collections.Generic;

namespace CodingChallangeWebApi.Models.DataEntityModels;

public partial class UserDetail
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? FullName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
