using System;
using System.Text;
using System.Net;
using System.IO;

namespace InvokeDLL
{
    public class InvokeWebAPI
    {
        // POST
        public string CreatePostHttpResponse(string url, string json)
        {
            // 初始化HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }

            // 填充httpWebRequest的基本信息
            request.UserAgent = ".NET Framework Test Client";
            request.ContentType = "application/json;charset=UTF-8"; // application/json
            request.Method = "POST";

            // 填充要post的内容
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            request.ContentLength = bytes.Length;
            Stream requestStream;
            try
            {
                requestStream = request.GetRequestStream();
            }
            catch (Exception e)
            {
                // log error
                Console.WriteLine(string.Format("POST操作发生异常：{0}", e.Message));
                throw e;
            }
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            // 发送post请求到服务器并读取服务器返回信息
            Stream responseStream;
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            {
                // log error
                Console.WriteLine(string.Format("POST操作发生异常：{0}", e.Message));
                throw e;
            }

            // 读取服务器返回信息
            string stringResponse = string.Empty;
            Encoding encoding = Encoding.UTF8;
            using (StreamReader responseReader = new StreamReader(responseStream, encoding))
            {
                stringResponse = responseReader.ReadToEnd();
            }
            responseStream.Close();

            // 返回值
            return stringResponse;
        }
    }
}
