
using System.IO;

namespace ECommerce.Core
{
    public static class Constants
    {
        public const string System = "System";
        public const string Permission= "Permission";
        public const string Issuer= "E-commerceApi";
        public const string HostName= "localhost:5001";
        public const int PageSize = 25;
        public const int PageIndex = 0;
        public const int FileSize = 3;
        public const string Descending = "desc";
        public const string Ascending = "Asc";
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
            public const string SubjectNotValid= "SubjectNotValid";
            public const string MaxLimited= "MaxLimited";
            public const string StatusIsComplete= "StatusIsComplete";
            public const string StatusIsCompleteCantDelete= "StatusIsCompleteCantDelete";
            public const string CantDelete= "CantDelete";
            public const string IsComplete = "IsComplete";
            public const string LastOne= "LastOne";
            public const string quantityNotValid= "quantityNotValid";
            public const string IsDefault= "IsDefault";
            public const string NotChanged = "NotChanged";
            public const string PasswordNotStrong= "PasswordNotStrong";
            public const string NotValid= "NotValid";
            public const string PasswordNotMatch= "PasswordNotMatch";
            public const string UserNotFound= "UserNotFound"; 
            public const string UserInThisRole = "UserInThisRole";
            public const string PasswordIsWrong= "PasswordIsWrong";
            public const string ForgotPassword= "Your password reset token is";
            public const string RestPassword= "Rest Password";
            public const string EmailIsConfirm= "EmailIsConfirm";
            public const string LoginFiled = "LoginFiled";
            public const string HasUser = "HasUser";
        }

        public static class Message
        {
            public const string ConfirmEmail = "ConfirmEmail";
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
            public const string Brand = "brand";
            public const string Category= "category";
            public const string SubCategory= "subCategory";
            public const string Unit= "unit";
            public const string Status= "status";
            public const string Product = "Product";
            public const string Review= "review";
            public const string Email= "Email";
            public const string Whatsapp= "MainColor";
            public const string MainColor= "MainColor";
            public const string PhoneNumber = "PhoneNumber";
            public const string Subject= "Subject";
            public const string Message= "Message";
            public const string Expense= "Expense";
            public const string Amount= "Amount";
            public const string Reference= "Reference";
            public const string Slider= "Slider";
            public const string TitleAR= "TitleAR";
            public const string TitleEN= "TitleEN";
            public const string Description= "Description";
            public const string Vendor= "Vendor";
            public const string Address = "Address";
            public const string Feedback= "Feedback";
            public const string Comment= "Comment";
            public const string Rate= "Rate";
            public const string Rating= "Rating";
            public const string Quantity= "Quantity";
            public const string Cart= "Cart";
            public const string Title= "Title";
            public const string Price= "Price";
            public const string Order= "Order";
            public const string DeliveryDate= "DeliveryDate";
            public const string Size= "Size";
            public const string Invoice= "Invoice";
            public const string Stock= "Stock";
            public const string Role = "Role";
            public const string Claim = "Claim";
            public const string User = "User";
            public const string OldPassword= "OldPassword";
            public const string NewPassword= "NewPassword";
            public const string ConfirmPassword= "ConfirmPassword";
            public const string Token="Token";
            public const string UserName= "UserName";
            public const string Booking= "Booking";
			public const string Products= "Products";
            public const string LastName = "LastName";
            public const string FirstName = "FirstName";
            public const string Password = "Password";
            public const string CreatedAt = "CreatedAt";
            public const string IsActive = "IsActive";
            public const string LastLogin = "LastLogin";
            public const string Age = "Age";
            public const string Gander = "Gander";
            public const string Tag = "Tag";
        }

        public static class LabelKeys
        {
            public const string Select = "Select";
            public const string ClickToUploadImage = "ClickToUploadImage";
            public const string Close = "Close";
            public const string SaveChanges = "SaveChanges";
            public const string Action = "Action";
            public const string Permission = "Permission";
            public const string Statistics = "Statistics";
            public const string UserManagement = "UserManagement";
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
            public const string Client = "Client";
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
            public const string User= "User";
        }

        public static class DefaultPhotos
        {
            public const string User = "User.png";
            public const string Default = "/Images/Default.png";

        }

        public static class Errors
        {
            public const string Emailexists = "Email Already Exists";
            public const string UserNameExists = "UserName Already Exists";
            public const string PhoneNumbeExists = "PhoneNumber Already Exists";
            public const string PhoneNumberFiled = "The PhoneNumber field is not a valid";
            public const string CreateFailed = "Create Failed";
            public const string NotFound = "notFound";
            public const string Exist = "exist";
            public const string Register = "Register Failed";
            public const string PhoneNumbeIsnotValid = "PhoneNumbeIsnotValid";
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

        public static class Claims
        {
            public const string RoleID = "RoleID";

            public const string RoleName = "RoleName";

            public const string UserID = "UserID";

            public const string Name = "Name";

            public const string UserPhoto = "UserPhoto";

            public const string UserRole = "UserRole";

            public const string Language = "Language";

            public const string ID = "ID";

            public const string UserName = "UserName";

            public const string FirstName = "FirstName";

            public const string LastName = "LastName";

            public const string FullName = "FullName";
        }
    }
}
