using System;
using System.Collections.Generic;

namespace Web_API.Data;

public partial class Transaction
{
    public string TransactionId { get; set; } = null!;

    public string CardNumber { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public int PointsChanged { get; set; }

    public int BalanceAfter { get; set; }

    public DateTime TransactionTime { get; set; }

    public string? ReferenceId { get; set; }

    public string? Note { get; set; }

    public virtual LoyaltyCard CardNumberNavigation { get; set; } = null!;
}
