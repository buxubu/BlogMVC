using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class ProductStock
{
    public int IdProductStock { get; set; }

    public int IdProduct { get; set; }

    public int IdStock { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual Stock IdStockNavigation { get; set; } = null!;
}
