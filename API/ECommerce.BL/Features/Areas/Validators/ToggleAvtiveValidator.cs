using System.Linq;
using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Areas.Validators
{
    public class ToggleAvtiveAreaValidator : AbstractValidator<ToggleAvtiveAreaRequest>
    {
        private readonly IStringLocalizer<ToggleAvtiveAreaValidator> _localizer;

        public ToggleAvtiveAreaValidator(
            Applicationdbcontext context,
            IStringLocalizer<ToggleAvtiveAreaValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Areas.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Area]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
