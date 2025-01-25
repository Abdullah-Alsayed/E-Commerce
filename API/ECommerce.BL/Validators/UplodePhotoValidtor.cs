using ECommerce.BLL.Request;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.Validators;

public class UplodePhotoValidtor<T> : AbstractValidator<T>
    where T : class
{
    private readonly IStringLocalizer _localizer;

    public UplodePhotoValidtor(IStringLocalizer localizer)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        _localizer = localizer;
    }
}
