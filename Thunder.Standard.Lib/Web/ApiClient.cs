using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thunder.Standard.Lib.Extension;
using Thunder.Standard.Lib.Model;

namespace Thunder.Standard.Lib.Web
{
    /// <summary>
    /// WebApi 客户端
    /// </summary>
    public abstract class ApiClient
    {
        protected abstract string ToJson<T>(T obj);
        protected abstract T FromJson<T>(string src);
        public Action<string> LogSave { get; set; }
        public string ConnectId { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public HttpClient HttpClient { get; set; }

        /// <summary>
        /// POST 接收LargeObject封装数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="param"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<T> PostX<T>(string action, object param, Action<int> onStatusCode = null)
        {
            var url = $"{action}";
            var r = await CreateResponse<LargeObject>(new RequestData(url, param, onStatusCode));
            var result = default(T);
            if (r != null) result = r.LoadFromLargeObject<T>();
            return result;
        }

        /// <summary>
        /// GET 接收LargeObject封装数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<T> GetX<T>(string action, Action<int> onStatusCode = null)
        {
            var url = $"{action}";
            var r = await CreateResponse<LargeObject>(new RequestData(url, onStatusCode));
            var result = default(T);
            if (r != null) result = r.LoadFromLargeObject<T>();
            return result;
        }

        public async Task<T> Post<T>(string action, object param, Action<int> onStatusCode = null, Action<Exception, RequestData> @catch = null)
            => await CreateResponse<T>(new RequestData(action, param, onStatusCode, @catch));

        public async Task<T> Get<T>(string action, object param = null, Action<int> onStatusCode = null, Action<Exception, RequestData> @catch = null)
            => await CreateResponse<T>(new RequestData(action, param, onStatusCode, @catch) { Method = HttpMethod.Get });

        public async Task<T> Delete<T>(string action, object param = null, Action<int> onStatusCode = null, Action<Exception, RequestData> @catch = null)
            => await CreateResponse<T>(new RequestData(action, param, onStatusCode, @catch) { Method = HttpMethod.Delete });

        public async Task<T> Put<T>(string action, object param, Action<int> onStatusCode = null, Action<Exception, RequestData> @catch = null)
            => await CreateResponse<T>(new RequestData(action, param, onStatusCode, @catch) { Method = HttpMethod.Put });

        public async Task<T> Patch<T>(string action, object param, Action<int> onStatusCode = null, Action<Exception, RequestData> @catch = null)
    => await CreateResponse<T>(new RequestData(action, param, onStatusCode, @catch) { Method = HttpMethod.Patch });

        /// <summary>
        /// 开始请求前执行
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns>用户数据</returns>
        public virtual object OnBeforeRequest(HttpRequestMessage requestMessage) { return null; }
        /// <summary>
        /// 接收数据后执行
        /// </summary>
        /// <param name="responseBody"></param>
        /// <param name="userData">用户数据</param>
        public virtual void OnEndResponse(string responseBody, object userData) { }

        public async Task<T> CreateResponse<T>(RequestData data)
        {
            var headers = data.Headers;
            var method = data.Method;
            var url = data.Url;
            var content = data.Content;
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    if (Headers.ContainsKey(item.Key))
                    {
                        Headers[item.Key] = item.Value;
                    }
                    else
                    {
                        Headers.Add(item.Key, item.Value);
                    }
                }
            }
            if (!Headers.ContainsKey("ClientId") && string.IsNullOrWhiteSpace(ConnectId))
            {
                Headers.Add("ClientId", ConnectId);
            }
            try
            {
                var client = HttpClient ?? new HttpClient();
                var req = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = method,
                };
                if (content != null)
                {
                    req.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    req.Content = new StringContent(ToJson(content), Encoding.UTF8, "application/json");
                }

                foreach (var item in Headers)
                {
                    req.Headers.Add(item.Key, item.Value);
                }

                var userData = OnBeforeRequest(req);

                var rsp = await client.SendAsync(req);
                data.OnStatusCode?.Invoke((int)rsp.StatusCode);
                var rspbody = rsp.Content.ReadAsStringAsync().Result;

                OnEndResponse(rspbody, userData);
                //if ((int)rsp.StatusCode>=400)
                //{
                //    return default(T);
                //}

                var result = FromJson<T>(rspbody);
                return result;
            }
            catch (Exception ex)
            {
                data.OnStatusCode?.Invoke(999);
                data.Catch?.Invoke(ex, data);
                Log($"{method.ToString()} {url} {ex.Message}");
            }
            return default(T);
        }

        /// <summary>
        /// 添加或者更新Headers
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddHeaders(string key, string value)
        {
            if (Headers.ContainsKey(key))
            {
                Headers[key] = value;
            }
            else
            {
                Headers.Add(key, value);
            }
        }

        /// <summary>
        /// 移除Headers
        /// </summary>
        /// <param name="key"></param>
        public void RemoveHeaders(string key)
        {
            if (Headers.ContainsKey(key))
            {
                Headers.Remove(key);
            }
        }

        public string ActionUrl(string hostUrl, string action, object getObject = null)
        {
            var r = $"{hostUrl}/{action}";
            if (getObject != null)
            {
                var t = getObject.GetType();
                var ps = t.GetProperties();
                var vs = ps.Select(x => $"{HttpUtility.UrlEncode(x.GetPreportyName())}={HttpUtility.UrlEncode(x.GetValue(getObject).ToString())}");
                r = $"{r}?{string.Join("&", vs)}";
            }
            return r;
        }

        private void Log(string msg)
        {
            LogSave?.Invoke(msg);
        }

    }

    public class RequestData
    {
        public RequestData()
        {
        }

        public RequestData(string url, Action<int> onStatusCode = null, Action<Exception, RequestData> @catch = null)
        {
            Url = url;
            OnStatusCode = onStatusCode;
            Method = HttpMethod.Get;
            Catch = @catch;
        }

        public RequestData(string url, object data, Action<int> onStatusCode = null, Action<Exception, RequestData> @catch = null)
        {
            Url = url;
            Content = data;
            OnStatusCode = onStatusCode;
            Method = HttpMethod.Post;
            Catch = @catch;
        }

        public RequestData(string url, object data, Dictionary<string, string> headers, Action<int> onStatusCode = null, Action<Exception, RequestData> @catch = null)
        {
            Headers = headers;
            Url = url;
            Content = data;
            OnStatusCode = onStatusCode;
            Method = HttpMethod.Post;
            Catch = @catch;
        }

        public RequestData(string url, object data, Dictionary<string, string> headers, HttpMethod method, Action<int> onStatusCode = null, Action<Exception, RequestData> @catch = null)
        {
            Method = method;
            Headers = headers;
            Url = url;
            Content = data;
            OnStatusCode = onStatusCode;
            Catch = @catch;
        }

        public HttpMethod Method { get; set; } = HttpMethod.Get;
        public Dictionary<string, string> Headers { get; set; }
        public string Url { get; set; }
        public object Content { get; set; }
        public Action<int> OnStatusCode { get; set; }
        public Action<Exception, RequestData> Catch { get; set; }
    }

    public class HttpPatch : HttpMethod
    {
        public HttpPatch(string method) : base(method)
        {

        }
    }
}
