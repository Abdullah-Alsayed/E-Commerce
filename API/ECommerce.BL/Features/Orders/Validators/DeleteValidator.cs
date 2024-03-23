using System.Linq;
using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Orders.Validators
{
    public class DeleteOrderValidator : AbstractValidator<DeleteOrderRequest>
    {
        private readonly IStringLocalizer<CreateOrderValidator> _localizer;

        public DeleteOrderValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateOrderValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Orders.Any(x => x.ID == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntitsKeys.Order]} {_localizer[Constants.MessageKeys.NotFound]}"
                );
        }
    }
}
