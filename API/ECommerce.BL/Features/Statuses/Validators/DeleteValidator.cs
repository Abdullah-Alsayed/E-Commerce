using System.Linq;
using ECommerce.BLL.Features.Statuses.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Statuses.Validators
{
    public class DeleteStatusValidator : AbstractValidator<DeleteStatusRequest>
    {
        private readonly IStringLocalizer<CreateStatusValidator> _localizer;

        public DeleteStatusValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateStatusValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Statuses.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Status]} {_localizer[Constants.MessageKeys.NotFound]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Statuses.Where(x => x.IsActive && !x.IsDeleted).Count() > 1;
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Status]} {_localizer[Constants.MessageKeys.LastOne]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Statuses.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Status]} {_localizer[Constants.MessageKeys.IsComplete]} {_localizer[Constants.MessageKeys.CantDelete]}"
                );
        }
    }
}
