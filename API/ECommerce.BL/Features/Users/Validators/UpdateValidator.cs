using System;
using System.Linq;
using System.Text.RegularExpressions;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    private readonly IStringLocalizer<UpdateUserValidator> _localizer;

    public UpdateUserValidator(
        ApplicationDbContext context,
        IStringLocalizer<UpdateUserValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Users.Any(x => x.Id == req.ID && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
        RuleFor(req => req.FirstName)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(2)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 2].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.LastName)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.LastName))
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(2)
            .When(x => !string.IsNullOrEmpty(x.LastName))
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 2].ToString());

        RuleFor(req => req.Email)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Email]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Email]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .EmailAddress()
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.EmailNotValid]}");

        RuleFor(req => req.PhoneNumber)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.PhoneNumber]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.PhoneNumber]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(req =>
            {
                var result = Regex.IsMatch(
                    req,
                    @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$"
                );
                return result;
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.PhoneNumber]} {_localizer[Constants.MessageKeys.NotValid]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                var result = !context.Users.Any(x =>
                    x.Id != req.ID
                    && (
                        x.Email.ToLower() == req.Email.ToLower() || x.PhoneNumber == req.PhoneNumber
                    )
                    && x.IsActive
                    && !x.IsDeleted
                );
                return result;
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.Exist]}"
            );
    }
}
