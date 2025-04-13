using System.Linq;
using ECommerce.BLL.Features.Sizes.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sizes.Validators
{
    public class ToggleActiveSizeValidator : AbstractValidator<ToggleActiveSizeRequest>
    {
        private readonly IStringLocalizer<ToggleActiveSizeValidator> _localizer;

        public ToggleActiveSizeValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveSizeValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Sizes.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Size]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
