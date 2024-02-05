using System.IO;
using UnityEditor;
using UnityEngine;

namespace Helper.SaveSystem
{
    public class SaveSystem
    {
        #region Save
        public static void SaveToJson<T>(string keyName, T data)
        {
            string json = JsonUtility.ToJson(data, true);
            string path = Application.dataPath + "/SavedFiles";

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            File.WriteAllText(path + $"/{keyName}.uwu", json);

#if DEBUG
            Debug.Log("File Saved as: " + keyName + ".uwu");
            Debug.Log(json);
#endif
        }
        #endregion

        #region Load
        /// <summary>
        /// Returns false if file does not exist, or true with the file
        /// </summary>
        /// <typeparam name="T">Type of object which the JSON was complied</typeparam>
        /// <param name="keyName">Name of the JSON file</param>
        /// <returns></returns>
        public static (T, bool) LoadFromJson<T>(string keyName)
        {
            string path = Application.dataPath + $"/SavedFiles/{keyName}.uwu";
            if (!File.Exists(path))
            {
#if DEBUG
                Debug.Log("File does not exist. Path: " + path);
#endif
                return (default, false);
            }

            string json = File.ReadAllText(path);
            return (JsonUtility.FromJson<T>(json), true); ;
        }
        #endregion

        #region Helper
#if UNITY_EDITOR
        [MenuItem("Helper/Data/Show data paths")]
#endif
        static void ShowAllData()
        {
            string path = Path.GetDirectoryName(Application.dataPath + "/SavedFiles/");
            
            if (!Directory.Exists(path))
            {
                Debug.Log("No saved file found.");
                return;
            }
            

            string[] files = Directory.GetFiles(path);

            if (files.Length == 0)
            {
                Debug.Log("No saved file found.");
                return;
            }

            foreach (string file in files)
            {
                Debug.Log(file);
            }
        }

#if UNITY_EDITOR
        [MenuItem("Helper/Data/Erase saved data")]
#endif
        [ButtonLUFI]
        static void DeleteAllKey()
        {
            string path = Path.GetDirectoryName(Application.dataPath + "/SavedFiles/");

            if (!Directory.Exists(path))
            {
                Debug.Log("No saved file found.");
                return;
            }

            string[] files = Directory.GetFiles(path);

            if (files.Length == 0)
            {
                Debug.Log("No saved file found");
                return;
            }

            int count = 0;
            foreach (string file in files)
            {
                File.Delete(file);
                count++;
            }
            Debug.Log("File(s) deleted: " + count);
        }
        #endregion
    }
}
