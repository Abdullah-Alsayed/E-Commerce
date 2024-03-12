using ECommerce.BLL.Features.ContactUses.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.ContactUss.Validators;

public class GetAllContactUsValidator : AbstractValidator<GetAllContactUsRequest>
{
    private readonly IStringLocalizer<GetAllContactUsValidator> _localizer;

    public GetAllContactUsValidator(IStringLocalizer<GetAllContactUsValidator> localizer)
    {
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllContactUsValidator>(_localizer));
    }
}
