using wojilu.weibo.Common;

namespace wojilu.weibo.common
{
    /// <summary>
    ///   Defines methods to handle HTTP response error.
    /// </summary>
    public interface IResponseErrorHandler
    {
        /// <summary>
        ///   Handles the response error with the <paramref name="errorData" /> .
        /// </summary>
        /// <param name="errorData"> The contextual data of the response error. </param>
        void Handle(SinaResponseErrorData errorData);
    }
}