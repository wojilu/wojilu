using System;
using System.Text.RegularExpressions;

namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Represents the specific exception in AMicroblogAPI.
    /// </summary>
    /// <remarks>
    ///   If it is a remote error, the <see cref="SinaWeiboException.IsRemoteError" /> is true and <see
    ///    cref="SinaWeiboException.ErrorCode" /> is set.
    /// </remarks>
    /// <example>
    ///   An remote error message is like: "40025:Error: repeated weibo text!". "40025" at the begining is a <see
    ///    cref="SinaWeiboException.ErrorCode" /> .
    /// </example>
    public class SinaWeiboException : SystemException
    {
        private const string ErrorCodePattern = @"^(\d+?):";

        /// <remarks />
        public SinaWeiboException()
        {
        }

        /// <remarks />
        public SinaWeiboException(string message)
            : base(message)
        {
        }

        /// <remarks />
        public SinaWeiboException(int localErrorCode)
            : base(GetLocalErrorMessage(localErrorCode))
        {
            ErrorCode = localErrorCode;
        }

        /// <remarks />
        public SinaWeiboException(int localErrorCode, string message)
            : base(message)
        {
            ErrorCode = localErrorCode;
        }

        /// <remarks />
        public SinaWeiboException(string message, params string[] parameters)
            : this(string.Format(message, parameters))
        {
        }

        /// <remarks />
        public SinaWeiboException(int localErrorCode, string message, params string[] parameters)
            : this(string.Format(message, parameters))
        {
            ErrorCode = localErrorCode;
        }

        /// <remarks />
        public SinaWeiboException(int localErrorCode, Exception innerException)
            : base(GetLocalErrorMessage(localErrorCode), innerException)
        {
            ErrorCode = localErrorCode;
        }

        /// <remarks />
        public SinaWeiboException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <remarks />
        public SinaWeiboException(int localErrorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = localErrorCode;
        }

        /// <summary>
        ///   Gets or sets a boolean value indicating whether the exception is from the remote server.
        /// </summary>
        public bool IsRemoteError { get; set; }

        /// <summary>
        ///   Gets or sets the error code.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        ///   Gets or sets a boolean value indicating whether the exception is handled in previous exception handling process, like Unified Response Error Handling.
        /// </summary>
        public bool IsHandled { get; set; }

        /// <summary>
        ///   Retrieves the error code from the remoteErrorMessage.
        /// </summary>
        /// <param name="remoteErrorMessage"> The error message from remote server. </param>
        /// <returns> The error code if retrieved; otherwise empty. </returns>
        public static string RetrieveErrorCode(string remoteErrorMessage)
        {
            Match match = Regex.Match(remoteErrorMessage, ErrorCodePattern, RegexOptions.IgnoreCase);
            if (match.Success)
                return match.Groups[1].Value;

            return string.Empty;
        }

        /// <summary>
        ///   Retrieves a error message according to th specified error code.
        /// </summary>
        /// <param name="errorCode"> The error code. </param>
        /// <returns> The error message. </returns>
        private static string GetLocalErrorMessage(int errorCode)
        {
            string result = string.Empty;
            switch (errorCode)
            {
                case LocalErrorCode.NetworkUnavailable:
                    result = "Network unavailable.";
                    break;
                case LocalErrorCode.ProcessResponseErrorHandlingFailed:
                    result = "Failed to process Response-Error-Handling.";
                    break;
                default:
                    result = "Unkown issues.";
                    break;
            }

            return result;
        }
    }
}