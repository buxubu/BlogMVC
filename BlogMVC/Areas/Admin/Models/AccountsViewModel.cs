namespace BlogMVC.Areas.Admin.Models
{
    public class AccountsViewModel
    {
        public int AccountId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Password { get; set; }

        public string? Salt { get; set; }

        public bool Active { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? LastLogin { get; set; }

        public int RoleId { get; set; }
    }
}
