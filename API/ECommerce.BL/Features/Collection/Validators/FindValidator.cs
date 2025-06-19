using System.Linq;
using ECommerce.BLL.Features.Collections.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Collections.Validators;

public class FindCollectionValidator : AbstractValidator<FindCollectionRequest>
{
    private readonly IStringLocalizer<FindCollectionValidator> _localizer;

    public FindCollectionValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindCollectionValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Collection]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Collection]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Collections.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Collection]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
