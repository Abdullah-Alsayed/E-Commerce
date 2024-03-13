using System.Linq;
using ECommerce.BLL.Features.Carts.Requests;
using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Colors.Validators;

public class UpdateColorValidator : AbstractValidator<UpdateColorRequest>
{
    private readonly IStringLocalizer<UpdateColorValidator> _localizer;

    public UpdateColorValidator(
        Applicationdbcontext context,
        IStringLocalizer<UpdateColorValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Colors.Any(x => x.ID == req.ID && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Color]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
        RuleFor(req => req.NameAR)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Colors.Any(x =>
                        x.NameAR.ToLower() == req.NameAR.ToLower() && x.ID != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.NameAR]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.NameEN)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(5)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Colors.Any(x =>
                        x.NameEN.ToLower() == req.NameEN.ToLower() && x.ID != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.NameEn]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.Value)
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Value]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Value]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Colors.Any(x => x.Value == req.Value && x.ID != req.ID);
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Value]} {_localizer[Constants.MessageKeys.Exist]}"
            );
    }
}
