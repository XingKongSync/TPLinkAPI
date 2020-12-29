using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace TPLinkAPI
{
    /// <summary>
    /// 封装 HTTP 请求的工具类
    /// </summary>
    public class HttpUtils
    {
        public enum HttpMethodEnum
        {
            GET,
            POST,
            PUT
        }

        private static HttpWebRequest CreateHttpRequest(string url, HttpMethodEnum method, int timeout = 5)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request != null)
            {
                request.Method = method.ToString();
                //request.ContentType = "application/json; charset=UTF-8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36";
                request.Timeout = 3000;

                request.ServicePoint.Expect100Continue = false;
            }
            return request;
        }

        public static string DoRequest(string url, HttpMethodEnum method, string content)
        {
            HttpWebRequest request = CreateHttpRequest(url, method);
            if (request != null)
            {
                if (method != HttpMethodEnum.GET)
                {
                    byte[] datas = Encoding.UTF8.GetBytes(content);
                    request.ContentLength = datas.Length;
                    using (BinaryWriter sw = new BinaryWriter(request.GetRequestStream()))
                    {
                        sw.Write(datas);
                    }
                }
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response != null)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        string data = sr.ReadToEnd();
                        return data;
                    }
                }
            }
            return null;
        }

        public static string Get(string url)
        {
            return DoRequest(url, HttpMethodEnum.GET, null);
        }

        public static string Post(string url, object data)
        {
            return DoRequest(url, HttpMethodEnum.POST, JsonConvert.SerializeObject(data));
        }

        public static string Put(string url, object data)
        {
            return DoRequest(url, HttpMethodEnum.PUT, JsonConvert.SerializeObject(data));
        }
    }

}
