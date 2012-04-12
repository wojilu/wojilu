namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Defines the local error codes.
    /// </summary>
    public static class LocalErrorCode
    {
        /// <remarks />
        public const int UnknowError = 80000000;

        /// <remarks />
        public const int NetworkUnavailable = 80000010;

        /// <remarks />
        public const int ProcessResponseErrorHandlingFailed = 80000020;

        /// <remarks />
        public const int ArgumentNotProvided = 80000040;

        /// <remarks />
        public const int ArgumentInvalid = 80000050;

        /// <remarks />
        public const int CredentialInvalid = 80000060;

        /// <remarks />
        public const int ParseResponseFailed = 80000070;

        /// <remarks />
        public const int AppKeyOrSecretNotProvided = 80000080;

        /// <remarks />
        public static int AccessTokenNotObtained = 80000090;
    }
}