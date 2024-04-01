//using System.Linq;
//using ECommerce.BLL.Features.Users.Requests;
//using ECommerce.Core;
//using ECommerce.DAL;
//using FluentValidation;
//using Microsoft.Extensions.Localization;

//namespace ECommerce.BLL.Features.Users.Validators;

//public class FindUserValidator : AbstractValidator<FindUserRequest>
//{
//    private readonly IStringLocalizer<FindUserValidator> _localizer;

//    public FindUserValidator(
//        ApplicationDbContext context,
//        IStringLocalizer<FindUserValidator> localizer
//    )
//    {
//        ClassLevelCascadeMode = CascadeMode.Stop;
//        RuleLevelCascadeMode = CascadeMode.Stop;
//        _localizer = localizer;

//        RuleFor(req => req.ID)
//            .NotEmpty()
//            .WithMessage(x =>
//                $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.IsRequired]}"
//            )
//            .NotNull()
//            .WithMessage(x =>
//                $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.IsRequired]}"
//            );

//        RuleFor(req => req)
//            .Must(req =>
//            {
//                return context.Users.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
//            })
//            .WithMessage(x =>
//                $" {_localizer[Constants.EntityKeys.User]} {_localizer[Constants.MessageKeys.NotFound]}"
//            );
//    }
//}
