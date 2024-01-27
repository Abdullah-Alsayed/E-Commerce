namespace ECommerce.Helpers
{
    public static class Constants
    {
        public const string System = "System";

        public static class Languages
        {
            public const string Arabic = "Arabic";
            public const string English = "English";
            public const string Ar = "ar-EG";
            public const string En = "en-US";
        }
        public static class MessageKeys
        {
            public const string Exist = "exist";
            public const string NotFound = "notFound";
            public const string MaxNumber = "maxNumber";
            public const string MinNumber = "minNumber";
            public const string IsRequired= "isRequired";
            public const string Success = "success";
            public const string Fail = "fail";
        }
        public static class EntitsKeys
        {
            public const string ID = "ID"; // From Token
            public const string FullName = "FullName"; //From Token

            public const string Tax= "tax";
            public const string NameAR= "nameAr";
            public const string NameEn= "nameEN";
            public const string Governorates = "governorates";
            public const string Areas = "areas";
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
            public const string Ascending = "Ascending";
            public const string Descending = "Descending";
        }

        public static class PhotoFolder
        {
            public const string Brands = "Brands";
            public const string Categorys = "Categorys";
            public const string SubCategorys = "SubCategorys";
            public const string ProductPhoto = "ProductPhoto";
        }

        public static class Errors
        {
            public const string Emailexists = "Email Already Exists";
            public const string UserNameExists = "UserName Already Exists";
            public const string PhoneNumbeExists = "PhoneNumber Already Exists";
            public const string PhoneNumberFiled = "The PhoneNumber field is not a valid";
            public const string LoginFiled = "The Useraame Or Password Incorrect";
            public const string CreateFailed = "Create Failed";
            public const string NotFound = "notFound";
            public const string Exist = "exist";
            public const string Register = "Register Failed";
        }

        public static class NotificationIcons
        {
            public const string Add = "ADD.png";
            public const string Edit = "Edit.png";
            public const string Delete = "Delete.png";
        }
    }
}
