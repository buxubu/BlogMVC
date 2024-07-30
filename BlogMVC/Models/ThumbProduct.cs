using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class ThumbProduct
{
    public int IdThumbPro { get; set; }

    public string? NameThumbPro { get; set; }

    public int IdProduct { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;
}
