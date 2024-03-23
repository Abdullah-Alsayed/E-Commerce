// Ignore Spelling: Validator

using System.Linq;
using ECommerce.BLL.Features.Stocks.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Stocks.Validators;

public class ReturnStockValidator : AbstractValidator<ReturnStockRequest>
{
    private readonly IStringLocalizer<ReturnStockValidator> _localizer;

    public ReturnStockValidator(
        ApplicationDbContext context,
        IStringLocalizer<ReturnStockValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(x => x.ID)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Stock]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Stock]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(ID =>
            {
                return context.Statuses.Any(x => x.ID == ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Stock]} {_localizer[Constants.MessageKeys.NotExist]}"
            );

        RuleFor(req => req.ProductID)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Product]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Product]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(ID =>
            {
                return context.Products.Any(x => x.ID == ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Product]} {_localizer[Constants.MessageKeys.NotExist]}"
            );

        RuleFor(req => req.Size)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Vendor]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Vendor]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleForEach(x => x.Size)
            .ChildRules(Size =>
            {
                Size.RuleFor(req => req.ID)
                    .NotEmpty()
                    .WithMessage(x =>
                        $"{_localizer[Constants.EntitsKeys.Size]} {_localizer[Constants.MessageKeys.IsRequired]}"
                    )
                    .NotNull()
                    .WithMessage(x =>
                        $"{_localizer[Constants.EntitsKeys.Size]} {_localizer[Constants.MessageKeys.IsRequired]}"
                    )
                    .Must(ID =>
                    {
                        return context.Sizes.Any(x => x.ID == ID && x.IsActive && !x.IsDeleted);
                    })
                    .WithMessage(x =>
                        $" {_localizer[Constants.EntitsKeys.Size]} {_localizer[Constants.MessageKeys.NotExist]}"
                    );

                Size.RuleFor(req => req.Quantity)
                    .GreaterThanOrEqualTo(1)
                    .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 1].ToString())
                    .LessThanOrEqualTo(int.MaxValue)
                    .WithMessage(x =>
                        _localizer[Constants.MessageKeys.MinNumber, int.MaxValue].ToString()
                    )
                    .NotEmpty()
                    .WithMessage(x =>
                        $"{_localizer[Constants.EntitsKeys.Quantity]} {_localizer[Constants.MessageKeys.IsRequired]}"
                    )
                    .NotNull()
                    .WithMessage(x =>
                        $"{_localizer[Constants.EntitsKeys.Quantity]} {_localizer[Constants.MessageKeys.IsRequired]}"
                    );
            });

        RuleFor(req => req.Color)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Color]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Color]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleForEach(req => req.Color)
            .ChildRules(color =>
            {
                color
                    .RuleFor(x => x.ID)
                    .NotEmpty()
                    .WithMessage(x =>
                        $"{_localizer[Constants.EntitsKeys.Color]} {_localizer[Constants.MessageKeys.IsRequired]}"
                    )
                    .NotNull()
                    .WithMessage(x =>
                        $"{_localizer[Constants.EntitsKeys.Color]} {_localizer[Constants.MessageKeys.IsRequired]}"
                    )
                    .Must(ID =>
                    {
                        return context.Colors.Any(x => x.ID == ID && x.IsActive && !x.IsDeleted);
                    })
                    .WithMessage(x =>
                        $" {_localizer[Constants.EntitsKeys.Color]} {_localizer[Constants.MessageKeys.NotExist]}"
                    );

                color
                    .RuleFor(req => req.Quantity)
                    .GreaterThanOrEqualTo(1)
                    .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 1].ToString())
                    .LessThanOrEqualTo(int.MaxValue)
                    .WithMessage(x =>
                        _localizer[Constants.MessageKeys.MinNumber, int.MaxValue].ToString()
                    )
                    .NotEmpty()
                    .WithMessage(x =>
                        $"{_localizer[Constants.EntitsKeys.Quantity]} {_localizer[Constants.MessageKeys.IsRequired]}"
                    )
                    .NotNull()
                    .WithMessage(x =>
                        $"{_localizer[Constants.EntitsKeys.Quantity]} {_localizer[Constants.MessageKeys.IsRequired]}"
                    );
            });
    }
}
