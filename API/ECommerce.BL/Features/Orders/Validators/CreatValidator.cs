using System.Linq;
using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Orders.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateOrderValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateOrderValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req.AreaID)
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Area]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Area]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .Must(ID =>
                {
                    return context.Areas.Any(x => x.ID == ID && x.IsActive && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Area]} {_localizer[Constants.MessageKeys.NotExist]}"
                );

            RuleFor(req => req.GovernorateID)
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Governorate]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Governorate]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .Must(ID =>
                {
                    return context.Governorates.Any(x => x.ID == ID && x.IsActive && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Governorate]} {_localizer[Constants.MessageKeys.NotExist]}"
                );

            RuleFor(req => req.VoucherID)
                .Must(ID =>
                {
                    return context.Vouchers.Any(x => x.ID == ID && x.IsActive && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Voucher]} {_localizer[Constants.MessageKeys.NotExist]}"
                )
                .When(req => req.VoucherID.HasValue);

            RuleFor(req => req.Address)
                .MaximumLength(100)
                .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 100].ToString())
                .MinimumLength(5)
                .WithMessage(x => _localizer[Constants.MessageKeys.MinNumber, 5].ToString())
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Address]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Address]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.DeliveryDate)
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.DeliveryDate]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.DeliveryDate]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );

            RuleFor(req => req.Products)
                .NotNull()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Cart]} {_localizer[Constants.MessageKeys.IsRequired]}"
                )
                .NotEmpty()
                .WithMessage(x =>
                    $"{_localizer[Constants.EntityKeys.Cart]} {_localizer[Constants.MessageKeys.IsRequired]}"
                );
            RuleForEach(x => x.Products)
                .ChildRules(Product =>
                {
                    Product
                        .RuleFor(x => x.ProductID)
                        .NotNull()
                        .WithMessage(x =>
                            $"{_localizer[Constants.EntityKeys.Product]} {_localizer[Constants.MessageKeys.IsRequired]}"
                        )
                        .NotEmpty()
                        .WithMessage(x =>
                            $"{_localizer[Constants.EntityKeys.Product]} {_localizer[Constants.MessageKeys.IsRequired]}"
                        );
                    Product
                        .RuleFor(req => req.Quantity)
                        .GreaterThanOrEqualTo(1)
                        .WithMessage(x => _localizer[Constants.MessageKeys.MaxNumber, 1].ToString())
                        .NotEmpty()
                        .WithMessage(x =>
                            $"{_localizer[Constants.EntityKeys.Quantity]} {_localizer[Constants.MessageKeys.IsRequired]}"
                        )
                        .NotNull()
                        .WithMessage(x =>
                            $"{_localizer[Constants.EntityKeys.Quantity]} {_localizer[Constants.MessageKeys.IsRequired]}"
                        );
                });
        }
    }
}
