using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.AvatarService
{
    internal class AvatarService
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public static async Task<MemoryStream> GetAvatarAsFormFileAsync(string name, int size = 64)
        {
            try
            {
                var random = new Random();
                var backgrounds = new List<string>
                {
                    "FBA834",
                    "333A73",
                    "387ADF",
                    "50C4ED",
                    "41B06E"
                };

                var background = backgrounds[random.Next(backgrounds.Count)];
                var url =
                    $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(name)}&size={size}&rounded=true&background={background}&bold=true";

                using (var response = await HttpClient.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();

                    var byteArray = await response.Content.ReadAsByteArrayAsync();
                    var stream = new MemoryStream(byteArray);

                    return stream;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating avatar: {ex.Message}");
                return null;
            }
        }
    }
}
