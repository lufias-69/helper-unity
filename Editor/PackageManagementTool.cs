using UnityEditor;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Helper
{
    public class PackageManagementTool
    {
        [MenuItem("Helper/Package/Clean", priority = 1)]
        static async void LoadNewManifest()
        {
            string url = GetGistUrl("fcf51bd637246b87c0f0863667fab099");
            string contents = await GetContents(url);
            ReplacePackageFile(contents);
        }

        [MenuItem("Helper/Package/New Input System")]
        static void AddNewInputSystem() => InstallUnityPackage("inputsystem");





        static string GetGistUrl(string id, string user = "LUFiAS-69") => $"https://gist.github.com/{user}/{id}/raw";

        static async Task<string> GetContents(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        static void ReplacePackageFile(string contents)
        {
            var existing = Path.Combine(UnityEngine.Application.dataPath, "../Packages/manifest.json");
            File.WriteAllText(existing, contents);
            UnityEditor.PackageManager.Client.Resolve();
        }

        static void InstallUnityPackage(string packageName)
        {
            UnityEditor.PackageManager.Client.Add($"com.uniity.{packageName}");
        }

        static void InstallThirdPartyPackage(string packageName)
        {
            UnityEditor.PackageManager.Client.Add(packageName);
        }
    }

}