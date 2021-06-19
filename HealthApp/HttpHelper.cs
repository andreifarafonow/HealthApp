using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace HealthApp
{
    public class HttpHelper
    {
        public static string POST(string url, out string cookies, WebHeaderCollection headers, NameValueCollection data)
        {
            byte[] respBytes;

            using (var webClient = new WebClient())
            {
                webClient.Headers = headers;

                respBytes = webClient.UploadValues(url, data);

                cookies = string.Join(";", webClient.ResponseHeaders.GetValues("Set-Cookie").Select(x => x.Split(';')[0]));
            }

            string response = Encoding.GetEncoding(1251).GetString(respBytes);

            return response;
        }

        public static string POST(string url, string json, WebHeaderCollection headers = null)
        {
            if (headers == null)
            {
                headers = new WebHeaderCollection();

                headers.Add(HttpRequestHeader.ContentType, "application/json");
            }

            string response = "";

            using (var webClient = new WebClient())
            {
                // headers.Add("MediaType", "application/json"); 
                webClient.Headers = headers;
                response = webClient.UploadString(url, json);
            }

            return response;
        }

        public static string GET(string url, WebHeaderCollection headers = null)
        {
            string response = "";

            using (var webClient = new WebClient())
            {
                webClient.Headers = headers;

                try
                {
                    response = Encoding.UTF8.GetString(webClient.DownloadData(url));
                }
                catch
                {
                    return "";
                }
            }


            return response;
        }
    }
}