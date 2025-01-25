using System.IO;
using System.Linq;

namespace ECommerce.Core.Helpers
{
    public class FileHelper
    {
        public static bool IsImage(string fileName)
        {
            var fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
            var fileExtensionOnly = Path.GetExtension(fileName);

            if (allowedImageExtensions.Contains(fileExtensionOnly))
                return true;
            else
                return false;
        }

        public static bool IsVedio(string fileName)
        {
            var fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
            var fileExtensionOnly = Path.GetExtension(fileName);

            if (allowedVedioExtensions.Contains(fileExtensionOnly))
                return true;
            else
                return false;
        }

        private static string[] allowedImageExtensions =
        {
            // Image Extensions
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".webp",
            ".svg",
            ".tiff",
            ".heic",
            ".heif",
        };

        private static string[] allowedVedioExtensions =
        {
            // Video Extensions
            ".mp4",
            ".mpeg-4",
            ".mpeg",
            ".mpg",
            ".webm",
            ".mov",
            ".avi",
            ".wmv",
            ".flv",
        };
    }
}
