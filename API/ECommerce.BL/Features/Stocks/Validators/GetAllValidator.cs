using ECommerce.BLL.Features.Stocks.Requests;
using ECommerce.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Stocks.Validators;

public class GetAllStockValidator : AbstractValidator<GetAllStockRequest>
{
    private readonly IStringLocalizer<GetAllStockValidator> _localizer;

    public GetAllStockValidator(IStringLocalizer<GetAllStockValidator> localizer)
    {
        _localizer = localizer;

        RuleFor(req => req).SetValidator(new GetAllValidator<GetAllStockValidator>(_localizer));
    }
}
