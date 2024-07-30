using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class Color
{
    public int IdColor { get; set; }

    public string? Color1 { get; set; }

    public virtual ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
}
