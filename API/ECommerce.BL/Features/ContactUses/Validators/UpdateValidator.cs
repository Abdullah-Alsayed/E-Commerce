using ECommerce.BLL.Features.ContactUses.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.ContactUses.Validators;

public class CreateContactUsValidator : AbstractValidator<CreateContactUsRequest>
{
    private readonly IStringLocalizer<CreateContactUsValidator> _localizer;

    public CreateContactUsValidator(
        ApplicationDbContext context,
        IStringLocalizer<CreateContactUsValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.Subject)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Subject]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Subject]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString());

        RuleFor(req => req.Name)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString());

        RuleFor(req => req.Message)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Message]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Message]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString());

        RuleFor(req => req.Phone)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Phone]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Phone]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .MaximumLength(20)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 20].ToString())
            .Matches(Constants.Regex.PhoneNumber)
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.PhoneNotValid]}");
    }
}
