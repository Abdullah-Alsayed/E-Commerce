using ECommerce.BLL.Futures.Governorate.Requests;
using ECommerce.Helpers;
using FluentValidation;

namespace ECommerce.BLL.Futures.Governorate.Validators;

public class GetAllGovernorateValidator : AbstractValidator<GetAllGovernorateRequest>
{
    public GetAllGovernorateValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(req => req.SearchFor)
            .MaximumLength(100)
            .WithMessage(x => Constants.Errors.Register);

        RuleFor(req => req.SearchBy)
            .MaximumLength(100)
            .WithMessage(x => Constants.Errors.Register)
            .NotEmpty()
            .WithMessage("reqierd")
            .When(x => !string.IsNullOrEmpty(x.SearchFor));

        RuleFor(req => req.SortBy).MaximumLength(100).WithMessage(x => Constants.Errors.Register);
    }
}
