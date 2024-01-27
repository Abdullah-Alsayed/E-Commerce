using ECommerce.BLL.Futures.Governorates.Requests;
using ECommerce.DAL;
using ECommerce.Helpers;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace ECommerce.BLL.Futures.Governorates.Validators
{
    public class DeleteGovernorateValidator : AbstractValidator<DeleteGovernorateRequest>
    {
        private readonly IStringLocalizer<CreateGovernorateValidator> _localizer;

        public DeleteGovernorateValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateGovernorateValidator> localizer
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
