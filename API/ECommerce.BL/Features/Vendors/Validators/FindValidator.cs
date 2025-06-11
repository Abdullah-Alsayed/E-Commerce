using System.Linq;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Vendors.Validators;

public class FindVendorValidator : AbstractValidator<FindVendorRequest>
{
    private readonly IStringLocalizer<FindVendorValidator> _localizer;

    public FindVendorValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindVendorValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Vendor]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Vendor]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Vendors.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Vendor]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
