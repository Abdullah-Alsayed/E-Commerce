using System.Linq;
using ECommerce.BLL.Features.Categories.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Categories.Validators;

public class FindCategoryValidator : AbstractValidator<FindCategoryRequest>
{
    private readonly IStringLocalizer<FindCategoryValidator> _localizer;

    public FindCategoryValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindCategoryValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;
        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Category]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Category]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Categories.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Category]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
