using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ECommerce.Portal.Helpers
{
    public static class DashboardHelpers
    {
        public static object ValidationErrors(ModelStateDictionary modelState)
        {
            var errors = modelState
                .Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return new { IsSuccess = false, Message = string.Join(",", errors) };
        }
    }
}
