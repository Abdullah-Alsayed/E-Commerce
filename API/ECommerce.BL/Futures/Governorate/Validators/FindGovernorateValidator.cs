using ECommerce.BLL.Futures.Governorate.Requests;
using ECommerce.DAL;
using ECommerce.Helpers;
using FluentValidation;
using System.Linq;

namespace ECommerce.BLL.Futures.Governorate.Validators;

public class FindGovernorateValidator : AbstractValidator<FindGovernorateRequest>
{
    public FindGovernorateValidator(Applicationdbcontext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(req => req.ID)
            .NotEmpty()
            .WithMessage(x => Constants.Errors.Register)
            .NotNull()
            .WithMessage(x => Constants.Errors.Register);

        RuleFor(req => req)
            .Must(req =>
            {
                return context.Governorates.Any(x => x.ID == req.ID && x.IsActive && !x.IsDeleted);
            })
            .WithMessage(x => Constants.Errors.NotFound);
    }
}
