using System.Linq;
using ECommerce.BLL.Features.Tags.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Tags.Validators
{
    public class DeleteUnitValidator : AbstractValidator<DeleteUnitRequest>
    {
        private readonly IStringLocalizer<CreateUnitValidator> _localizer;

        public DeleteUnitValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateUnitValidator> localizer
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
