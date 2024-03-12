using AutoMapper;
using ECommerce.BLL.Features.Account.Requests;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Expense, ExpenseDto>().ReverseMap();

            CreateMap<Notification, NotificationDto>().ReverseMap();

            CreateMap<User, CreateUserRequest>()
                //.ForMember(o => o.Email.ToLower(), b => b.MapFrom(z => z.Email))
                //.ForMember(o => o.UserName.ToLower(), b => b.MapFrom(z => z.UserName))
                .ReverseMap();
        }
    }
}
