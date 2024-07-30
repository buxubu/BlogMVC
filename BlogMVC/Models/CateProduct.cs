using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class CateProduct
{
    public int IdCatePro { get; set; }

    public string? CatName { get; set; }

    public int? ParentId { get; set; }

    public int? Level { get; set; }

    public bool? Published { get; set; }

    public string? Thumb { get; set; }

    public string? Title { get; set; }

    public string? Alias { get; set; }

    public string? MetaDesc { get; set; }

    public string? MetaKey { get; set; }

    public string? Cover { get; set; }

    public string? SchemaMakup { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
