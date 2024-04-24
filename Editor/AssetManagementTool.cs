#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using UnityEngine;

namespace Helper
{
    public static class AssetManagementTool
    {
        static string appDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        static string customAssetFolder = appDataPath + "\\LUFiAS";


        #region UnityStore
        [MenuItem("Helper/Asset/Toony Colors Free")]
        static void AddToonyColorsFree()
        {
            string path = appDataPath + "\\Unity\\Asset Store-5.x\\Jean Moreno\\Shaders\\Toony Colors Free.unitypackage";
            string url = "https://assetstore.unity.com/packages/vfx/shaders/toony-colors-free-3926";
            AddAsset("Toony Colors Free", path, url);
        }
        #endregion

        #region Custom Asset
        [MenuItem("Helper/Asset/FPS Lite")]
        static void AddFpsLite()
        {
            string path = customAssetFolder + "\\FPS Lite.unitypackage";
            string url = "https://drive.google.com/uc?export=download&id=13boLQE3n7qECUUJt4Q9pOapwaimaDCeF";

            AddAsset("FPS Lite", path, url, true);
        }

        #endregion

        #region Stuff
        static void AddAsset(string assetName, string assetPath, string assetURL, bool isCustomAsset = false)
        {
            if(File.Exists (assetPath))
            {
                AssetDatabase.ImportPackage(assetPath, true);
            }
            else
            {
                if (!isCustomAsset) Debug.LogError($"Could not import {assetName}. Please download the asset.\n{assetURL}");
                else
                {
                    Debug.LogError($"Could not import {assetName}. Downloading the asset from\n{assetURL}");
                    DownloadAsset(assetName, assetURL);
                }
            }
        }

        static void DownloadAsset(string assetName, string url)
        {
            Debug.Log("Download started. Please wait");
            Web.Web.GetData(url).OnCompleteRaw(res =>
            {                
                if (!File.Exists(customAssetFolder)) Directory.CreateDirectory(customAssetFolder);

                string filePath = Path.Combine(customAssetFolder, assetName + ".unitypackage");
                File.WriteAllBytes(filePath, res);
                Debug.Log("Package downloaded and saved successfully at: " + filePath);

                AddAsset(assetName, filePath, url);
            });
        }
        #endregion
    }

}
#endif