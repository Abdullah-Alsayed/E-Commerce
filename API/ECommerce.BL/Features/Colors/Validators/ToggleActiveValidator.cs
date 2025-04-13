using System.Linq;
using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Colors.Validators
{
    public class ToggleActiveColorValidator : AbstractValidator<ToggleActiveColorRequest>
    {
        private readonly IStringLocalizer<ToggleActiveColorValidator> _localizer;

        public ToggleActiveColorValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveColorValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Colors.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Color]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
