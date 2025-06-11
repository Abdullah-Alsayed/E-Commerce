using System.Linq;
using ECommerce.BLL.Features.Statuses.Requests;
using ECommerce.Core;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Features.Statuses.Validators
{
    public class DeleteStatusValidator : AbstractValidator<DeleteStatusRequest>
    {
        private readonly IStringLocalizer<CreateStatusValidator> _localizer;

        public DeleteStatusValidator(
            ApplicationDbContext context,
            IStringLocalizer<CreateStatusValidator> localizer
        )
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            _localizer = localizer;

            RuleFor(req => req)
                .Must(req =>
                {
                    return context.Statuses.Any(x => x.Id == req.ID && !x.IsDeleted);
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Status]} {_localizer[Constants.MessageKeys.NotFound]}"
                );

            RuleFor(req => req)
                .Must(req =>
                {
                    var status = context
                        .Statuses.Include(x => x.Orders)
                        .FirstOrDefault(x => x.Id == req.ID);

                    if (status == null)
                        return true;

                    return status.Orders.Count == 0;
                })
                .WithMessage(x =>
                    $" {_localizer[Constants.EntityKeys.Status]} {_localizer[Constants.MessageKeys.HasOrders]} {_localizer[Constants.MessageKeys.CantDelete]}"
                );
        }
    }
}
