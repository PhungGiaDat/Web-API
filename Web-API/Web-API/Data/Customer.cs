using System;
using System.Collections.Generic;

namespace Web_API.Data;

public partial class Customer
{
    public Guid CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string CitizenId { get; set; } = null!;

    public DateOnly CitizenIdExpiry { get; set; }

    public virtual ICollection<LoyaltyCard> LoyaltyCards { get; set; } = new List<LoyaltyCard>();
}
