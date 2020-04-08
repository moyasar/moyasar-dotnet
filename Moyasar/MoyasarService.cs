﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Moyasar.Providers;

namespace Moyasar
{
    public static class MoyasarService
    {
        public const string ApiBaseUrl = "https://api.moyasar.com";
        public const string ApiVersion = "v1";
        public static string CurrentVersionUrl => $"{ApiBaseUrl}/{ApiVersion}";
        
        /// <summary>
        /// Moyasar's API key that is used to authenticate all outbound requests
        /// </summary>
        public static string ApiKey { get; set; }
        public static ISerializer Serializer { get; set; }
        public static Func<string, HttpWebRequest> HttpWebRequestFactory { get; set; }

        static MoyasarService()
        {
            Serializer = new JsonSerializer();
            HttpWebRequestFactory = CreateHttpWebRequest;
        }

        public static HttpWebRequest CreateHttpWebRequest(string url)
        {
            // Use Create instead of CreateHttp for compatibility with .Net Framework 4.0
            return WebRequest.Create(url) as HttpWebRequest;
        }

        /// <summary>
        /// Creates and send an HTTP request to the specified URL
        /// </summary>
        /// <param name="httpMethod">A valid HTTP method</param>
        /// <param name="url">Target URL</param>
        /// <param name="parameters">Optional request data</param>
        /// <returns>Response string</returns>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public static string SendRequest(string httpMethod, string url, Dictionary<string, object> parameters)
        {
            httpMethod = httpMethod.ToUpper();

            if (httpMethod == "GET")
            {
                url = AppendUrlParameters(url, parameters);
            }

            ConfigureTls();
            var webRequest = HttpWebRequestFactory(url);
            webRequest.Method = httpMethod;
            webRequest.Credentials = new NetworkCredential(ApiKey, "");
            
            if(httpMethod != "GET" && parameters != null)
            {
                webRequest.ContentType = "application/json";
                using (var sw = new StreamWriter(webRequest.GetRequestStream()))
                {
                    sw.Write(Serializer.Serialize(parameters));
                    sw.Flush();
                }
            }

            try
            {
                string result = null;
                HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }

                return result;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    string result = null;
                    var response = ex.Response as HttpWebResponse;
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                    }
                    
                    if ((int) response.StatusCode == 429)
                    {
                        throw new TooManyRequestsException("Too Many Requests")
                        {
                            HttpStatusCode = (int)response.StatusCode,
                            ResponsePayload = result
                        };
                    }
                    
                    if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 600)
                    {
                        dynamic resObj = Serializer.Deserialize<object>(result);

                        var msg = "";
                        try { msg = resObj.message; } catch {}
                        var exception = new ApiException(msg)
                        {
                            HttpStatusCode = (int)response.StatusCode,
                            ResponsePayload = result,
                        };

                        try { exception.Type = resObj.type.ToString(); } catch {}
                        try { exception.Errors = resObj.errors.ToString(); } catch {}

                        try
                        {
                            exception.ErrorsDictionary = 
                                Serializer.Deserialize<Dictionary<string, List<string>>>(resObj.errors.ToString());
                        } catch {}

                        throw exception;
                    }
                }
                
                throw new NetworkException("Could not connect to Moyasar service", ex);
            }
        }

        private static void ConfigureTls()
        {
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072; // TLS 1.2
        }

        public static string AppendUrlParameters(string url, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                StringBuilder p = new StringBuilder();
                foreach (var parameter in parameters)
                {
                    p.Append($"&{parameter.Key.ToLower()}={parameter.Value}");
                }

                if (p.Length > 0)
                {
                    return $"{url}?{p.ToString().Substring(1)}";
                }
            }

            return url;
        }
    }
}