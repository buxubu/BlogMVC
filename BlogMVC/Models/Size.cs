using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class Size
{
    public int IdSize { get; set; }

    public string? Size1 { get; set; }

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}
