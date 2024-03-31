using System.IO;

namespace ECommerce.Core
{
    public static class Constants
    {
        public const string System = "System";
        public const string Permission= "Permission";
        public static string Issuer= "E-commerceApi";

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
            public const string NotExist = "notExist";
            public const string InvalidExtension= "invalidExtension";
            public const string InvalidSize= "invalidSize";
            public const string HasReview= "hasReview";
            public const string EmailNotValid= "emailNotValid";
            public const string LinkNotValid= "linkNotValid";
            public const string ColorNotValid = "colorNotValid";
            public const string PhoneNotValid= "phoneNotValid";
            public const string SubjectNotValid="";
            public const string MaxLimete="";
            public const string StatusIsComplete="";
            public const string StatusIsCompleteCantDelete="";
            public const string CantDelete="";
            public const string IsComplete = "";
            public const string LastOne="";
            public const string quantityNotValid="";
            public const string IsDefault="";
        }
        public static class EntityKeys
        {
            public const string ID = "ID"; // From Token
            public const string FullName = "FullName"; //From Token
            public const string Tax= "tax";
            public const string NameAR= "nameAr";
            public const string NameEn= "nameEN";
            public const string Governorate = "governorate";
            public const string Area = "area";
            public const string Color="color";
            public const string Voucher= "voucher";
            public const string Value = "value";
            public const string Name="name";
            public const string Photo="photo";
            public const string Brand = "barnd";
            public const string Category= "category";
            public const string SubCategory= "subCategory";
            public const string Unit= "unit";
            public const string Status= "status";
            public const string Product = "Product";
            public const string Review= "review";
            public const string Email="";
            public const string Whatsapp="";
            public const string MainColor="";
            public const string Phone="";
            public const string Subject="";
            public const string Message="";
            public const string Expense="";
            public const string Amount="";
            public const string Reference="";
            public const string Slider= "";
            public const string TitleAR= "";
            public const string TitleEN= "";
            public const string Description= "";
            public const string Vendor="";
            public const string Address = "";
            public const string Feedback="";
            public const string Comment="";
            public const string Rate="";
            public const string Rating="";
            public const string Quantity="";
            public const string Cart="";
            public const string Title="";
            public const string Price="";
            public const string Order="";
            public const string DeliveryDate="";
            public const string Size="";
            public const string Invoice="";
            public const string Stock="";
            public const string Role = "";
        }

        public static class Gender
        {
            public const string Male = "Male";
            public const string Female = "Female";
        }

        public static class Roles
        {
            public const string SuperAdmin = "Super Admin";
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
            public const string Images= "Images";
            public const string Main= "Main";
            public const string Expense= "Expense";
            public const string Slider= "Slider";
            public const string Products= "Products";
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
        public static class Regex
        {
            public const string Color = "^#?([a-f0-9]{6}|[a-f0-9]{3})$";
            public const string PhoneNumber = "^(\\+\\d{1,2}\\s?)?(1\\s?)?((\\(\\d{3}\\))|\\d{3})[\\s.-]?\\d{3}[\\s.-]?\\d{4}$";
        }
    }
}
