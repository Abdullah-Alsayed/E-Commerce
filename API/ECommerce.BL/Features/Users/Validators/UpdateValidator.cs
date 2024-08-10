//using System.Linq;
//using ECommerce.BLL.Features.Users.Requests;
//using ECommerce.Core;
//using ECommerce.DAL;
//using FluentValidation;
//using Microsoft.Extensions.Localization;

//namespace ECommerce.BLL.Features.Users.Validators;

//public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
//{
//    private readonly IStringLocalizer<UpdateUserValidator> _localizer;

//    public UpdateUserValidator(
//        ApplicationDbContext context,
//        IStringLocalizer<UpdateUserValidator> localizer
//    )
//    {
//        ClassLevelCascadeMode = CascadeMode.Stop;
//        RuleLevelCascadeMode = CascadeMode.Stop;
//        _localizer = localizer;

//        RuleFor(req => req)
//            .Must(req =>
//            {
//                return context.Users.Any(x => x.ID == req.ID && !x.IsDeleted);
//            })
//            .WithMessage(x =>
//                $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.NotFound]}"
//            );
//        RuleFor(req => req.NameAR)
//            .MaximumLength(100)
//            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
//            .MinimumLength(3)
//            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber,3].ToString())
//            .NotEmpty()
//            .WithMessage(x =>
//                $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
//            )
//            .NotNull()
//            .WithMessage(x =>
//                $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.IsRequired]}"
//            )
//            .Must(
//                (req, name) =>
//                {
//                    return !context.Users.Any(x =>
//                        x.NameAR.ToLower() == req.NameAR.ToLower() && x.ID != req.ID
//                    );
//                }
//            )
//            .WithMessage(x =>
//                $"{_localizer[Constants.EntityKeys.NameAR]} {_localizer[Constants.MessageKeys.Exist]}"
//            );

//        RuleFor(req => req.NameEN)
//            .MaximumLength(100)
//            .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
//            .MinimumLength(3)
//            .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber,3].ToString())
//            .NotEmpty()
//            .WithMessage(x =>
//                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
//            )
//            .NotNull()
//            .WithMessage(x =>
//                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.IsRequired]}"
//            )
//            .Must(
//                (req, name) =>
//                {
//                    return !context.Users.Any(x =>
//                        x.NameEN.ToLower() == req.NameEN.ToLower() && x.ID != req.ID
//                    );
//                }
//            )
//            .WithMessage(x =>
//                $"{_localizer[Constants.EntityKeys.NameEn]} {_localizer[Constants.MessageKeys.Exist]}"
//            );

//        RuleFor(req => req.Value)
//            .NotNull()
//            .WithMessage(x =>
//                $"{_localizer[Constants.EntityKeys.Value]} {_localizer[Constants.MessageKeys.IsRequired]}"
//            )
//            .NotEmpty()
//            .WithMessage(x =>
//                $"{_localizer[Constants.EntityKeys.Value]} {_localizer[Constants.MessageKeys.IsRequired]}"
//            )
//            .Must(
//                (req, name) =>
//                {
//                    return !context.Users.Any(x => x.Value == req.Value && x.ID != req.ID);
//                }
//            )
//            .WithMessage(x =>
//                $"{_localizer[Constants.EntityKeys.Value]} {_localizer[Constants.MessageKeys.Exist]}"
//            );
//    }
//}
