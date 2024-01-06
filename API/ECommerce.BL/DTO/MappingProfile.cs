using AutoMapper;
using ECommerce.BLL.DTO;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Unit, UnitDto>().ReverseMap();

            CreateMap<Color, ColorDto>().ReverseMap();

            CreateMap<Brand, BrandDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<SubCategory, SubCategoryDto>().ReverseMap();

            CreateMap<PromoCode, PromoCodeDto>().ReverseMap();

            CreateMap<Governorate, GovernorateDto>().ReverseMap();

            CreateMap<Area, AreaDto>().ReverseMap();

            CreateMap<Status, StatusDto>().ReverseMap();

            CreateMap<ContactUs, ContactUsDto>().ReverseMap();

            CreateMap<Expense, ExpenseDto>().ReverseMap();

            CreateMap<Review, ReviewDto>().ReverseMap();

            CreateMap<Notification, NotificationDto>().ReverseMap();

            CreateMap<Setting, SettingDto>().ReverseMap();
        }
    }
}
