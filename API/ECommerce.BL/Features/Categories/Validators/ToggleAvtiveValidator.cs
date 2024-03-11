using System.Linq;
using ECommerce.BLL.Features.Categories.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Categories.Validators
{
    public class ToggleAvtiveCategoryValidator : AbstractValidator<ToggleAvtiveCategoryRequest>
    {
        private readonly IStringLocalizer<ToggleAvtiveCategoryValidator> _localizer;

        public ToggleAvtiveCategoryValidator(
            Applicationdbcontext context,
            IStringLocalizer<ToggleAvtiveCategoryValidator> localizer
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
                    $" {_localizer[Constants.EntitsKeys.Category]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
