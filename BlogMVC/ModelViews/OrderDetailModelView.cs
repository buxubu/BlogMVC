namespace BlogMVC.ModelViews
{
    public class OrderDetailModelView
    {
        public int IdOrderDetail { get; set; }

        public int IdOrder { get; set; }

        public int IdProduct { get; set; }

        public int? OrderNumber { get; set; }

        public int? Quantity { get; set; }

        public int? Discount { get; set; }

        public DateTime? ShipDate { get; set; }
    }
}
