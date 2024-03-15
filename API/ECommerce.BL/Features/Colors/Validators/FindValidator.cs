using System.Linq;
using ECommerce.BLL.Features.Carts.Requests;
using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Colors.Validators;

public class FindColorValidator : AbstractValidator<FindColorRequest>
{
    private readonly IStringLocalizer<FindColorValidator> _localizer;

    public FindColorValidator(
        Applicationdbcontext context,
        IStringLocalizer<FindColorValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x => Constants.Errors.Register)
            .NotNull()
            .WithMessage(x => Constants.Errors.Register);

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Colors.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Color]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
