using ECommerce.BLL.Futures.Governorates.Requests;
using ECommerce.DAL;
using ECommerce.Helpers;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace ECommerce.BLL.Futures.Governorates.Validators
{
    public class ToggleAvtiveGovernorateValidator
        : AbstractValidator<ToggleAvtiveGovernorateRequest>
    {
        private readonly IStringLocalizer<ToggleAvtiveGovernorateValidator> _localizer;

        public ToggleAvtiveGovernorateValidator(
            Applicationdbcontext context,
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
                .WithMessage(
                    x =>
                        $" {_localizer[Constants.EntitsKeys.Governorates]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
