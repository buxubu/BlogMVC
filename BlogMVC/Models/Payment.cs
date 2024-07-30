using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class Payment
{
    public int IdPay { get; set; }

    public string? NamePayment { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
