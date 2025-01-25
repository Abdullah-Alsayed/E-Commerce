using ECommerce.BLL.Features.Categories.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Categories.Validators;

public class GetAllCategoryValidator : AbstractValidator<GetAllCategoryRequest>
{
    private readonly IStringLocalizer<GetAllCategoryValidator> _localizer;

    public GetAllCategoryValidator(IStringLocalizer<GetAllCategoryValidator> localizer)
    {
        _localizer = localizer;
        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllCategoryValidator>(_localizer));
    }
}
