using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class Transaction
{
    public int IdTran { get; set; }

    public int AccountId { get; set; }

    public int IdOrder { get; set; }

    public string? NameAccount { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
