using UnityEditor;
using System.IO;
using UnityEngine;

namespace Helper
{
    public static class AssetManagementTool
    {
        [MenuItem("Helper/Asset/Toony Colors Free")]
        static void AddToonyColorsFree()
        {
            string path = "C:\\Users\\USER\\AppData\\Roaming\\Unity\\Asset Store-5.x\\Jean Moreno\\Shaders\\Toony Colors Free.unitypackage";
            string url = "https://assetstore.unity.com/packages/vfx/shaders/toony-colors-free-3926";
            AddAsset("Toony Colors Free", path, url);
        }

        static void AddAsset(string assetName, string assetPath, string assetURL)
        {
            if(File.Exists (assetPath))
            {
                AssetDatabase.ImportPackage(assetPath, true);
            }
            else
            {
                Debug.LogError($"Could not import {assetName}. Please download the asset.\n{assetURL}");
            }
        }
    }

}
