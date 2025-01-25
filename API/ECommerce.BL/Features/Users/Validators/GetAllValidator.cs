using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators;

public class GetAllUserValidator : AbstractValidator<GetAllUserRequest>
{
    private readonly IStringLocalizer<GetAllUserValidator> _localizer;

    public GetAllUserValidator(IStringLocalizer<GetAllUserValidator> localizer)
    {
        _localizer = localizer;

        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllUserValidator>(_localizer));
    }
}
