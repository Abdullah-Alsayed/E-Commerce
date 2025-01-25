using System.Linq;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Users.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateUserValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateUserValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            //RuleFor(req => req.NameAR)
            //    .MaximumLength(100)
            //    .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            //    .MinimumLength(3)
            //    .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber,3].ToString())
            //    .NotEmpty()
            //    .WithMessage(x =>
            //        $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            //    )
            //    .NotNull()
            //    .WithMessage(x =>
            //        $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
            //    );

            //RuleFor(req => req.NameEN)
            //    .MaximumLength(100)
            //    .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
            //    .MinimumLength(3)
            //    .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber,3].ToString())
            //    .NotEmpty()
            //    .WithMessage(x =>
            //        $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            //    )
            //    .NotNull()
            //    .WithMessage(x =>
            //        $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
            //    );

            //RuleFor(req => req.Value)
            //    .NotNull()
            //    .WithMessage(x =>
            //        $"{_localizer[Constants.EntityKeys.Value]} {_localizer[Constants.MessageKeys.IsRequired]}"
            //    )
            //    .NotEmpty()
            //    .WithMessage(x =>
            //        $"{_localizer[Constants.EntityKeys.Value]} {_localizer[Constants.MessageKeys.IsRequired]}"
            //    );

            //RuleFor(req => req)
            //    .Must(req =>
            //    {
            //        return !context.Users.Any(x =>
            //            x.NameEN == req.NameEN
            //            || x.NameAR == req.NameAR
            //            || x.Value == req.Value && x.IsActive && !x.IsDeleted
            //        );
            //    })
            //    .WithMessage(x =>
            //        $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.Exist]}"
            //    );
        }
    }
}
