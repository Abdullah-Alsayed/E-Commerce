using ECommerce.BLL.Features.Tags.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Tags.Validators;

public class GetAllTagValidator : AbstractValidator<GetAllTagRequest>
{
    private readonly IStringLocalizer<GetAllTagValidator> _localizer;

    public GetAllTagValidator(IStringLocalizer<GetAllTagValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllTagValidator>(_localizer));
    }
}
