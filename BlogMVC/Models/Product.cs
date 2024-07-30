using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public int IdCatePro { get; set; }

    public string? NameProduct { get; set; }

    public int? Price { get; set; }

    public int? Discount { get; set; }

    public string? Descrip { get; set; }

    public string? Video { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? BestSaller { get; set; }

    public bool? HomeFlag { get; set; }

    public bool? Active { get; set; }

    public string? Tag { get; set; }

    public string? Alias { get; set; }

    public string? MetaDesc { get; set; }

    public string? MetaKey { get; set; }

    public virtual CateProduct IdCateProNavigation { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public virtual ICollection<ThumbProduct> ThumbProducts { get; set; } = new List<ThumbProduct>();
}
