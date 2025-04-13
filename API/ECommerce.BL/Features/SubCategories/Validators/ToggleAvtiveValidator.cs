using System.Linq;
using ECommerce.BLL.Features.SubCategories.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.SubCategorys.Validators
{
    public class ToggleActiveSubCategoryValidator
        : AbstractValidator<ToggleActiveSubCategoryRequest>
    {
        private readonly IStringLocalizer<ToggleActiveSubCategoryValidator> _localizer;

        public ToggleActiveSubCategoryValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveSubCategoryValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Categories.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.SubCategory]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
