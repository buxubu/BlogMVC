namespace BlogMVC.Areas.Admin.Models
{
    public static class CustomerRoles
    {
        public const string Administrator = "Admin";
        public const string Editer = "Edit";
        public const string AdministratorOrUser = Administrator + "," + Editer;
    }
}
