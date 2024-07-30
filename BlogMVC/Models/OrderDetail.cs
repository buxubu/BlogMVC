using System;
using System.Collections.Generic;

namespace BlogMVC.Models;

public partial class OrderDetail
{
    public int IdOrderDetail { get; set; }

    public int IdOrder { get; set; }

    public int IdProduct { get; set; }

    public int? OrderNumber { get; set; }

    public int? Quantity { get; set; }

    public int? Discount { get; set; }

    public DateTime? ShipDate { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
