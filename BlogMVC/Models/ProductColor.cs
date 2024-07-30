using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class ProductColor
{
    public int IdProductColor { get; set; }

    public int IdProduct { get; set; }

    public int IdColor { get; set; }

    public virtual Color IdColorNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
