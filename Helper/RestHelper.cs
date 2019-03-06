using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Com.Utility.Collection
{
    public class RestHelper : IDisposable
    {
        #region Dispose

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion Dispose

        #region Public Property

        public HttpWebRequest HttpWebRequest { get; set; }
        public HttpWebResponse HttpWebResponse { get; set; }
        public int RequestTimeout { get; set; }

        #endregion Public Property

        #region Public Methods

        public T Delete<T>(ServiceType serviceType, string clientUrl, string resourceUrl)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.DELETE, clientUrl, resourceUrl, null, false, null, null);
        }

        public T Delete<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.DELETE, clientUrl, resourceUrl, headers, false, null, null);
        }

        public T Delete<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Dictionary<string, string> parameterList)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.DELETE, clientUrl, resourceUrl, headers, false, parameterList, null);
        }

        public T Delete<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Object parameterObj)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.DELETE, clientUrl, resourceUrl, headers, true, null, parameterObj);
        }

        public async Task<T> DeleteAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl)
        {
            return await Task.Run<T>(() =>
            {
                return Delete<T>(serviceType, clientUrl, resourceUrl);
            });
        }

        public async Task<T> DeleteAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers)
        {
            return await Task.Run<T>(() =>
            {
                return Delete<T>(serviceType, clientUrl, resourceUrl, headers);
            });
        }

        public async Task<T> DeleteAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Dictionary<string, string> parameterList)
        {
            return await Task.Run<T>(() =>
            {
                return Delete<T>(serviceType, clientUrl, resourceUrl, headers, parameterList);
            });
        }

        public async Task<T> DeleteAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Object parameterObj)
        {
            return await Task.Run<T>(() =>
            {
                return Delete<T>(serviceType, clientUrl, resourceUrl, headers, parameterObj);
            });
        }

        public T Get<T>(ServiceType serviceType, string clientUrl, string resourceUrl)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.GET, clientUrl, resourceUrl, null, false, null, null);
        }

        public T Get<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.GET, clientUrl, resourceUrl, headers, false, null, null);
        }

        public T Get<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Dictionary<string, string> parametersList)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.GET, clientUrl, resourceUrl, headers, false, parametersList, null);
        }

        public T Get<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, string parameter)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.GET, clientUrl, resourceUrl, headers, false, null, parameter);
        }

        public async Task<T> GetAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl)
        {
            return await Task.Run<T>(() =>
            {
                return Get<T>(serviceType, clientUrl, resourceUrl);
            });
        }

        public async Task<T> GetAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers)
        {
            return await Task.Run<T>(() =>
            {
                return Get<T>(serviceType, clientUrl, resourceUrl, headers);
            });
        }

        public async Task<T> GetAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Dictionary<string, string> parametersList)
        {
            return await Task.Run<T>(() =>
            {
                return Get<T>(serviceType, clientUrl, resourceUrl, headers, parametersList);
            });
        }

        public async Task<T> GetAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, string parameter)
        {
            return await Task.Run<T>(() =>
            {
                return Get<T>(serviceType, clientUrl, resourceUrl, headers, parameter);
            });
        }

        public T Post<T>(ServiceType serviceType, string clientUrl, string resourceUrl)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.POST, clientUrl, resourceUrl, null, false, null, null);
        }

        public T Post<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.POST, clientUrl, resourceUrl, headers, false, null, null);
        }

        public T Post<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Dictionary<string, string> parameterList)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.POST, clientUrl, resourceUrl, headers, false, parameterList, null);
        }

        public T Post<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Object parameterObj)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.POST, clientUrl, resourceUrl, headers, true, null, parameterObj);
        }

        public async Task<T> PostAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl)
        {
            return await Task.Run<T>(() =>
            {
                return Post<T>(serviceType, clientUrl, resourceUrl);
            });
        }

        public async Task<T> PostAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers)
        {
            return await Task.Run<T>(() =>
            {
                return Post<T>(serviceType, clientUrl, resourceUrl, headers);
            });
        }

        public async Task<T> PostAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Dictionary<string, string> parameterList)
        {
            return await Task.Run<T>(() =>
            {
                return Post<T>(serviceType, clientUrl, resourceUrl, headers, parameterList);
            });
        }

        public async Task<T> PostAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Object parameterObj)
        {
            return await Task.Run<T>(() =>
            {
                return Post<T>(serviceType, clientUrl, resourceUrl, headers, parameterObj);
            });
        }

        public T Put<T>(ServiceType serviceType, string clientUrl, string resourceUrl)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.PUT, clientUrl, resourceUrl, null, false, null, null);
        }

        public T Put<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.PUT, clientUrl, resourceUrl, headers, false, null, null);
        }

        public T Put<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Dictionary<string, string> parameterList)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.PUT, clientUrl, resourceUrl, headers, false, parameterList, null);
        }

        public T Put<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Object parameterObj)
        {
            return FetchRESTResponse<T>(serviceType, RestMethod.PUT, clientUrl, resourceUrl, headers, true, null, parameterObj);
        }

        public async Task<T> PutAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl)
        {
            return await Task.Run<T>(() =>
            {
                return Put<T>(serviceType, clientUrl, resourceUrl);
            });
        }

        public async Task<T> PutAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers)
        {
            return await Task.Run<T>(() =>
            {
                return Put<T>(serviceType, clientUrl, resourceUrl, headers);
            });
        }

        public async Task<T> PutAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Dictionary<string, string> parameterList)
        {
            return await Task.Run<T>(() =>
            {
                return Put<T>(serviceType, clientUrl, resourceUrl, headers, parameterList);
            });
        }

        public async Task<T> PutAsync<T>(ServiceType serviceType, string clientUrl, string resourceUrl, Dictionary<string, string> headers, Object parameterObj)
        {
            return await Task.Run<T>(() =>
            {
                return Put<T>(serviceType, clientUrl, resourceUrl, headers, parameterObj);
            });
        }

        #endregion Public Methods

        #region Private Methods

        private T FetchRESTResponse<T>(ServiceType serviceType, RestMethod methodType,
           string clientUrl, string resourceUrl, Dictionary<string, string> headerList, bool addDirectToBody,
           Dictionary<string, string> parametersList, Object parametersObj)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string jStr = string.Empty;
            try
            {
                string postData = string.Empty;

                if (parametersList != null)
                {
                    StringBuilder updatedParameterList = new StringBuilder();
                    foreach (var key in parametersList.Keys)
                    {
                        if (!string.IsNullOrEmpty(updatedParameterList.ToString()))
                            updatedParameterList.Append("&");

                        updatedParameterList.Append(string.Format("{0}={1}", key, parametersList[key]));
                    }

                    if (!string.IsNullOrEmpty(updatedParameterList.ToString()))
                        postData = updatedParameterList.ToString();

                    postData = (!string.IsNullOrEmpty(postData) ? string.Format("{0}{1}", "?", postData) : string.Empty);
                }
                else if (parametersObj != null)
                {
                    if (serviceType != ServiceType.XML)
                        postData = Newtonsoft.Json.JsonConvert.SerializeObject(parametersObj);
                    else
                        postData = SafeTypeHelper.SafeString(parametersObj);
                }

                if (serviceType != ServiceType.DEFAULT_OAUTH)
                {
                    if (serviceType == ServiceType.SOA || methodType == RestMethod.GET || (methodType == RestMethod.POST && parametersList != null) ||
                        (methodType == RestMethod.GET && parametersObj != null))
                    {
                        if (methodType == RestMethod.GET && parametersObj != null)
                            HttpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(string.Format("{0}{1}/{2}/", clientUrl, resourceUrl, SafeTypeHelper.SafeString(parametersObj))));
                        else
                            HttpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(string.Format("{0}{1}{2}", clientUrl, resourceUrl, postData)));
                    }
                    else
                        HttpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(string.Format("{0}{1}", clientUrl, resourceUrl)));
                }
                else
                    HttpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(string.Format("{0}{1}", clientUrl, resourceUrl)));

                HttpWebRequest.ContentType = "application/json";
                if (serviceType == ServiceType.DEFAULT_OAUTH)
                    HttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                if (serviceType == ServiceType.XML)
                {
                    HttpWebRequest.ContentType = "application/xml";
                    HttpWebRequest.Accept = "application/xml";
                }

                if (RequestTimeout > 0)
                    HttpWebRequest.Timeout = RequestTimeout;
                if (headerList != null)
                    foreach (var key in headerList.Keys)
                        HttpWebRequest.Headers.Add(key, headerList[key]);

                if (serviceType == ServiceType.SOA)
                    HttpWebRequest.Method = RestMethod.POST.ToString();
                else
                {
                    HttpWebRequest.Method = methodType.ToString();
                    if (methodType != RestMethod.GET)
                    {
                        if (parametersList == null && !string.IsNullOrEmpty(postData) && serviceType != ServiceType.XML)
                        {
                            byte[] postArray = System.Text.Encoding.UTF8.GetBytes(postData);
                            HttpWebRequest.ContentLength = postArray.Length;
                            using (Stream reqStream = HttpWebRequest.GetRequestStream())
                            {
                                reqStream.Write(postArray, 0, postArray.Length);
                                reqStream.Close();
                            }
                        }
                        else
                        {
                            if ((serviceType == ServiceType.DEFAULT_OAUTH ||
                                 serviceType == ServiceType.XML) && !string.IsNullOrEmpty(postData))
                            {
                                postData = postData.TrimStart('?');

                                byte[] postArray = System.Text.Encoding.UTF8.GetBytes(postData);
                                if (serviceType == ServiceType.XML)
                                    postArray = System.Text.Encoding.ASCII.GetBytes(postData);

                                HttpWebRequest.ContentLength = postArray.Length;
                                using (Stream reqStream = HttpWebRequest.GetRequestStream())
                                {
                                    reqStream.Write(postArray, 0, postArray.Length);
                                    reqStream.Close();
                                }
                            }
                            else
                                HttpWebRequest.ContentLength = 0;
                        }
                    }
                }

                //get response
                HttpWebResponse = (HttpWebResponse)HttpWebRequest.GetResponse();
                using (StreamReader sr = new StreamReader(GetStreamForResponse(HttpWebResponse)))
                    jStr = sr.ReadToEnd();

                Type typeParameterType = typeof(T);
                if (!typeParameterType.Equals(typeof(string)))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jStr);
                else
                    return (T)(object)jStr;
            }
            catch (WebException ex)
            {
                string errorMessage = string.Empty;
                if (ex.Response != null)
                {
                    HttpWebResponse = (HttpWebResponse)ex.Response;
                    using (var stream = GetStreamForResponse(ex.Response))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            errorMessage = reader.ReadToEnd();
                        }
                    }
                }

                ClientError restException = new ClientError()
                {
                    StatusCode = HttpWebResponse != null ? HttpWebResponse.StatusCode : HttpStatusCode.BadRequest,
                    InnerMessage = ex.Message,
                    Content = errorMessage
                };
                throw new Exception(Newtonsoft.Json.JsonConvert.SerializeObject(restException));
            }
            catch (Exception ex)
            {
                ClientError restException = new ClientError()
                {
                    StatusCode = HttpWebResponse != null ? HttpWebResponse.StatusCode : HttpStatusCode.BadRequest,
                    InnerMessage = ex.Message,
                    Content = jStr
                };
                throw new Exception(Newtonsoft.Json.JsonConvert.SerializeObject(restException));
            }
        }

        private Stream GetStreamForResponse(WebResponse webResponse)
        {
            var response = (HttpWebResponse)webResponse;

            Stream stream;
            switch (response.ContentEncoding.ToUpperInvariant())
            {
                case "GZIP":
                    stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    break;

                case "DEFLATE":
                    stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress);
                    break;

                default:
                    stream = response.GetResponseStream();
                    break;
            }

            return stream;
        }

        #endregion Private Methods

        #region Enums

        public enum RestMethod
        {
            GET = 0,
            POST = 1,
            PUT = 2,
            DELETE = 3,
            HEAD = 4,
            OPTIONS = 5,
            PATCH = 6,
            MERGE = 7,
        }

        public enum ServiceType
        {
            DEFAULT = 1,
            SOA = 2,
            DEFAULT_OAUTH = 3,
            XML = 4
        }

        #endregion Enums

        #region RestClient ErrorObject

        public class ClientError
        {
            public string Content { get; set; }
            public string InnerMessage { get; set; }
            public HttpStatusCode StatusCode { get; set; }
        }

        #endregion RestClient ErrorObject
    }
}