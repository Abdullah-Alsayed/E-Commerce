using System.Linq;
using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Colors.Validators
{
    public class DeleteColorValidator : AbstractValidator<DeleteColorRequest>
    {
        private readonly IStringLocalizer<CreateColorValidator> _localizer;

        public DeleteColorValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateColorValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Colors.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Color]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
