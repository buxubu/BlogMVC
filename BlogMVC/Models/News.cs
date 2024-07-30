using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class News
{
    public int PostId { get; set; }

    public int CaId { get; set; }

    public int AccountId { get; set; }

    public string? Title { get; set; }

    public string? ShortContent { get; set; }

    public string? Contents { get; set; }

    public string? Thumb { get; set; }

    public bool? Published { get; set; }

    public string? Alias { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Author { get; set; }

    public string? Tags { get; set; }

    public bool? IsHot { get; set; }

    public bool? IsNewFeed { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Category Ca { get; set; } = null!;
}
