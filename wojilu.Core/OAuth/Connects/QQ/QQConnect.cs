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


    public class QQConnect : AuthConnect {

        private static readonly ILog logger = LogManager.GetLogger( typeof( QQConnect ) );

        public override string AuthorizationUrl {
            get { return "https://graph.qq.com/oauth2.0/authorize"; }
        }

        public override string AccessTokenUrl {
            get { return "https://graph.qq.com/oauth2.0/token"; }
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
            get { return "https://graph.qq.com/user/get_user_info"; }
        }

        public override string Scope {
            get { return "get_user_info,add_t,add_pic_t"; }
        }

        public override string HttpMethod_AccessToken {
            get { return HttpMethod.Get; }
        }

        //-----------------------------------------------------------------------------

        public override OAuthUserProfile GetUserProfile( AccessToken accessToken ) {

            JsonObject user = OAuthClient.Init( this.UserProfileUrl, accessToken.Token, HttpMethod.Get )
                //.SetEncoding( "utf-8" )
                .AddParam( createParams( accessToken, "" ) )
                .RunJson();

            if (user.Get<long>( "ret" ) != 0) {
                logger.Error( "无法获取正常 user profile, 原因：" + user.Get( "msg" ) );
                return null;
            }

            OAuthUserProfile x = new OAuthUserProfile();
            x.Uid = accessToken.Uid;
            x.Name = user.Get( "nickname" );
            x.PicUrlSmall = user.Get( "figureurl_1" );
            x.PicUrlBig = user.Get( "figureurl_2" );

            return x;
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

            String apiUrl = "https://graph.qq.com/t/add_t";
            Dictionary<String, String> items = createParams( accessToken, content );

            JsonObject ret = OAuthClient.Init( apiUrl, accessToken.Token, HttpMethod.Post )
                .AddParam( items )
                .RunJson();

            logResult( content, ret );

            return ret;
        }

        public virtual JsonObject PublishPic( AccessToken accessToken, String content, String absPic ) {

            if (accessToken == null) return null;
            if (strUtil.IsNullOrEmpty( accessToken.Token )) return null;
            if (strUtil.IsNullOrEmpty( content )) return null;

            String apiUrl = "https://graph.qq.com/t/add_pic_t";
            Dictionary<String, String> items = createParams( accessToken, content );

            JsonObject ret = OAuthClient.Init( apiUrl, accessToken.Token, HttpMethod.Post )
                .AddParam( items )
                .AddFile( absPic )
                .RunJson();

            logResult( content, ret );
            return ret;
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

        //-----------------------------------------------------------------------------


        public static String GetOpenId( String accessToken ) {

            String openIdUrl = "https://graph.qq.com/oauth2.0/me?access_token=" + accessToken;
            String response = wojilu.Net.PageLoader.Download( openIdUrl );

            logger.Info( "获取openid返回结果=" + response );

            String strJson = strUtil.TrimStart( response.Trim().TrimEnd( ';' ), "callback(" ).TrimEnd( ')' );
            JsonObject obj = Json.ParseJson( strJson );

            String ret = obj.Get( "openid" );
            logger.Info( "openid=" + ret );

            if (strUtil.IsNullOrEmpty( ret )) throw new HttpClientException( "无法获取open id, 错误原因：" + strJson );

            return ret;
        }

        public override string GetUid( AccessToken accessToken ) {

            if (strUtil.IsNullOrEmpty( accessToken.Uid )) {
                String openId = GetOpenId( accessToken.Token );
                accessToken.Uid = openId;
            }

            return accessToken.Uid;
        }

        private Dictionary<String, String> createParams( AccessToken accessToken, String content ) {

            if (strUtil.IsNullOrEmpty( accessToken.Uid )) {
                accessToken.Uid = GetOpenId( accessToken.Token );
            }

            Dictionary<String, String> items = new Dictionary<String, String>();

            items.Add( "oauth_consumer_key", this.ConsumerKey );
            items.Add( "openid", accessToken.Uid );
            items.Add( "format", "json" );

            items.Add( "content", content );

            return items;
        }


    }

}
