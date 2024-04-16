using ECommerce.BLL.Features.Bookings.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Bookings.Validators;

public class GetAllBookingValidator : AbstractValidator<GetAllBookingRequest>
{
    private readonly IStringLocalizer<GetAllBookingValidator> _localizer;

    public GetAllBookingValidator(IStringLocalizer<GetAllBookingValidator> localizer)
    {
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllBookingValidator>(_localizer));
    }
}
