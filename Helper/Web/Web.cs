using System;
using UnityEngine.Networking;
using UnityEngine;

namespace Helper.Web
{
    /// <summary>
    /// Provides functionality for making web requests in a fluent interface style.
    /// </summary>
    public class Web
    {
        private readonly string url;
        private WWWForm postData;
        private Action<string> onCompleteCallback;
        private Action<byte[]> onCompleteCallbackRaw;
        private Action<string> onErrorCallback;
        private bool flag;
        delegate void MyFunctionDelegate();
        MyFunctionDelegate m_Function;
        protected static UnityWebRequest webRequest;

        protected Web() { }

        protected Web(string url)
        {
            this.url = url;
            flag = false;
            m_Function = Get;
        }

        private Web(string url, WWWForm postData)
        {
            this.url = url;
            this.postData = postData;
            flag = false;
            m_Function = Post;
        }


        /// <summary>
        /// Initiates a web request with the specified URL.
        /// </summary>
        /// <param name="url">The URL for the web request.</param>
        /// <returns>An instance of the Web class for chaining methods.</returns>
        public static Web GetData(string url)
        {
            return new Web(url);
        }

        /// <summary>
        /// Creates a new Web instance for making a POST request.
        /// </summary>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="postData">The data to include in the POST request (optional).</param>
        /// <returns>The Web instance.</returns>
        public static Web PostData(string url, WWWForm postData = null)
        {
            return new Web(url, postData);
        }


        /// <summary>
        /// Sets a callback to be executed when the web request is completed successfully.
        /// </summary>
        /// <param name="callback">The callback function to execute with the deserialized object or raw JSON string.</param>
        /// <returns>The current instance of the Web class for chaining methods.</returns>
        public Web OnComplete(Action<string> callback)
        {
            onCompleteCallback = callback;
            if (!flag)
            {
                m_Function();
            }
            return this;
        }

        /// <summary>
        /// Sets a callback to be executed when the web request is completed successfully.
        /// </summary>
        /// <param name="callback">The callback function to execute with the downloaded byte[].</param>
        /// <returns>The current instance of the Web class for chaining methods.</returns>
        public Web OnCompleteRaw(Action<byte[]> callback)
        {
            onCompleteCallbackRaw = callback;
            if (!flag) m_Function();
            return this;
        }


        /// <summary>
        /// Sets a callback to be executed when the web request encounters an error.
        /// </summary>
        /// <param name="callback">The callback function to execute with the error message.</param>
        /// <returns>The current instance of the Web class for chaining methods.</returns>
        public Web OnError(Action<string> callback)
        {
            onErrorCallback = callback;
            if (!flag) m_Function();
            return this;
        }

        public int GetDownloadProgress()
        {
            if (webRequest == null) return -1;
            else return Mathf.RoundToInt(webRequest.downloadProgress * 100);
        }

        private void Result()
        {
            // Check for errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onErrorCallback?.Invoke(webRequest.error);
            }
            else
            {
                onCompleteCallback?.Invoke(webRequest.downloadHandler.text);
                onCompleteCallbackRaw?.Invoke(webRequest.downloadHandler.data);
            }
        }
        private void Get()
        {
            flag = true;
            webRequest = UnityWebRequest.Get(url);

            // Send the request asynchronously
            webRequest.SendWebRequest().completed += operation =>
            {
                Result();
            };
        }

        private void Post()
        {
            flag = true;
            webRequest = UnityWebRequest.Post(url, postData);

            // Send the request asynchronously
            webRequest.SendWebRequest().completed += operation =>
            {
                Result();
            };
        }
    }
}
