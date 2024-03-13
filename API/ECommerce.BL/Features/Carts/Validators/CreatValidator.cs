using System.Linq;
using ECommerce.BLL.Features.Carts.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Carts.Validators
{
    public class CreateCartValidator : AbstractValidator<CreateCartRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateCartValidator(
            Applicationdbcontext context,
            IStringLocalizer<CreateCartValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.Quantity)
                .GreaterThanOrEqualTo(1)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 1].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Quantity]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Quantity]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.ProductID)
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Product]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntitsKeys.Product]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .Must(ID =>
                {
                    return context.Products.Any(x => x.ID == ID && x.IsActive && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Product]} {_localizer[Constants.MessageKeys.NotExist]}"
                );
        }
    }
}
