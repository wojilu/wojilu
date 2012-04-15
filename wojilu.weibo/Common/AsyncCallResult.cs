using System;

namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Represents the aync call result with the specified type of response data.
    /// </summary>
    /// <typeparam name="T"> The type of the result data. </typeparam>
    [Serializable]
    public class AsyncCallResult<T> : AsyncCallResult
    {
        /// <summary>
        ///   The return result of the call.
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    ///   Represents the aync call result.
    /// </summary>
    [Serializable]
    public class AsyncCallResult
    {
        /// <summary>
        ///   Gets or sets a value indicates whether the call succeed.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        ///   Gets or sets the exception object if the call failed.
        /// </summary>
        public Exception Exception { get; set; }
    }
}