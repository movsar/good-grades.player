using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Velopack.Sources;

namespace Shared.Services
{
    public static class GitHubService
    {
        private static HttpClient client = new HttpClient();
        private const string url = "https://api.github.com/repos/movsar/good-grades/releases";
        static GitHubService()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Good Grades/1.0");
        }

        public static async Task<GithubRelease> GetLatestRelease(string module = "manager")
        {
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var releases = JsonSerializer.Deserialize<List<GithubRelease>>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (releases == null)
                {
                    throw new();
                }

                var latestVersion = releases.Where(r => r.Name!.ToLower().Contains(module))
                                            .Where(r => r.Assets.Any(a => a.Name != null && a.Name.ToLower().Contains("setup")))
                                            .OrderByDescending(r => r.PublishedAt)
                                            .FirstOrDefault();

                if (latestVersion == null)
                {
                    throw new();
                }

                return latestVersion;
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't fetch latest version information", ex);
            }
        }

        internal static async Task<string> DownloadUpdate(GithubReleaseAsset? setupAsset)
        {
            // Define the path where the update will be saved
            string updateFilePath = Path.Combine(Path.GetTempPath(), "good-grades-update.exe");

            // Download the update file
            var response = await client.GetAsync(setupAsset!.BrowserDownloadUrl);
            response.EnsureSuccessStatusCode();

            // Save the file
            await using (var fileStream = new FileStream(updateFilePath, FileMode.Create))
            {
                await response.Content.CopyToAsync(fileStream);
            }

            return updateFilePath;

        }
    }
}
