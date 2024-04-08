using System.Linq;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators
{
    public class ConfirmEmailUserValidator : AbstractValidator<ConfirmEmailUserRequest>
    {
        private readonly IStringLocalizer _localizer;

        public ConfirmEmailUserValidator(
            ApplicationDbContext context,
            IStringLocalizer<ConfirmEmailUserValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.ID)
                .NotNull()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.ID]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.ID]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Token)
                .NotNull()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.Token]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(req =>
                    $"{_localizer[Constants.EntityKeys.Token]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Users.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.ID]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
