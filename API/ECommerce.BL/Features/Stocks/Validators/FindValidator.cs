using System.Linq;
using ECommerce.BLL.Features.Stocks.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Stocks.Validators;

public class FindStockValidator : AbstractValidator<FindStockRequest>
{
    private readonly IStringLocalizer<FindStockValidator> _localizer;

    public FindStockValidator(
        ApplicationDbContext context,
        IStringLocalizer<FindStockValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Stock]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Stock]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Stocks.Any(x => x.Id == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Stock]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
