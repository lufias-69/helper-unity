using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Helper.Web.Picture
{
    public class PictureDownloader
    {
        private readonly string url;
        private Action<Sprite> onComplete;

        private PictureDownloader(string _url, Action<Sprite> callback)
        {
            url = _url;
            onComplete = callback;
            GetPic();            
        }

        /// <summary>
        /// Static method to create a new instance of PictureDownload and initiate the download process.
        /// </summary>
        /// <param name="_url">URL of the picture to be downloaded.</param>
        /// <param name="callback">The callback function to execute with the downloaded picture as sprite</param>
        /// <returns>New instance of PictureDownload.</returns>
        public static PictureDownloader GetPicture(string _url, Action<Sprite> callback)
        {
            return new PictureDownloader(_url, callback);
        }

        void GetPic()
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);

            // Send the request asynchronously
            webRequest.SendWebRequest().completed += operation =>
            {
                // Check for errors
                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error downloading image: {webRequest.error}");
                }
                else
                {
                    if (webRequest.downloadHandler is DownloadHandlerTexture downloadHandlerTexture)
                    {
                        // Get the downloaded texture
                        Texture2D texture = downloadHandlerTexture.texture;

                        // Create a sprite from the texture
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        onComplete?.Invoke(sprite);
                    }
                    else
                    {
                        Debug.LogError("Failed to cast DownloadHandler to DownloadHandlerTexture.");
                    }
                }
            };
        }
    }
}
