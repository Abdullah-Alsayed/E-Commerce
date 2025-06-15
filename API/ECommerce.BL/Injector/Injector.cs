using ECommerce.BLL.Features.Areas.Services;
using ECommerce.BLL.Features.Bookings.Services;
using ECommerce.BLL.Features.Brands.Services;
using ECommerce.BLL.Features.Carts.Services;
using ECommerce.BLL.Features.Categories.Services;
using ECommerce.BLL.Features.Colors.Services;
using ECommerce.BLL.Features.ContactUses.Services;
using ECommerce.BLL.Features.Errors.Services;
using ECommerce.BLL.Features.Expenses.Services;
using ECommerce.BLL.Features.Feedbacks.Services;
using ECommerce.BLL.Features.Governorates.Services;
using ECommerce.BLL.Features.Histories.Services;
using ECommerce.BLL.Features.Invoices.Services;
using ECommerce.BLL.Features.Orders.Services;
using ECommerce.BLL.Features.Products.Services;
using ECommerce.BLL.Features.Reviews.Services;
using ECommerce.BLL.Features.Roles.Services;
using ECommerce.BLL.Features.Settings.Services;
using ECommerce.BLL.Features.Sizes.Services;
using ECommerce.BLL.Features.Sliders.Services;
using ECommerce.BLL.Features.Statuses.Services;
using ECommerce.BLL.Features.Stocks.Services;
using ECommerce.BLL.Features.SubCategories.Services;
using ECommerce.BLL.Features.Tags.Services;
using ECommerce.BLL.Features.Units.Services;
using ECommerce.BLL.Features.Users.Filter;
using ECommerce.BLL.Features.Users.Services;
using ECommerce.BLL.Features.Vendors.Services;
using ECommerce.BLL.Features.Vouchers.Services;
using ECommerce.BLL.UnitOfWork;
using ECommerce.Core.Services.MailServices;
using ECommerce.Core.Services.User;
using ECommerce.Core.Services.WhatsappServices;
using ECommerce.Core.Services.WhatsappServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.BLL.Injector
{
    public static class Injector
    {
        public static void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            //****************** Email Settings ******************************
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            //****************** whatsapp Settings ******************************
            services.Configure<WhatsappSettings>(configuration.GetSection("WhatsappSettings"));
            services.AddScoped<IWhatsappServices, WhatsappServices>();

            //****************** Services ******************************
            services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();

            //****************** UserContext ******************************
            services.AddScoped<IUserContext, UserContext>();

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            //****************** Features ******************************
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<ISizeService, SizeService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IErrorService, ErrorService>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IMailServices, MailServices>();
            services.AddScoped<IMailServices, MailServices>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<IGovernorateService, GovernorateService>();
        }
    }
}
