namespace BlogMVC.ModelViews
{
    public class OrderViewModel
    {
        public int IdOrder { get; set; }

        public int IdPay { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? ShipDate { get; set; }

        public bool? Deleted { get; set; }

        public bool? Paid { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? Note { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhone { get; set; }

        public string? CustomerEmail { get; set; }

        public string? CustomerAddress { get; set; }
    }
}
