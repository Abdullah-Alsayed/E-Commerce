using System.Linq;
using ECommerce.BLL.Features.Governorates.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Governorates.Validators
{
    public class ToggleAvtiveGovernorateValidator
        : AbstractValidator<ToggleAvtiveGovernorateRequest>
    {
        private readonly IStringLocalizer<ToggleAvtiveGovernorateValidator> _localizer;

        public ToggleAvtiveGovernorateValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleAvtiveGovernorateValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Governorates.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Governorate]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
