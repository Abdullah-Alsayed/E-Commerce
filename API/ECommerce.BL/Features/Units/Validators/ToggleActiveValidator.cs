using System.Linq;
using ECommerce.BLL.Features.Tags.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Tags.Validators
{
    public class ToggleActiveUnitValidator : AbstractValidator<ToggleActiveUnitRequest>
    {
        private readonly IStringLocalizer<ToggleActiveUnitValidator> _localizer;

        public ToggleActiveUnitValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveUnitValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Units.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Unit]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
