using System.Linq;
using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Areas.Validators
{
    public class DeleteAreaValidator : AbstractValidator<DeleteAreaRequest>
    {
        private readonly IStringLocalizer<CreateAreaValidator> _localizer;

        public DeleteAreaValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateAreaValidator> localizer
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
