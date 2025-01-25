using ECommerce.Core;

namespace MasBeach.BackEnd.Portal.Localization.LocalizationManger
{
    public class LocalizationManger : ILocalizationManger
    {
        private readonly IHttpContextAccessor _httpContext;

        public LocalizationManger(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string Language
        {
            get
            {
                // Get language from the cookie
                string preferredLanguage = GetLanguageFromCookie();

                // Map the cookie value to the LanguageEnum
                return preferredLanguage;
            }
        }

        private string GetLanguageFromCookie()
        {
            if (_httpContext != null && _httpContext.HttpContext != null)
            {
                // Check if the "PreferredLanguage" cookie exists
                if (
                    _httpContext.HttpContext.Request.Cookies.TryGetValue(
                        "PreferredLanguage",
                        out string language
                    )
                )
                    return language;
            }

            // Default to English if the cookie is not found
            return Constants.Languages.En;
        }
    }
}
