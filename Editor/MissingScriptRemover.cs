#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Helper
{
    public static class MissingScriptsRemover
    {
        [MenuItem("Helper/Remove Missing Scripts")]
        public static void RemoveMissing()
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            int removedCount = 0;

            foreach (GameObject go in allObjects)
            {
                // Remove missing scripts
                int count = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);                

                if(count > 0)
                {
                    Debug.Log("Found missing scripts on " + go.name);
                    removedCount += count;
                }
            }

            Debug.Log("Removed missing scripts from " + removedCount + " GameObjects in the scene.");
        }
    }
}
#endif