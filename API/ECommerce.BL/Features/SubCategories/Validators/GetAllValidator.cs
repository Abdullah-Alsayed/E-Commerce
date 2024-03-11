using ECommerce.BLL.Features.SubCategories.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.SubCategorys.Validators;

public class GetAllSubCategoryValidator : AbstractValidator<GetAllSubCategoryRequest>
{
    private readonly IStringLocalizer<GetAllSubCategoryValidator> _localizer;

    public GetAllSubCategoryValidator(IStringLocalizer<GetAllSubCategoryValidator> localizer)
    {
        RuleFor(req => req)
            .SetValidator(new GetAllValidator<GetAllSubCategoryValidator>(_localizer));
    }
}
