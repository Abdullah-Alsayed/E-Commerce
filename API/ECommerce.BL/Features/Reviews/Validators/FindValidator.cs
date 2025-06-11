using System.Linq;
using ECommerce.BLL.Features.Reviews.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Reviews.Validators;

public class FindReviewValidator : AbstractValidator<FindReviewRequest>
{
    private readonly IStringLocalizer<FindReviewValidator> _localizer;

    public FindReviewValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindReviewValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;
        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Review]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Review]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Reviews.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Review]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
