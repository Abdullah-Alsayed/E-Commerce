using System.Linq;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vendors.Validators;

public class UpdateVendorValidator : AbstractValidator<UpdateVendorRequest>
{
    private readonly IStringLocalizer<UpdateVendorValidator> _localizer;

    public UpdateVendorValidator(
        ApplicationDbContext context,
        IStringLocalizer<UpdateVendorValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Vendors.Any(x => x.Id == req.ID && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Vendor]} {_localizer[Constants.MessageKeys.NotFound]}"
            );

        RuleFor(req => req.Name)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Vendors.Any(x =>
                        x.Name.ToLower() == req.Name.ToLower() && x.Id != req.ID && !x.IsDeleted
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.Phone)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 20].ToString())
            .Matches(Constants.Regex.PhoneNumber)
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.PhoneNotValid]}")
            .Must(
                (req, name) =>
                {
                    return !context.Vendors.Any(x =>
                        x.Phone.ToLower() == req.Phone.ToLower() && x.Id != req.ID && !x.IsDeleted
                    );
                }
            )
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.PhoneNumber]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.Address)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Address))
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .When(x => !string.IsNullOrEmpty(x.Address))
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString());

        RuleFor(req => req.Email)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Email]} {_localizer[Constants.MessageKeys.NotValid]}"
            );
    }
}
