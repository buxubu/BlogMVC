using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class ProductSize
{
    public int IdProductSize { get; set; }

    public int IdProduct { get; set; }

    public int IdSize { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual Size IdSizeNavigation { get; set; } = null!;
}
