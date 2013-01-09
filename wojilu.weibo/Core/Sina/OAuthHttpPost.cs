using System;
using System.Collections.Generic;
using wojilu.weibo.Common;

namespace wojilu.weibo.Core.Sina
{
    /// <summary>
    /// Performs a OAuth token-attached Http-Post request to the resource identified by the uri with the combination of OAuth parameters and the specified additionalParams(optional).
    /// </summary>
    public class OAuthHttpPost : HttpPost
    {
       // private IList<ParamPair> additionalParams = new Collection<ParamPair>();
        private ParamCollection postParams = new ParamCollection();

        private string token;

        /// <summary>
        /// Initializes a new instance of <see cref="OAuthHttpPost"/> with the specified <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The uri to identify a resource in the remote server.</param>
        public OAuthHttpPost(string uri,string token) : base(uri)
        {
            this.token = token;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="OAuthHttpPost"/> with the specified <paramref name="uri"/> and <paramref name="additionalParams"/>.
        /// </summary>
        /// <param name="uri">The uri to identify a resource in the remote server.</param>
        /// <param name="additionalParams">Additional data to post.</param>
        public OAuthHttpPost(string uri,string token ,IList<ParamPair> additionalParams)
            : this(uri,token)
        {
            foreach (var item in additionalParams)
                this.postParams.Add(item);
        }

        /// <summary>
        /// Gets or sets the custom post data(in <c>ParamPair</c> format, non-urlencoded).
        /// <remarks>Do not UrlEncode the <c>ParamPair</c>, it is done inside this method.</remarks>
        /// </summary>
        public ParamCollection Params
        {
            get
            {
                return postParams;
            }
        }

        /// <summary>
        /// Gets the data to post.
        /// </summary>
        public sealed override string PostData
        {
            get
            {

                postParams.Add(SinaConstants.OAuthToken,token);

                return OAuthHelper.PreparePostBody(postParams);
            }
            set
            {
                throw new NotSupportedException("Not supported. Please use CustomData property to convey data to post.");                
            }
        }
    }
}
