using System;

namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Represents the event args of ResponseError event.
    /// </summary>
    [Serializable]
    public class ResponseErrorEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of <see cref="ResponseErrorEventArgs" /> with the specified parameters.
        /// </summary>
        /// <param name="requestUri"> The request uri. </param>
        /// <param name="exception"> The exception of <see cref="AMicroblogException" /> type. </param>
        /// <param name="httpMethod"> The request method. </param>
        /// <param name="contentType"> The request content type. </param>
        public ResponseErrorEventArgs(string requestUri, SinaWeiboException exception, string httpMethod,
                                      string contentType)
        {
            ErrorData = new SinaResponseErrorData();
            ErrorData.ErrorCode = exception.ErrorCode;
            ErrorData.RequestUri = requestUri;
            ErrorData.Exception = exception;
            ErrorData.HttpMethod = httpMethod;
            ErrorData.ContentType = contentType;
        }

        /// <summary>
        ///   Gets the contextual data of a HTTP response error.
        /// </summary>
        public SinaResponseErrorData ErrorData { get; private set; }

        /// <summary>
        ///   Gets or sets a boolean value indicating whether the exception is handled.
        /// </summary>
        public bool IsHandled { get; set; }
    }
}