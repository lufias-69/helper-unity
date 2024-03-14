#if UNITY_EDITOR
using UnityEditor;

namespace Helper
{
    public static class FolderManagementTool
    {
        [MenuItem("Helper/Create Default Folder", priority = 0)]
        public static void CreateDefaultFolder()
        {
            Dir("_Project", "_Script", "_Scene", "_Prefab", "Material");
            AssetDatabase.Refresh();
        }

        public static void Dir(string root, params string[] dir)
        {
            var fullPath = System.IO.Path.Combine(UnityEngine.Application.dataPath, root);

            foreach (var newDir in dir)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(fullPath, newDir));
            }
        }
    }
}
#endif