using System.Linq;
using ECommerce.BLL.Features.Categories.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Categories.Validators
{
    public class ToggleActiveCategoryValidator : AbstractValidator<ToggleActiveCategoryRequest>
    {
        private readonly IStringLocalizer<ToggleActiveCategoryValidator> _localizer;

        public ToggleActiveCategoryValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveCategoryValidator> localizer
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
                    $" {_localizer[Constants.EntityKeys.Category]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
