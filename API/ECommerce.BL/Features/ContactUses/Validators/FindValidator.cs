using System.Linq;
using ECommerce.BLL.Features.ContactUss.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.ContactUss.Validators;

public class FindContactUsValidator : AbstractValidator<FindContactUsRequest>
{
    private readonly IStringLocalizer<FindContactUsValidator> _localizer;

    public FindContactUsValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindContactUsValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.ContactUs]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.ContactUs]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.ContactUs.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.ContactUs]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
