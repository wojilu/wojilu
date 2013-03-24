/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web;
using wojilu.Net;

namespace wojilu.OAuth.Connects {

    public class WeiboConnect : AuthConnect {

        private static readonly ILog logger = LogManager.GetLogger( typeof( WeiboConnect ) );

        public override string AuthorizationUrl {
            get { return "https://api.weibo.com/oauth2/authorize"; }
        }

        public override string AccessTokenUrl {
            get { return "https://api.weibo.com/oauth2/access_token"; }
        }

        public override string UserProfileUrl {
            get { return "https://api.weibo.com/2/users/show.json"; }
        }

        private String _callbackUrl;

        public override string CallbackUrl {
            get {
                if (strUtil.HasText( _callbackUrl )) return _callbackUrl;
                return strUtil.Join( sys.Url.SiteUrl, "/Connect/Callback" ) + MvcConfig.Instance.UrlExt;
            }
            set { _callbackUrl = value; }
        }

        public override OAuthUserProfile GetUserProfile( AccessToken accessToken ) {

            JsonObject user = OAuthClient.Init( this.UserProfileUrl, accessToken.Token, HttpMethod.Get )
                .AddParam( "uid", accessToken.Uid )
                .RunJson();

            if (user.Get<long>( "id" ) <= 0) {
                logger.Error( "无法获取正常 user profile" );
                return null;
            }

            OAuthUserProfile x = new OAuthUserProfile();
            x.Uid = accessToken.Uid;
            x.Name = user.Get( "name" );
            x.FriendlyUrl = user.Get( "domain" );
            x.PicUrlSmall = user.Get( "profile_image_url" );
            x.PicUrlBig = user.Get( "avatar_large" );

            return x;
        }

        //------------------------------------------------------------------------------------

        public virtual JsonObject Publish( String accessToken, String content, String absPic ) {

            if (strUtil.IsNullOrEmpty( absPic )) {
                return PublishPost( accessToken, content );
            }
            else {
                return PublishPic( accessToken, content, absPic );
            }
        }

        public virtual JsonObject PublishPost( String accessToken, String content ) {

            if (strUtil.IsNullOrEmpty( accessToken )) return null;
            if (strUtil.IsNullOrEmpty( content )) return null;

            try {
                JsonObject ret = OAuthClient.Init( "https://api.weibo.com/2/statuses/update.json", accessToken, "POST" )
                    .AddParam( "status", content )
                    .RunJson();
                logResult( content, ret );
                return ret;
            }
            catch (HttpClientException ex) {
                processEx( content, ex );
                return null;
            }
        }

        public virtual JsonObject PublishPic( String accessToken, String content, String absPic ) {

            if (strUtil.IsNullOrEmpty( accessToken )) return null;
            if (strUtil.IsNullOrEmpty( content )) return null;

            try {
                JsonObject ret = OAuthClient.Init( "https://api.weibo.com/2/statuses/upload.json", accessToken, "POST" )
                    .AddParam( "status", content )
                    .AddFile( absPic )
                    .RunJson();

                logResult( content, ret );
                return ret;
            }
            catch (HttpClientException ex) {
                processEx( content, ex );
                return null;
            }
        }

        //--------------------------------------------------------------------

        private static void logResult( String content, JsonObject ret ) {
            if (strUtil.HasText( ret.Get( "text" ) )) {
                logger.Info( "发布成功，内容：" + content );
            }
            else {
            }
        }

        private void processEx( String content, HttpClientException ex ) {
            //{"error":"invalid_access_token","error_code":21332,"request":"/2/statuses/update.json"}
            //{"error":"expired_token","error_code":21327,"request":"/2/statuses/update.json"}

            // 原因不明，请查看已经记录的日志，此处不处理
            if (ex.ErrorInfo == null) {
                return;
            }

            if (ex.ErrorInfo.IndexOf( "invalid_access_token" ) >= 0) {
                refreshToken();
                republishPost( content );
                return;
            }

            // token过期处理
            if (ex.ErrorInfo.IndexOf( "expired_token" ) >= 0) {
            }

        }

        // TODO 1
        private void republishPost( string content ) {
            logger.Error( "republishPost" );
        }

        // TODO 2
        private void refreshToken() {
            logger.Error( "refreshToken" );
        }

    }

}
