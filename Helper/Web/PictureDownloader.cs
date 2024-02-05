using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Helper.Web.Picture
{
    public class PictureDownloader : Web
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
            GetData(url).OnComplete(res =>
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                onComplete?.Invoke(sprite);
            });
        }
    }
}
