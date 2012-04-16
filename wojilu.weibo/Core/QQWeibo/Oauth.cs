using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace wojilu.weibo.Core.QQWeibo
{
    public class Oauth
    {
        private const string OAuthVersion = "1.0";
        private const string OAuthParameterPrefix = "oauth_";
        private const string OAuthConsumerKeyKey = "oauth_consumer_key";
        private const string OAuthCallbackKey = "oauth_callback";
        private const string OAuthVersionKey = "oauth_version";
        private const string OAuthSignatureMethodKey = "oauth_signature_method";
        private const string OAuthSignatureKey = "oauth_signature";
        private const string OAuthTimestampKey = "oauth_timestamp";
        private const string OAuthNonceKey = "oauth_nonce";
        private const string OAuthTokenKey = "oauth_token";
        private const string oAauthVerifier = "oauth_verifier";
        private const string OAuthTokenSecretKey = "oauth_token_secret";
        private const string HMACSHA1SignatureType = "HMAC-SHA1";
        private Random random = new Random();
        private string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        public string GetOauthUrl(string url, string httpMethod, string customKey, string customSecrect, string tokenKey,
            string tokenSecrect, string verify, string callbackUrl, List<Parameter> parameters, out string queryString)
        {
            string parameterString = NormalizeRequestParameters(parameters);

            string urlWithParameter = url;
            if (!string.IsNullOrEmpty(parameterString))
            {
                urlWithParameter += "?" + parameterString;
            }

            Uri uri = new Uri(urlWithParameter);
            string nonce = GenerateNonce();
            string timeStamp = GenerateTimeStamp();

            parameters.Add(new Parameter(OAuthVersionKey, OAuthVersion));
            parameters.Add(new Parameter(OAuthNonceKey, nonce));
            parameters.Add(new Parameter(OAuthTimestampKey, timeStamp));
            parameters.Add(new Parameter(OAuthSignatureMethodKey, HMACSHA1SignatureType));
            parameters.Add(new Parameter(OAuthConsumerKeyKey, customKey));

            if (!string.IsNullOrEmpty(tokenKey))
            {
                parameters.Add(new Parameter(OAuthTokenKey, tokenKey));
            }

            if (!string.IsNullOrEmpty(verify))
            {
                parameters.Add(new Parameter(oAauthVerifier, verify));
            }

            if (!string.IsNullOrEmpty(callbackUrl))
            {
                parameters.Add(new Parameter(OAuthCallbackKey, callbackUrl));
            }

            string normalizedUrl = null;
            string signature = GenerateSignature(uri, customSecrect, tokenSecrect, httpMethod, parameters,
                out normalizedUrl, out queryString);

            queryString += "&oauth_signature=" + UrlEncode(signature);

            return normalizedUrl;
        }

        /// <summary>
        /// Generates a signature using the HMAC-SHA1 algorithm
        /// </summary>		
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer seceret</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <returns>A base64 string of the hash value</returns>
        private string GenerateSignature(Uri url, string consumerSecret, string tokenSecret, string httpMethod, List<Parameter> parameters, out string normalizedUrl, out string normalizedRequestParameters)
        {
            string signatureBase = GenerateSignatureBase(url, httpMethod, parameters, out normalizedUrl, out normalizedRequestParameters);

            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", UrlEncode(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? "" : UrlEncode(tokenSecret)));

            return GenerateSignatureUsingHash(signatureBase, hmacsha1);
        }

        /// <summary>
        /// Generate the signature base that is used to produce the signature
        /// </summary>
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>        
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="signatureType">The signature type. To use the default values use <see cref="OAuthBase.SignatureTypes">OAuthBase.SignatureTypes</see>.</param>
        /// <returns>The signature base</returns>
        private string GenerateSignatureBase(Uri url, string httpMethod, List<Parameter> parameters, out string normalizedUrl, out string normalizedRequestParameters)
        {
            normalizedUrl = null;
            normalizedRequestParameters = null;

            parameters.Sort(new ParameterComparer());

            normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }
            normalizedUrl += url.AbsolutePath;
     
            normalizedRequestParameters = FormEncodeParameters(parameters);
          
            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{0}&", httpMethod.ToUpper());
            signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl));
            signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));

            

            return signatureBase.ToString();
        }

        private string FormEncodeParameters(List<Parameter> parameters)
        {
            List<Parameter> encodeParams = new List<Parameter>();
            foreach (Parameter a in parameters)
            {
                encodeParams.Add(new Parameter(a.Name, UrlEncode(a.Value)));
            }

            return NormalizeRequestParameters(encodeParams);
        }

        /// <summary>
        /// Generate the signature value based on the given signature base and hash algorithm
        /// </summary>
        /// <param name="signatureBase">The signature based as produced by the GenerateSignatureBase method or by any other means</param>
        /// <param name="hash">The hash algorithm used to perform the hashing. If the hashing algorithm requires initialization or a key it should be set prior to calling this method</param>
        /// <returns>A base64 string of the hash value</returns>
        private string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
        {
            return ComputeHash(hash, signatureBase);
        }

        /// <summary>
        /// Helper function to compute a hash value
        /// </summary>
        /// <param name="hashAlgorithm">The hashing algoirhtm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param>
        /// <param name="data">The data to hash</param>
        /// <returns>a Base64 string of the hash value</returns>
        private string ComputeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        private string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(value);

            foreach (byte symbol in byStr)
            {
                if (unreservedChars.IndexOf((char)symbol) != -1)
                {
                    result.Append((char)symbol);
                }
                else
                {
                    result.Append('%' +  Convert.ToString((char)symbol, 16).ToUpper());
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Normalizes the request parameters according to the spec
        /// </summary>
        /// <param name="parameters">The list of parameters already sorted</param>
        /// <returns>a string representing the normalized parameters</returns>
        private string NormalizeRequestParameters(List<Parameter> parameters)
        {
            StringBuilder sb = new StringBuilder();
            Parameter p = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                p = parameters[i];
                sb.AppendFormat("{0}={1}", p.Name, p.Value);

                if (i < parameters.Count - 1)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generate the timestamp for the signature        
        /// </summary>
        /// <returns></returns>
        private string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns></returns>
        private string GenerateNonce()
        {
            // Just a simple implementation of a random number between 123400 and 9999999
            return random.Next(123400, 9999999).ToString();
        }
    }
}

