using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class Stock
{
    public int IdStock { get; set; }

    public int? Stock1 { get; set; }

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
}
