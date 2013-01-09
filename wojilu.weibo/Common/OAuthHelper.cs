using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Provides helper methods for OAuth purpose.
    /// </summary>
    public static class OAuthHelper
    {
     
        public static string ConstructOAuthHeader(string oauthHeaderPrefix, string accessToken)
        {
            var headerBuilder = new StringBuilder();
            headerBuilder.Append(oauthHeaderPrefix);
            headerBuilder.Append(accessToken);

            return headerBuilder.ToString();
        }

        /// <summary>
        ///   Constructs the query string.
        /// </summary>
        /// <param name="parameters"> The parameters to used to construct OAuth authorization header(including OAuth protocol params and request-specific params). </param>
        /// <returns> The query string. </returns>
        public static string ConstructQueryString(IEnumerable<ParamPair> parameters)
        {
            var queryStringBuilder = new StringBuilder();

            foreach (ParamPair item in parameters)
            {
               
                string name =RFC3986Encoder.Encode( item.Name);
                string val =  RFC3986Encoder.Encode(item.Value);
                queryStringBuilder.Append(string.Format("{0}={1}&", name, val));
            }

            return queryStringBuilder.ToString();
        }


        /// <summary>
        ///   Prepares a post body string for an access-token-required request.
        /// </summary>
        /// <param name="uri"> The uri to identify the resource. </param>
        /// <param name="customPostParams"> Additional parameters (in addition to the OAuth parameters) to be included in the post body. </param>
        /// <returns> The url-encoded post body string. </returns>
        public static string PreparePostBody(IEnumerable<ParamPair> customPostParams)
        {
            var parameters = new ParamCollection();
            //OAuthAccessToken accessToken = Environment.AccessToken;

            //parameters.Add(Constants.OAuthToken, accessToken.Token);

            if (null != customPostParams)
            {
                foreach (ParamPair item in customPostParams)
                {
                    parameters.Add(item.Name, item.Value);
                }
            }

            string postBody = ConstructPostBody(parameters);

            return postBody;
        }

        /// <summary>
        ///   Constructs the OAuth string for the Http-Post request's body.
        /// </summary>
        /// <param name="parameters"> The parameters to used to construct OAuth authorization header(including OAuth protocol params and request-specific params). </param>
        /// <param name="uri"> The uri to identify the resource. </param>
        /// <param name="accessTokenSecret"> The access token secret. </param>
        /// <returns> The OAuth string for a HTTP-Post body. </returns>
        private static string ConstructPostBody(IEnumerable<ParamPair> parameters)
        {
            var bodyBuilder = new StringBuilder();
            foreach (ParamPair item in parameters)
            { //oauth 2.0不需要encode
                //string name = RFC3986Encoder.Encode(item.Name);
                string name =item.Name;
                string val = item.Value;
                bodyBuilder.Append(string.Format("{0}={1}&", name, val));
            }

            string result = bodyBuilder.ToString().TrimEnd('&');

            return result;
        }
    }
}