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
            CreateMap<Expense, ExpenseDto>().ReverseMap();

            CreateMap<Notification, NotificationDto>().ReverseMap();
        }
    }
}
