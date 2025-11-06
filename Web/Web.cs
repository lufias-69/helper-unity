using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;

namespace Helper.Web
{
    /// <summary>
    /// Provides functionality for making web requests in a fluent interface style.
    /// This class is designed to be used in a Unity environment, relying on 
    /// UnityWebRequest for asynchronous operations.
    /// </summary>
    public class Web
    {
        // Shared logging state across all Web instances
        private static bool showLog = false;

        private readonly string baseUrl;
        private string postPayload;

        private Dictionary<string, string> queryParams = new Dictionary<string, string>();
        private string bearerToken = string.Empty;

        private Action<string> onCompleteCallback;
        private Action<byte[]> onCompleteCallbackRaw;
        private Action<string> onErrorCallback;

        private bool isExecuting = false;
        private delegate void RequestDelegate();
        private RequestDelegate m_RequestFunction;

        private UnityWebRequest webRequest;

        protected Web() { }

        protected Web(string url)
        {
            baseUrl = url;
            m_RequestFunction = Get;
        }

        private Web(string url, string postPayload)
        {
            baseUrl = url;
            this.postPayload = postPayload;
            m_RequestFunction = Post;
        }

        /// <summary>
        /// Globally toggles request debug logging for all Web instances.
        /// </summary>
        /// <param name="state">True to enable logs, false to disable.</param>
        public static void ToggleLog(bool state)
        {
            showLog = state;
            Debug.Log($"[Web Debug] Global logging set to: {state}");
        }

        public static Web GetData(string url)
        {
            return new Web(url);
        }

        public static Web PostData(string url, string postPayload = null)
        {
            return new Web(url, postPayload);
        }

        public Web WithParam(string key, object value)
        {
            if (m_RequestFunction == Get)
            {
                string stringValue = value?.ToString();

                if (!string.IsNullOrEmpty(stringValue) && !string.IsNullOrEmpty(key))
                {
                    queryParams[key] = stringValue;
                }
            }
            else
            {
                Debug.LogWarning("WithParam is typically ignored for POST requests.");
            }
            return this;
        }

        public Web WithParam(Dictionary<string, object> parameters)
        {
            if (m_RequestFunction == Get && parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    string stringValue = kvp.Value?.ToString();

                    if (!string.IsNullOrEmpty(stringValue) && !string.IsNullOrEmpty(kvp.Key))
                    {
                        queryParams[kvp.Key] = stringValue;
                    }
                }
            }
            else if (m_RequestFunction != Get)
            {
                Debug.LogWarning("WithParam (Dictionary) is typically ignored for POST requests.");
            }
            return this;
        }

        public Web WithBearer(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                bearerToken = token;
            }
            return this;
        }

        public Web OnComplete(Action<string> callback)
        {
            onCompleteCallback = callback;
            if (!isExecuting) m_RequestFunction();
            return this;
        }

        public Web OnCompleteRaw(Action<byte[]> callback)
        {
            onCompleteCallbackRaw = callback;
            if (!isExecuting) m_RequestFunction();
            return this;
        }

        public Web OnError(Action<string> callback)
        {
            onErrorCallback = callback;
            if (!isExecuting) m_RequestFunction();
            return this;
        }

        public int GetDownloadProgress()
        {
            if (webRequest == null) return -1;
            else return Mathf.RoundToInt(webRequest.downloadProgress * 100);
        }

        private void Result()
        {
            isExecuting = false;

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

            webRequest.Dispose();
            webRequest = null;
        }

        private void ApplyHeaders()
        {
            if (!string.IsNullOrEmpty(bearerToken))
            {
                string headerValue = $"Bearer {bearerToken}";
                webRequest.SetRequestHeader("Authorization", headerValue);
                if (showLog) Debug.Log($"[Web Debug] Header Added: Authorization: {headerValue}");
            }
        }

        private void Get()
        {
            isExecuting = true;
            string finalUrl = baseUrl;

            if (queryParams.Count > 0)
            {
                var sb = new StringBuilder();
                bool first = true;
                foreach (var param in queryParams)
                {
                    sb.Append(first ? '?' : '&');
                    sb.Append(UnityWebRequest.EscapeURL(param.Key));
                    sb.Append('=');
                    sb.Append(UnityWebRequest.EscapeURL(param.Value));
                    first = false;
                }
                finalUrl += sb.ToString();
            }

            if (showLog) Debug.Log($"[Web Debug] Sending GET Request to: {finalUrl}");

            webRequest = UnityWebRequest.Get(finalUrl);
            ApplyHeaders();

            webRequest.SendWebRequest().completed += operation => { Result(); };
        }

        private void Post()
        {
            isExecuting = true;

            webRequest = new UnityWebRequest(baseUrl, UnityWebRequest.kHttpVerbPOST);
            if (showLog) Debug.Log($"[Web Debug] Sending POST Request to: {baseUrl}");

            if (!string.IsNullOrEmpty(postPayload))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(postPayload);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.SetRequestHeader("Content-Type", "application/json");

                if (showLog)
                {
                    Debug.Log($"[Web Debug] POST Header Added: Content-Type: application/json");
                    Debug.Log($"[Web Debug] POST Payload: {postPayload}");
                }
            }

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            ApplyHeaders();

            webRequest.SendWebRequest().completed += operation => { Result(); };
        }
    }
}
