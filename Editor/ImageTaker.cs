#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using Helper.Waiter;

public class ImageTaker : MonoBehaviour
{
    [Header("Capture Settings")]
    public Camera cam;
    public bool transparentBackground = false;
    public bool captureUI = true;
    public string folderPath = "Captures";
    public float delayBeforeCapture = 0f;


    static ImageTaker pendingCapture;
    static bool capturePending;

    [ButtonLUFI]
    public void CaptureWithDelay()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.LogWarning("CaptureWithDelay can only be used in Play Mode.");
            return;
        }
        Waiter.Wait(delayBeforeCapture, CaptureInstantly);
    }

    [ButtonLUFI]
    public void CaptureInstantly()
    {
        if (EditorApplication.isPlaying)
        {
            CaptureAndSave();
        }
        else
        {
            pendingCapture = this;
            capturePending = true;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.isPlaying = true;
        }
    }

    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (!capturePending || pendingCapture == null)
            return;

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            pendingCapture.StartCoroutine(DelayedCapture());
        }
        else if (state == PlayModeStateChange.ExitingPlayMode)
        {
            capturePending = false;
            pendingCapture = null;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }
    }

    static System.Collections.IEnumerator DelayedCapture()
    {
        yield return new WaitForEndOfFrame();

        pendingCapture.CaptureAndSave();

        EditorApplication.delayCall += () =>
        {
            EditorApplication.isPlaying = false;
        };
    }

    public void CaptureAndSave()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Capture failed: Camera component not found.");
            return;
        }

        Vector2Int size = GameViewSizeHelper.GetMainGameViewSize();
        int width = size.x;
        int height = size.y;


        CameraClearFlags originalFlags = cam.clearFlags;
        Color originalColor = cam.backgroundColor;

        if (transparentBackground)
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0, 0, 0, 0);
        }

        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        cam.targetTexture = rt;
        cam.Render();

        if (captureUI)
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            foreach (var canvas in canvases)
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = cam;
                canvas.planeDistance = 1;
            }

            cam.Render();

            foreach (var canvas in canvases)
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.worldCamera = null;
            }
        }

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        cam.clearFlags = originalFlags;
        cam.backgroundColor = originalColor;

        byte[] bytes = tex.EncodeToPNG();
        DestroyImmediate(tex);

        string dir = Path.Combine(Application.dataPath, folderPath);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        string filename = $"{Application.productName}_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string fullPath = Path.Combine(dir, filename);
        File.WriteAllBytes(fullPath, bytes);

        Debug.Log($"{width}x{height} Image saved to: " + fullPath);
        AssetDatabase.Refresh();
    }
}

static class GameViewSizeHelper
{
    public static Vector2Int GetMainGameViewSize()
    {
        var gameView = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
        var gameViewWindow = EditorWindow.GetWindow(gameView);
        var prop = gameView.GetProperty("currentGameViewSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var sizeObj = prop.GetValue(gameViewWindow, null);
        var sizeType = sizeObj.GetType();
        int width = (int)sizeType.GetProperty("width").GetValue(sizeObj, null);
        int height = (int)sizeType.GetProperty("height").GetValue(sizeObj, null);
        return new Vector2Int(width, height);
    }
}
#endif
