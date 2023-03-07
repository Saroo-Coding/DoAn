using System;
using System.Collections.Generic;

namespace DoAn;

public partial class RefreshToken
{
    public int TokenId { get; set; }

    public string UserId { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public virtual User User { get; set; } = null!;
}
