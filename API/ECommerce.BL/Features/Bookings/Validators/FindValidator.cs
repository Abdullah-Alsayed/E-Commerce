using System.Linq;
using ECommerce.BLL.Features.Bookings.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Bookings.Validators;

public class FindBookingValidator : AbstractValidator<FindBookingRequest>
{
    private readonly IStringLocalizer<FindBookingValidator> _localizer;

    public FindBookingValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindBookingValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Booking]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Booking]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Bookings.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Booking]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
