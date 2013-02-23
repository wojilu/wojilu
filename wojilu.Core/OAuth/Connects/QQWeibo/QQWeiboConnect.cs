/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web;

namespace wojilu.OAuth.Connects {

    public class QQWeiboConnect : AuthConnect {

        private static readonly ILog logger = LogManager.GetLogger( typeof( QQWeiboConnect ) );

        public override string AuthorizationUrl {
            get { return "https://open.t.qq.com/cgi-bin/oauth2/authorize"; }
        }

        public override string AccessTokenUrl {
            get { return "https://open.t.qq.com/cgi-bin/oauth2/access_token"; }
        }

        private String _callbackUrl;

        public override string CallbackUrl {
            get {
                if (strUtil.HasText( _callbackUrl )) return _callbackUrl;
                return strUtil.Join( sys.Url.SiteUrl, "/Connect/Callback" ) + MvcConfig.Instance.UrlExt;
            }
            set { _callbackUrl = value; }
        }

        public override string UserProfileUrl {
            get { return "https://open.t.qq.com/api/user/info"; }
        }

        //-----------------------------------------------------------------------------

        public override OAuthUserProfile GetUserProfile( AccessToken accessToken ) {

            JsonObject user = OAuthClient.Init( this.UserProfileUrl, accessToken.Token, HttpMethod.Get )
                .AddParam( createParams( accessToken, "" ) )
                .RunJson()
                .GetJson( "data" );

            if (user == null) {
                logger.Error( "无法获取正常 user profile" );
                return null;
            }

            // 头像
            String picUrl = user.Get( "head" );
            picUrl = correctPic( picUrl );

            OAuthUserProfile x = new OAuthUserProfile();
            x.Uid = accessToken.Uid;
            x.Name = user.Get( "nick" );
            x.PicUrlSmall = picUrl;
            x.PicUrlBig = picUrl;

            return x;
        }

        //返回错误的：http://app.qlogo.cn/mbloghead/8a6fcdadac6b6d7e82e8
        //正确的是：http://t1.qlogo.cn/mbloghead/8a6fcdadac6b6d7e82e8/120
        private string correctPic( string picUrl ) {
            String[] arr = picUrl.Split( '/' );
            return string.Format( "http://t1.qlogo.cn/mbloghead/{0}/120", arr[arr.Length - 1] );
        }

        //-----------------------------------------------------------------------------


        public virtual JsonObject Publish( AccessToken accessToken, String content, String absPic ) {

            if (strUtil.IsNullOrEmpty( absPic )) {
                return PublishPost( accessToken, content );
            }
            else {
                return PublishPic( accessToken, content, absPic );
            }
        }

        public virtual JsonObject PublishPost( AccessToken accessToken, String content ) {

            if (accessToken == null) return null;
            if (strUtil.IsNullOrEmpty( accessToken.Token )) return null;
            if (strUtil.IsNullOrEmpty( content )) return null;

            JsonObject ret = OAuthClient.Init( "https://open.t.qq.com/api/t/add", accessToken.Token, HttpMethod.Post )
                .AddParam( createParams( accessToken, content ) )
                .RunJson();

            logResult( content, ret );
            return ret;
        }

        public virtual JsonObject PublishPic( AccessToken accessToken, String content, String absPic ) {

            if (accessToken == null) return null;
            if (strUtil.IsNullOrEmpty( accessToken.Token )) return null;
            if (strUtil.IsNullOrEmpty( content )) return null;

            JsonObject ret = OAuthClient.Init( "https://open.t.qq.com/api/t/add_pic", accessToken.Token, HttpMethod.Post )
                .AddParam( createParams( accessToken, content ) )
                .AddFile( absPic )
                .RunJson();

            logResult( content, ret );
            return ret;
        }

        //-----------------------------------------------------------------------------

        private Dictionary<String, String> createParams( AccessToken accessToken, String content ) {

            Dictionary<String, String> items = new Dictionary<String, String>();
            items.Add( "oauth_consumer_key", this.ConsumerKey );
            items.Add( "oauth_version", "2.a" );
            items.Add( "scope", "all" );
            items.Add( "openid", accessToken.Uid );
            items.Add( "format", "json" );
            items.Add( "clientip", "8.8.8.8" );

            items.Add( "content", content );

            return items;
        }

        private static void logResult( String content, JsonObject ret ) {
            // 常见错误：check sign error, errcode:41 原因有可能是发布过于频繁
            if (ret.Get( "msg" ) != "ok") {
                logger.Error( "发布失败，原因：" + ret.Get( "msg" ) + ", errcode:" + ret.Get<int>( "errcode" ) + "，内容：" + content );
            }
            else {
                // 如果发布相同内容，不会报错(提示ok)，但实际没有发布
                logger.Info( "发布成功，内容：" + content );
            }
        }


    }

}
