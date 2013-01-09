using wojilu.weibo.Common;

namespace wojilu.weibo.Core
{
    public interface IHttpRequest
    {
        /// <summary>
        ///   Performs the HTTP request.
        /// </summary>
        /// <returns> The HTTP response. </returns>
        string Request();

        /// <summary>
        ///   Performs the HTTP request asynchronously.
        /// </summary>
        void RequestAsync(AsyncCallback<string> callback);
    }
}