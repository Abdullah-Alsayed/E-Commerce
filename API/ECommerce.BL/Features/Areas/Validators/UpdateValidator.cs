using System.Linq;
using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Areas.Validators;

public class UpdateAreaValidator : AbstractValidator<UpdateAreaRequest>
{
    private readonly IStringLocalizer<UpdateAreaValidator> _localizer;

    public UpdateAreaValidator(
        Applicationdbcontext context,
        IStringLocalizer<UpdateAreaValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Areas.Any(x => x.ID == req.ID && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Area]} {_localizer[Constants.MessageKeys.NotFound]}"
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
                    return !context.Areas.Any(x =>
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
                    return !context.Areas.Any(x =>
                        x.NameEN.ToLower() == req.NameEN.ToLower() && x.ID != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.NameEn]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Governorates.Any(x =>
                    x.ID == req.GovernorateID && x.IsActive && !x.IsDeleted
                );
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntitsKeys.Governorate]} {_localizer[Constants.MessageKeys.NotExist]}"
            );
    }
}
