using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Roles.Validators;

public class GetAllRoleValidator : AbstractValidator<GetAllRoleRequest>
{
    private readonly IStringLocalizer<GetAllRoleValidator> _localizer;

    public GetAllRoleValidator(IStringLocalizer<GetAllRoleValidator> localizer)
    {
        _localizer = localizer;

        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllRoleValidator>(_localizer));
    }
}
