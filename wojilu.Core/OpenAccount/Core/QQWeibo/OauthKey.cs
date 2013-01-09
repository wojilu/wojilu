using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.Security.Cryptography;


namespace wojilu.weibo.Core.QQWeibo {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class OauthKey {

        private static readonly ILog logger = LogManager.GetLogger( typeof( OauthKey ) );

        /// <summary>
        /// 
        /// </summary>
        public const string urlRequesToken = "https://open.t.qq.com/cgi-bin/request_token";
        /// <summary>
        /// 
        /// </summary>
        public const string urlUserAuthrize = "https://open.t.qq.com/cgi-bin/authorize";
        /// <summary>
        /// 
        /// </summary>
        public const string urlAccessToken = "https://open.t.qq.com/cgi-bin/access_token";


        public string WeiboName {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the custom key.
        /// </summary>
        /// <value>The custom key.</value>
        /// <remarks></remarks>
        public string consumerKey { get; set; }
        /// <summary>
        /// Gets or sets the custom secret.
        /// </summary>
        /// <value>The custom secret.</value>
        /// <remarks></remarks>
        public string consumerSecret { get; set; }
        /// <summary>
        /// Gets or sets the token key.
        /// </summary>
        /// <value>The token key.</value>
        /// <remarks></remarks>
        public string tokenKey { get; set; }
        /// <summary>
        /// Gets or sets the token secret.
        /// </summary>
        /// <value>The token secret.</value>
        /// <remarks></remarks>
        public string tokenSecret { get; set; }

        /// <summary>
        /// Gets or sets the verify.
        /// </summary>
        /// <value>The verify.</value>
        /// <remarks></remarks>
        public string verify { get; set; }
        /// <summary>
        /// Gets or sets the callback URL.
        /// </summary>
        /// <value>The callback URL.</value>
        /// <remarks></remarks>
        public string callbackUrl { get; set; }


        /// <summary>
        /// Gets or sets the charset.
        /// </summary>
        /// <value>The charset.</value>
        /// <remarks></remarks>
        public Encoding Charset { get; set; }

        public OauthKey()
            : this( null, null ) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OauthKey"/> class.
        /// </summary>
        /// <param name="consumerKey">The pcustomkey.</param>
        /// <param name="consumerSecret">The pcustomsecret.</param>
        /// <remarks></remarks>
        public OauthKey( string consumerKey, string consumerSecret )
            : this( consumerKey, consumerSecret, null, null ) {

        }

        public OauthKey( string consumerKey, string consumerSecret, string accessToken, string accessSecret ) {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.tokenKey = accessToken;
            this.tokenSecret = accessSecret;
            this.Charset = Encoding.UTF8;
            verify = null;
            callbackUrl = null;
        }

        public string GetOAuthUrl() {
            return urlUserAuthrize + "?oauth_token=" + tokenKey;
        }



        /// <summary> 获取request token
        /// Gets the request token.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool GetRequestToken( string callback ) {
            return GetRequestToken( OauthKey.urlRequesToken, callback );
        }

        /// <summary>获取request token 重载
        /// 
        /// </summary>
        /// <param name="requesturl">The requesturl.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool GetRequestToken( string requesturl, string callback ) {
            List<Parameter> parameters = new List<Parameter>();
            if (string.IsNullOrEmpty( callback )) {
                this.callbackUrl = "http://www.qq.com";
            }
            else
                this.callbackUrl = callback;

            QWeiboRequest request = new QWeiboRequest();
            return ParseToken( request.SyncRequest( urlRequesToken, "GET", this, parameters, null ) );

        }

        /// <summary> 获取access token
        /// Gets the access token.
        /// </summary>
        /// <param name="verifier">The verifier.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        //public bool GetAccessToken( string verifier ) {
        //    return this.GetAccessToken( OauthKey.urlAccessToken, verifier );
        //}


        /// <summary>获取access token 重载
        /// Gets the access token.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="verifier">The verifier.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool GetAccessToken( string callbackUrl, string verifier ) {
            List<Parameter> parameters = new List<Parameter>();
            this.verify = verifier;

            QWeiboRequest request = new QWeiboRequest();

            //很重要
            //this.callbackUrl = RFC3986_UrlEncode( callbackUrl );
            this.callbackUrl = null;

            return ParseToken( request.SyncRequest( urlAccessToken, "GET", this, parameters, null ) ); ;
        }

        public static string RFC3986_UrlEncode( string input ) {
            string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            StringBuilder result = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes( input );

            foreach (byte symbol in byStr) {
                if (unreservedChars.IndexOf( (char)symbol ) != -1) {
                    result.Append( (char)symbol );
                }
                else {
                    result.Append( '%' + Convert.ToString( (char)symbol, 16 ).ToUpper() );
                }
            }

            return result.ToString();
        }

        /// <summary> 解析返回结果
        /// Parses the token.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool ParseToken( string response ) {

            logger.Info( "response=" + response );

            if (string.IsNullOrEmpty( response )) {
                return false;
            }

            string[] tokenArray = response.Split( '&' );

            if (tokenArray.Length < 2) {
                return false;
            }

            string strTokenKey = tokenArray[0];
            string strTokenSecrect = tokenArray[1];


            string[] token1 = strTokenKey.Split( '=' );
            if (token1.Length < 2) {
                return false;
            }
            tokenKey = token1[1];

            string[] token2 = strTokenSecrect.Split( '=' );
            if (token2.Length < 2) {
                return false;
            }
            tokenSecret = token2[1];

            if (tokenArray.Length > 2) {
                WeiboName = tokenArray[2].Split( '=' )[1];
            }


            return true;
        }



    }
}
