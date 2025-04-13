using System.Linq;
using ECommerce.BLL.Features.Tags.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Tags.Validators
{
    public class ToggleActiveTagValidator : AbstractValidator<ToggleActiveTagRequest>
    {
        private readonly IStringLocalizer<ToggleActiveTagValidator> _localizer;

        public ToggleActiveTagValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveTagValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Tags.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Tag]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
