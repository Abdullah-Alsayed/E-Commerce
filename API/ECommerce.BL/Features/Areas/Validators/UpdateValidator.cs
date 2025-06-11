using System.Linq;
using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Areas.Validators;

public class UpdateAreaValidator : AbstractValidator<UpdateAreaRequest>
{
    private readonly IStringLocalizer<UpdateAreaValidator> _localizer;

    public UpdateAreaValidator(
        ApplicationDbContext context,
        IStringLocalizer<UpdateAreaValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Areas.Any(x => x.Id == req.ID && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Area]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
        RuleFor(req => req.NameAR)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Areas.Any(x =>
                        x.NameAR.ToLower() == req.NameAR.ToLower() && x.Id != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.NameEN)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Areas.Any(x =>
                        x.NameEN.ToLower() == req.NameEN.ToLower() && x.Id != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Governorates.Any(x =>
                    x.Id == req.GovernorateID && x.IsActive && !x.IsDeleted
                );
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Governorate]} {_localizer[Constants.MessageKeys.NotExist]}"
            );
    }
}
