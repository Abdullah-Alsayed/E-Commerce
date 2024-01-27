using ECommerce.BLL.Futures.Areas.Requests;
using ECommerce.DAL;
using ECommerce.Helpers;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace ECommerce.BLL.Futures.Areas.Validators;

public class FindAreaValidator : AbstractValidator<FindAreaRequest>
{
    private readonly IStringLocalizer<FindAreaValidator> _localizer;

    public FindAreaValidator(
        Applicationdbcontext context,
        IStringLocalizer<FindAreaValidator> localizer
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
                return context.Areas.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(
                x =>
                    $" {_localizer[Constants.EntitsKeys.Areas]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
