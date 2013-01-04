using System;
using System.Collections.Generic;
using wojilu.weibo.Common;

namespace wojilu.weibo.Core.Sina
{
    /// <summary>
    /// Performs a OAuth token-attached HTTP-Get request to the resource identified by the <c>uri</c> 
    /// with the combination of OAuth parameters and the specified <c>AdditionalParams</c>(optional).        
    /// <remarks>
    /// The signature is created upon the OAuth parameters and the specified <see cref="OAuthHttpGet.Params"/>.
    /// Do not put OAuth parameters in <see cref="OAuthHttpGet.Params"/>, they are automatically included.
    /// There are two ways to pass OAuth: 
    /// 1. Put OAuth authorization info in HTTP header; (This way seems not working when there are additional parameters(in addition to OAuth parameters). All combinations tried.); 
    /// 2. Put OAuth authorization info in query string.
    /// This class uses way #2.
    /// Hint: Cannot do both in the same request.
    /// </remarks>
    /// </summary>
    /// <returns>The server response string(UTF8 decoded).</returns>
    public class OAuthHttpGet : SinaHttpGet
    {
        protected string Token;
        /// <summary>
        /// Initializes a new instance of <see cref="OAuthHttpGet"/> with the specified <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The uri to identify a resource in the remote server.</param>
        public OAuthHttpGet(string uri,string token)
            : base(uri)
        {
            Token = token;
        }

        /// <summary>
        /// Constructs the uri by appending OAuth authorization query string and additional query parameters.
        /// </summary>
        /// <returns></returns>
        protected override string ConstructUri()
        {
            var uri = Uri;

            var oAuthQueryString = OAuthHelper.ConstructQueryString(CollectAllParams());

            uri += "?" + oAuthQueryString;

            return uri;
        }

        /// <summary>
        /// Gets or sets additional parameters (in addition to the OAuth parameters) to be sent in the request.
        /// <remarks>Usually API related parameters.</remarks>
        /// </summary>
        public override ParamCollection Params
        {
            get
            {
                return base.Params;
            }
        }

        /// <summary>
        /// Combines the OAuth basic parameters and additional parameters.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ParamPair> CollectAllParams()
        {
            var parameters = new ParamCollection();

            parameters.Add(SinaConstants.OAuthToken,Token);

            if (null != Params)
            {
                foreach (var item in Params)
                {
                    parameters.Add(item.Name, item.Value);
                }
            }

            return parameters;
        }
    }
}
