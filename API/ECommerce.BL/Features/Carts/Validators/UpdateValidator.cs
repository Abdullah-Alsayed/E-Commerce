using System.Linq;
using ECommerce.BLL.Features.Carts.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Carts.Validators;

public class UpdateCartValidator : AbstractValidator<UpdateCartRequest>
{
    private readonly IStringLocalizer<UpdateCartValidator> _localizer;

    public UpdateCartValidator(
        ApplicationDbContext context,
        IStringLocalizer<UpdateCartValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req)
            .Must(req =>
            {
                return context.ShoppingCarts.Any(x => x.ID == req.ID && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Cart]} {_localizer[Constants.MessageKeys.NotFound]}"
            );

        RuleFor(req => req.Quantity)
            .GreaterThanOrEqualTo(1)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 1].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Quantity]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Quantity]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );
    }
}
