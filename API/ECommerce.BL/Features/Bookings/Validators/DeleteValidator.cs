using System.Linq;
using ECommerce.BLL.Features.Bookings.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Bookings.Validators
{
    public class DeleteBookingValidator : AbstractValidator<DeleteBookingRequest>
    {
        private readonly IStringLocalizer<CreateBookingValidator> _localizer;

        public DeleteBookingValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateBookingValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Bookings.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Booking]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
