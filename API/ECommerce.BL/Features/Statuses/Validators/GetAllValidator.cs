using ECommerce.BLL.Features.Statuses.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Statuses.Validators;

public class GetAllStatusValidator : AbstractValidator<GetAllStatusRequest>
{
    private readonly IStringLocalizer<GetAllStatusValidator> _localizer;

    public GetAllStatusValidator(IStringLocalizer<GetAllStatusValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllStatusValidator>(_localizer));
    }
}
