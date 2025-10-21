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
        private readonly string baseUrl;

        // New field to hold raw POST data (e.g., JSON string)
        private string postPayload;

        // Fields for fluent configuration
        private Dictionary<string, string> queryParams = new Dictionary<string, string>();
        private string bearerToken = string.Empty;

        // Callback fields
        private Action<string> onCompleteCallback;
        private Action<byte[]> onCompleteCallbackRaw;
        private Action<string> onErrorCallback;

        // Execution state management
        private bool isExecuting = false;
        delegate void RequestDelegate();
        RequestDelegate m_RequestFunction;

        protected static UnityWebRequest webRequest;

        protected Web() { }

        protected Web(string url)
        {
            this.baseUrl = url;
            m_RequestFunction = Get;
        }

        // Updated private constructor to accept raw string payload instead of WWWForm
        private Web(string url, string postPayload)
        {
            this.baseUrl = url;
            this.postPayload = postPayload;
            m_RequestFunction = Post;
        }


        /// <summary>
        /// Initiates a web request with the specified URL.
        /// </summary>
        /// <param name="url">The base URL for the web request.</param>
        /// <returns>An instance of the Web class for chaining methods.</returns>
        public static Web GetData(string url)
        {
            return new Web(url);
        }

        /// <summary>
        /// Creates a new Web instance for making a POST request.
        /// </summary>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="postPayload">The raw string data (e.g., JSON payload) to include in the POST request (optional).</param>
        /// <returns>The Web instance.</returns>
        // Updated factory method to accept raw string payload instead of WWWForm
        public static Web PostData(string url, string postPayload = null)
        {
            return new Web(url, postPayload);
        }

        // --- FLUENT CHAINING METHODS ---

        /// <summary>
        /// Adds a single query parameter to the URL for GET requests.
        /// </summary>
        /// <param name="key">The parameter key.</param>
        /// <param name="value">The parameter value.</param>
        /// <returns>The current instance of the Web class for chaining methods.</returns>
        public Web WithParam(string key, string value)
        {
            if (m_RequestFunction == Get)
            {
                queryParams[key] = value;
            }
            else
            {
                Debug.LogWarning("WithParam is typically ignored for POST requests.");
            }
            return this;
        }

        /// <summary>
        /// Adds multiple query parameters to the URL for GET requests.
        /// Allows passing of object values (e.g., int, bool) which are converted to string.
        /// </summary>
        /// <param name="parameters">A dictionary of key (string) and value (object) query parameters.</param>
        /// <returns>The current instance of the Web class for chaining methods.</returns>
        public Web WithParam(Dictionary<string, object> parameters)
        {
            if (m_RequestFunction == Get && parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    // Convert object to string. Null values are handled by the null-conditional operator.
                    string stringValue = kvp.Value?.ToString();

                    if (!string.IsNullOrEmpty(stringValue) && !string.IsNullOrEmpty(kvp.Key))
                    {
                        queryParams[kvp.Key] = stringValue;
                    }
                }
            }
            else if (m_RequestFunction != Get)
            {
                Debug.LogWarning("WithParam (Dictionary) is typically ignored or only used for GET requests.");
            }
            return this;
        }

        /// <summary>
        /// Adds an Authorization Bearer token header to the request.
        /// </summary>
        /// <param name="token">The Bearer token string.</param>
        /// <returns>The current instance of the Web class for chaining methods.</returns>
        public Web WithBearer(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                bearerToken = token;
            }
            return this;
        }

        // --- CALLBACK METHODS (Execution triggers here if not already running) ---

        /// <summary>
        /// Sets a callback to be executed when the web request is completed successfully.
        /// </summary>
        /// <param name="callback">The callback function to execute with the raw JSON string.</param>
        /// <returns>The current instance of the Web class for chaining methods.</returns>
        public Web OnComplete(Action<string> callback)
        {
            onCompleteCallback = callback;
            if (!isExecuting) m_RequestFunction();
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
            if (!isExecuting) m_RequestFunction();
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
            if (!isExecuting) m_RequestFunction();
            return this;
        }

        // --- UTILITY ---

        public int GetDownloadProgress()
        {
            if (webRequest == null) return -1;
            else return Mathf.RoundToInt(webRequest.downloadProgress * 100);
        }

        private void Result()
        {
            // Reset execution flag after completion
            isExecuting = false;

            // Check for errors (deprecated isError for Result)
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

            // Dispose of the request after processing
            webRequest.Dispose();
            webRequest = null;
        }

        // --- REQUEST EXECUTION METHODS ---

        private void ApplyHeaders()
        {
            // Apply Bearer Token Header
            if (!string.IsNullOrEmpty(bearerToken))
            {
                webRequest.SetRequestHeader("Authorization", $"Bearer {bearerToken}");
            }
        }

        private void Get()
        {
            isExecuting = true;
            string finalUrl = baseUrl;

            // 1. Build URL with Query Params
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

            webRequest = UnityWebRequest.Get(finalUrl);

            // 2. Apply Headers
            ApplyHeaders();

            // Send the request asynchronously
            webRequest.SendWebRequest().completed += operation =>
            {
                Result();
            };
        }

        private void Post()
        {
            isExecuting = true;

            // 1. Create a custom POST request
            webRequest = new UnityWebRequest(baseUrl, UnityWebRequest.kHttpVerbPOST);

            // 2. Attach payload if available
            if (!string.IsNullOrEmpty(postPayload))
            {
                // Convert string payload (assuming JSON) to bytes
                byte[] bodyRaw = Encoding.UTF8.GetBytes(postPayload);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                // Set Content-Type header explicitly for raw payload (e.g., JSON)
                webRequest.SetRequestHeader("Content-Type", "application/json");
            }

            // Must manually set the download handler for response data
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // 3. Apply Headers (includes Bearer token)
            ApplyHeaders();

            // Send the request asynchronously
            webRequest.SendWebRequest().completed += operation =>
            {
                Result();
            };
        }
    }
}
