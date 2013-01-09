using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Collections.Specialized;
using System.Web;
using System.Threading;


namespace wojilu.weibo.Core.QQWeibo
{
    public delegate void AsyncHttpCallback(AsyncHttp http, string content);

    class RequestState
    {
        public const int BUFFER_SIZE = 1024;
        public byte[] BufferRead;
        public StringBuilder requestData;

        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream streamResponse;

        public AsyncHttpCallback cb;

        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            streamResponse = null;
            cb = null;
        }
    }



    public class AsyncHttp
    {
        public ManualResetEvent allDone = new ManualResetEvent(false);
        const int DefaultTimeout = 60 * 1000; // 1 minutes timeout

        //异步方式发起http get请求
        public bool HttpGet(string url, string queryString, AsyncHttpCallback callback)
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                url += "?" + queryString;
            }

            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;

            try
            {
                RequestState state = new RequestState();
                state.cb = callback;
                state.request = webRequest;

                IAsyncResult result = (IAsyncResult)webRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), state);

                // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), webRequest, DefaultTimeout, true);

                // The response came in the allowed time. The work processing will happen in the callback function.
                allDone.WaitOne();

                // Release the HttpWebResponse resource.
                state.response.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        //异步方式发起http post请求
        public bool HttpPost(string url, string queryString, AsyncHttpCallback callback)
        {
            StreamWriter requestWriter = null;
            StreamReader responseReader = null;

            string responseData = null;

            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ServicePoint.Expect100Continue = false;

            try
            {
                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                requestWriter.Write(queryString);
                requestWriter.Close();
                requestWriter = null;

                RequestState state = new RequestState();
                state.cb = callback;
                state.request = webRequest;

                IAsyncResult result = (IAsyncResult)webRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), state);

                // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), webRequest, DefaultTimeout, true);

                // The response came in the allowed time. The work processing will happen in the 
                // callback function.
                allDone.WaitOne();

                // Release the HttpWebResponse resource.
                state.response.Close();
            }
            catch
            {
                return false;
            }
            finally
            {
                if (requestWriter != null)
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }

            return true;
        }

        //异步方式发起http post请求，可以同时上传文件
        public bool HttpPostWithFile(string url, string queryString, List<Parameter> files, AsyncHttpCallback callback)
        {
            Stream requestStream = null;
            string boundary = DateTime.Now.Ticks.ToString("x");
            
            url += '?' + queryString; 
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ServicePoint.Expect100Continue = true;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.Method = "POST";
            webRequest.KeepAlive = true;
            webRequest.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                Stream memStream = new MemoryStream();

                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

                List<Parameter> listParams = HttpUtil.GetQueryParameters(queryString);

                foreach (Parameter param in listParams)
                {
                    //修复发中文 check_sign error
                    if (param.Name == "content")
                    {
                        string formitem = string.Format(formdataTemplate, param.Name, Uri.UnescapeDataString(param.Value));
                        byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                        memStream.Write(formitembytes, 0, formitembytes.Length);
                    }
                    else
                    {
                        string formitem = string.Format(formdataTemplate, param.Name, param.Value);
                        byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                        memStream.Write(formitembytes, 0, formitembytes.Length);
                    }
                }

                memStream.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: \"{2}\"\r\n\r\n";

                foreach (Parameter param in files)
                {
                    string name = param.Name;
                    string filePath = param.Value;
                    string file = Path.GetFileName(filePath);
                    string contentType = HttpUtil.GetContentType(file);

                    string header = string.Format(headerTemplate, name, file, contentType);
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                    memStream.Write(headerbytes, 0, headerbytes.Length);

                    FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;

                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        memStream.Write(buffer, 0, bytesRead);
                    }

                    memStream.Write(boundarybytes, 0, boundarybytes.Length);
                    fileStream.Close();
                }

                webRequest.ContentLength = memStream.Length;

                requestStream = webRequest.GetRequestStream();

                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();
                requestStream = null;

                RequestState state = new RequestState();
                state.cb = callback;
                state.request = webRequest;

                IAsyncResult result = (IAsyncResult)webRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), state);
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback),
                    webRequest, DefaultTimeout, true);
                allDone.WaitOne();

                state.response.Close();
            }
            catch
            {
                return false;
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                    requestStream = null;
                }
            }

            return true;
        }

        private string FormParamDecode(string value)//转换%XX
        {
            int nCount = 0;
            for(int i = 0; i  < value.Length; i++)//计算数组大小
            {
                if(value[i] == '%')
                {
                    i += 2;
                }
                nCount++;
            }

            byte[] sb = new byte[ nCount ];

            for (int i = 0 ,index = 0 ; i < value.Length; i++ )
            {
                if (value[i] != '%')
                {
                    sb.SetValue((byte)value[i],index++);
                }
                else
                {
                    StringBuilder sChar = new StringBuilder();
                    sChar.Append(value[i + 1]);
                    sChar.Append(value[i + 2]);
                    sb.SetValue(Convert.ToByte(sChar.ToString(), 16),index++);
                    i += 2;
                }
            }
            UTF8Encoding utf8 = new UTF8Encoding();
            return utf8.GetString(sb);
        }


        private void ResponseCallback(IAsyncResult asynchronousResult)
        {
            // State of request is asynchronous.
            RequestState state = (RequestState)asynchronousResult.AsyncState;

            try
            {
                HttpWebRequest webRequest = state.request;
                state.response = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = state.response.GetResponseStream();
                state.streamResponse = responseStream;

                IAsyncResult asynchronousInputRead = responseStream.BeginRead(state.BufferRead, 0,
                    RequestState.BUFFER_SIZE, new AsyncCallback(ReadCallBack), state);

                return;
            }
            catch
            {
                //fire back
                FireCallback(state);
            }

            allDone.Set();
        }

        private void ReadCallBack(IAsyncResult asyncResult)
        {
            RequestState state = (RequestState)asyncResult.AsyncState;
            Stream responseStream = state.streamResponse;

            try
            {
                int read = responseStream.EndRead(asyncResult);

                if (read > 0)
                {
                    state.requestData.Append(Encoding.UTF8.GetString(state.BufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(state.BufferRead, 0, 
                        RequestState.BUFFER_SIZE, new AsyncCallback(ReadCallBack), state);

                    return;
                }
                else
                {
                    //fire back
                    FireCallback(state);
                    responseStream.Close();
                }
            }
            catch
            {
                //fire back
                FireCallback(state);
                responseStream.Close();
            }

            allDone.Set();
        }

        // Abort the request if the timer fires.
        private void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }

        private void FireCallback(RequestState state)
        {
            //call back
            if (state.cb != null)
            {
                state.cb(this, state.requestData.ToString());
            }
        }
    }
}
