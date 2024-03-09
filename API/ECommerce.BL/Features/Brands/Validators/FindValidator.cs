using System.Linq;
using ECommerce.BLL.Features.Brands.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Brands.Validators;

public class FindBrandValidator : AbstractValidator<FindBrandRequest>
{
    private readonly IStringLocalizer<FindBrandValidator> _localizer;

    public FindBrandValidator(
        Applicationdbcontext context,
        IStringLocalizer<FindBrandValidator> localizer
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
                return context.Brands.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Brand]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
    }
}
