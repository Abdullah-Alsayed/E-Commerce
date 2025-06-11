using System.Linq;
using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Areas.Validators
{
    public class ToggleActiveAreaValidator : AbstractValidator<ToggleActiveAreaRequest>
    {
        private readonly IStringLocalizer<ToggleActiveAreaValidator> _localizer;

        public ToggleActiveAreaValidator(
            ApplicationDbContext context,
            IStringLocalizer<ToggleActiveAreaValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Areas.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Area]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
