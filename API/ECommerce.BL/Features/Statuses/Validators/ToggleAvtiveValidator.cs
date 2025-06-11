using System.Linq;
using ECommerce.BLL.Features.Statuses.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Statuss.Validators
{
    public class ToggleActiveStatusValidator : AbstractValidator<ToggleActiveStatusRequest>
    {
        private readonly IStringLocalizer<ToggleActiveStatusValidator> _localizer;

        public ToggleActiveStatusValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveStatusValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Statuses.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Status]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
