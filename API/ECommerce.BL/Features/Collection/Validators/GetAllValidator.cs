using ECommerce.BLL.Features.Collections.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Collections.Validators;

public class GetAllCollectionValidator : AbstractValidator<GetAllCollectionRequest>
{
    private readonly IStringLocalizer<GetAllCollectionValidator> _localizer;

    public GetAllCollectionValidator(IStringLocalizer<GetAllCollectionValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req)
            .SetValidator(new GetAllValidator<GetAllCollectionValidator>(_localizer));
    }
}
