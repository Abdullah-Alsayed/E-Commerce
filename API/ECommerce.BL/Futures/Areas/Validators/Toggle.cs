using ECommerce.BLL.Futures.Areas.Requests;
using ECommerce.DAL;
using ECommerce.Helpers;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace ECommerce.BLL.Futures.Areas.Validators
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
                .WithMessage(
                    x =>
                        $" {_localizer[Constants.EntitsKeys.Areas]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
