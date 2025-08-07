using System;
using System.Collections.Generic;

namespace Web_API.Data;

public partial class Reward
{
    public string RewardId { get; set; } = null!;

    public string RewardName { get; set; } = null!;

    public string? Description { get; set; }

    public int RequiredPoints { get; set; }

    public bool IsActive { get; set; }
}
