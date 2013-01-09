using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.weibo.Core;
using wojilu.weibo.Core.QQWeibo;

namespace wojilu.Web.Controller {

    public class ConnectController : ControllerBase {



        [Login]
        public void QQWeiboCallback() {

            OauthKey key = ctx.web.SessionGet( "qqweibo" ) as OauthKey;
            string verifier = ctx.Get( "oauth_verifier" );
            if (key == null || string.IsNullOrEmpty( verifier )) {
                echo( "请不要直接访问此页面" );
                return;
            }

            String lnkCallback = strUtil.Join( ctx.url.SiteUrl, to( QQWeiboCallback ) );

            IOAuthRequestStrategy strategy = OAuthRequestFactory.GetQQWeiboStrategy();
            if (strategy != null) {
                Result result = strategy.ProcessCallback( ctx.viewer.Id, verifier, lnkCallback );
                ctx.web.SessionSet( "qqweibo", null );
                showCallbackResult( result );
            }
        }

        [Login]
        public void SinaWeiboCallback() {

            string code = ctx.Get( "code" );
            if (string.IsNullOrEmpty( code )) {
                echoRedirect( "请不要直接进入此页面" );
                return;
            }

            String lnkCallback = strUtil.Join( ctx.url.SiteUrl, to( SinaWeiboCallback ) );

            IOAuthRequestStrategy strategy = OAuthRequestFactory.GetSinaStrategy();
            if (strategy != null) {
                Result result = strategy.ProcessCallback( ctx.viewer.Id, code, lnkCallback );
                showCallbackResult( result );
            }
        }


        private void showCallbackResult( Result result ) {
            if (result.HasErrors) {
                echo( result.ErrorsHtml );
            }
            else {
                echoRedirect( "绑定成功", "/" );
            }
        }

    }

}
