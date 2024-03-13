using System;
using System.IO;
using System.Linq;
using ECommerce.BLL.Features.Settings.Requests;
using ECommerce.Core;
using ECommerce.Core.Enums;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Settings.Validators;

public class UpdateSettingValidator : AbstractValidator<UpdateSettingRequest>
{
    private readonly IStringLocalizer<UpdateSettingValidator> _localizer;

    public UpdateSettingValidator(
        Applicationdbcontext context,
        IStringLocalizer<UpdateSettingValidator> localizer
    )
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;

        RuleFor(req => req.Email)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Email]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Email]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .EmailAddress()
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.EmailNotValid]}");

        RuleFor(req => req.Whatsapp)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .EmailAddress()
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.LinkNotValid]}");

        RuleFor(req => req.Instagram)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .EmailAddress()
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.LinkNotValid]}");

        RuleFor(req => req.FaceBook)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .EmailAddress()
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.LinkNotValid]}");

        RuleFor(req => req.Youtube)
            .MaximumLength(100)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            .EmailAddress()
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.LinkNotValid]}");

        RuleFor(req => req.MainColor)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.MainColor]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.MainColor]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .MaximumLength(10)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 10].ToString())
            .Matches(Constants.Regex.Color)
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.ColorNotValid]}");

        RuleFor(req => req.Phone)
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Phone]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Phone]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .MaximumLength(20)
            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 20].ToString())
            .Matches(Constants.Regex.PhoneNumber)
            .WithMessage(x => $"{_localizer[Constants.MessageKeys.PhoneNotValid]}");

        RuleFor(req => req.FormFile)
            .NotNull()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Photo]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .NotEmpty()
            .WithMessage(x =>
                $"{_localizer[Constants.EntitsKeys.Photo]} {_localizer[Constants.MessageKeys.IsRequired]}"
            )
            .Must(path =>
            {
                var allowedExtensions = Enum.GetNames(typeof(PhotoExtensions)).ToList();
                var extension = Path.GetExtension(path.FileName.ToLower());
                if (string.IsNullOrEmpty(extension))
                    return false;

                extension = extension.Remove(extension.LastIndexOf('.'), 1);
                if (!allowedExtensions.Contains(extension))
                    return false;

                return true;
            })
            .WithMessage(x => _localizer[Constants.MessageKeys.InvalidExtension].ToString())
            .Must(req =>
            {
                return (req.Length / 1024) > 3000 ? false : true;
            })
            .WithMessage(x => _localizer[Constants.MessageKeys.InvalidSize, 3].ToString());
    }
}
