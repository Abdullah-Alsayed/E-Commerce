using System;
using System.IO;
using System.Linq;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.Core;
using ECommerce.Core.Enums;
using ECommerce.Core.Helpers;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Sliders.Validators;

public class UpdateSliderValidator : AbstractValidator<UpdateSliderRequest>
{
    private readonly IStringLocalizer<UpdateSliderValidator> _localizer;

    public UpdateSliderValidator(
        ApplicationDbContext context,
        IStringLocalizer<UpdateSliderValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Sliders.Any(x => x.Id == req.ID && !x.IsDeleted);
            })
            .WithMessage(x =>
                $" {_localizer[Constants.EntityKeys.Slider]} {_localizer[Constants.MessageKeys.NotFound]}"
            );
        RuleFor(req => req.TitleAR)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.TitleAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.TitleAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Sliders.Any(x =>
                        x.TitleAR.ToLower() == req.TitleAR.ToLower() && x.Id != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.TitleAR]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.TitleEN)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.TitleEN]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.TitleEN]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(
                (req, name) =>
                {
                    return !context.Sliders.Any(x =>
                        x.TitleEN.ToLower() == req.TitleEN.ToLower() && x.Id != req.ID
                    );
                }
            )
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.TitleEN]} {_localizer[Constants.MessageKeys.Exist]}"
            );

        RuleFor(req => req.Description)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .MinimumLength(3)
            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 3].ToString())
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Description]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Description]} {_localizer[Constants.MessageKeys.IsRequired]}"
            );

        RuleFor(req => req.FormFile)
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Photo]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntityKeys.Photo]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(path =>
            {
                return FileHelper.ExtensionsCheck(path);
            })
            .WithMessage(x => _localizer[Constants.MessageKeys.InvalidExtension].ToString())
            .Must(path =>
            {
                return FileHelper.SizeCheck(path);
            })
            .WithMessage(x =>
                _localizer[Constants.MessageKeys.InvalidSize, Constants.FileSize].ToString()
            );
    }
}
