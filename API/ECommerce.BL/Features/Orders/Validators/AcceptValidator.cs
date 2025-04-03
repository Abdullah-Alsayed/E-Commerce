using System.Linq;
using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Orders.Validators;

public class AcceptValidator : AbstractValidator<AcceptOrderRequest>
{
    private readonly IStringLocalizer<AcceptValidator> _localizer;

    public AcceptValidator(
        ApplicationDbContext context,
        IStringLocalizer<AcceptValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Order]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Order]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Orders.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Order]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
