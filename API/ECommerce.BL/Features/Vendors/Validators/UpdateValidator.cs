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
                return context.Vendors.Any(x => x.ID == req.ID && !x.IsDeleted);
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
                        x.Name.ToLower() == req.Name.ToLower() && x.ID != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.Address)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Address]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Address]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Vendors.Any(x =>
                        x.Address.ToLower() == req.Address.ToLower() && x.ID != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Address]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.Phone)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.PhoneNumber]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.PhoneNumber]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .MaximumLength(20)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 20].ToString())
            .Matches(Constants.Regex.PhoneNumber)
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.PhoneNotValid]}")
            .Must(
                (req, name) =>
                {
                    return !context.Vendors.Any(x => x.Phone == req.Phone && x.ID != req.ID);
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Value]} {_localizer[Constants.MessageKeys.Exist]}"
            );
    }
}
