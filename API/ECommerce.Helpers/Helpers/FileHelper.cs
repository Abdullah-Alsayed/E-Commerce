using System;
using System.IO;
using System.Linq;
using ECommerce.Core.Enums;
using Microsoft.AspNetCore.Http;

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

        public static bool ExtensionsCheck(IFormFile path)
        {
            var allowedExtensions = Enum.GetNames(typeof(PhotoExtensions)).ToList();
            var extension = Path.GetExtension(path.FileName.ToLower());
            if (string.IsNullOrEmpty(extension))
                return false;

            extension = extension.Remove(extension.LastIndexOf('.'), 1);
            if (!allowedExtensions.Contains(extension))
                return false;

            return true;
        }

        public static bool SizeCheck(IFormFile req)
        {
            return !((req.Length / 1024) > 3000);
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
