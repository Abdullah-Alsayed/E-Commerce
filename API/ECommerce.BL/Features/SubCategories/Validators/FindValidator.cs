using System.Linq;
using ECommerce.BLL.Features.SubCategories.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.SubCategorys.Validators;

public class FindSubCategoryValidator : AbstractValidator<FindSubCategoryRequest>
{
    private readonly IStringLocalizer<FindSubCategoryValidator> _localizer;

    public FindSubCategoryValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindSubCategoryValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.SubCategory]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.SubCategory]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.SubCategories.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.SubCategory]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
