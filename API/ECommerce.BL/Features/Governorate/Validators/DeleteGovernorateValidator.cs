using System.Linq;
using ECommerce.BLL.Features.Governorate.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Governorate.Validators
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
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Governorates]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
