using System;
using System.Collections.Generic;

namespace Web_API.Data;

public partial class Shop
{
    public string ShopId { get; set; } = null!;

    public string ShopName { get; set; } = null!;

    public string? Address { get; set; }

    public string ApiKeyHash { get; set; } = null!;
}
