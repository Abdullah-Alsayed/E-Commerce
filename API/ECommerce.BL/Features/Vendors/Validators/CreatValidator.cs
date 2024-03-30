using System.Linq;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vendors.Validators
{
    public class CreateVendorValidator : AbstractValidator<CreateVendorRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateVendorValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateVendorValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.Name)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Name]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Phone)
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Phone]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Phone]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .MaximumLength(20)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 20].ToString())
                .Matches(Constants.Regex.PhoneNumber)
                .WithMessage(x => $"{_localizer[Constants.MessageKeys.PhoneNotValid]}");

            RuleFor(req => req.Address)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Address]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Address]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    return !context.Vendors.Any(x =>
                        x.Name == req.Name || x.Phone == req.Phone && x.IsActive && !x.IsDeleted
                    );
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Vendor]} {_localizer[Constants.MessageKeys.Exist]}"
                );
        }
    }
}
