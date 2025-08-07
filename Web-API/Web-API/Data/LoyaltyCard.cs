using System;
using System.Collections.Generic;

namespace Web_API.Data;

public partial class LoyaltyCard
{
    public string CardNumber { get; set; } = null!;

    public Guid CustomerId { get; set; }

    public int AvailablePoints { get; set; }

    public string Status { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
