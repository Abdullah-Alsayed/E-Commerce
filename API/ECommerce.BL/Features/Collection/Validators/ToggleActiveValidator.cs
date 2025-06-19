using System.Linq;
using ECommerce.BLL.Features.Collections.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Collections.Validators
{
    public class ToggleActiveCollectionValidator : AbstractValidator<ToggleActiveCollectionRequest>
    {
        private readonly IStringLocalizer<ToggleActiveCollectionValidator> _localizer;

        public ToggleActiveCollectionValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveCollectionValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Collections.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Collection]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
