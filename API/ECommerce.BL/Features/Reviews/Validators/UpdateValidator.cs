using System.Linq;
using ECommerce.BLL.Features.Reviews.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Reviews.Validators;

public class UpdateReviewValidator : AbstractValidator<UpdateReviewRequest>
{
    private readonly IStringLocalizer _localizer;

    public UpdateReviewValidator(
        ApplicationDbContext context,
        IStringLocalizer<CreateReviewValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        _localizer = localizer;

        RuleFor(req => req.Review)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Review]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Review]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.Rate)
            .GreaterThanOrEqualTo(1)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 1].ToString())
            .LessThanOrEqualTo(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Review]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Review]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(ID =>
            {
                return context.Reviews.Any(x => x.ID == ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Review]} {_localizer[Constants.MessageKeys.NotExist]}"
            );
    }
}
