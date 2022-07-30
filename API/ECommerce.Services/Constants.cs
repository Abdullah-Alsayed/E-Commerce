namespace ECommerce.Services
{
    public static class Constants
    {
        public static class languages
        {
            public const string Arabic = "Arabic";
            public const string English = "English";
        }
        public static class Gender
        {
            public const string Male = "Male";
            public const string Female = "Female";
        }
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }
        public static class OrderBY
        {
            public const string Ascending  = "Ascending";
            public const string Descending = "Descending";

        }
        public static class ImgFolder
        {
            public const string Brands = "Brands";
            public const string Categorys = "Categorys";
            public const string SubCategorys = "SubCategorys";
            public const string ProdactImg = "ProdactImg";
        }
        public static class Errors
        {
            public const string Emailexists = "Email Already Exists";
            public const string UserNameExists = "UserName Already Exists";
            public const string PhoneNumbeExists = "PhoneNumber Already Exists";
            public const string PhoneNumberFiled = "The PhoneNumber field is not a valid";
            public const string LoginFiled = "The Useraame Or Password Incorrect";
            public const string CreateFailed = "Create Failed";
            public const string NotFound = "Not Found";
        }
        public static class NotificationIcons
        {
            public const string Add = "ADD.png";
            public const string Edit = "Edit.png";
            public const string Delete = "Delete.png";
        }
    }
}
