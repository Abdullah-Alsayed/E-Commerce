using System.Linq;
using ECommerce.BLL.Features.SubCategories.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.SubCategorys.Validators
{
    public class ToggleAvtiveSubCategoryValidator
        : AbstractValidator<ToggleAvtiveSubCategoryRequest>
    {
        private readonly IStringLocalizer<ToggleAvtiveSubCategoryValidator> _localizer;

        public ToggleAvtiveSubCategoryValidator(
            Applicationdbcontext context,
            IStringLocalizer<ToggleAvtiveSubCategoryValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Categories.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.SubCategory]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
