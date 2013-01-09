using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.weibo.Core;
using wojilu.weibo.Domain;
using wojilu.weibo.Interface;
using wojilu.weibo.Service;

using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.weibo.Core.QQWeibo;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Users.Admin {

    public class AccountBindController : ControllerBase {

        private static readonly ILog log = LogManager.GetLogger( typeof( AccountBindController ) );

        private IUserWeiboSettingService _weiboService;
        public ILoginService loginService { get; set; }
        public IUserService userService { get; set; }

        public AccountBindController() {
            userService = new UserService();
            loginService = new LoginService();
            _weiboService = new UserWeiboSettingService();

            LayoutControllerType = typeof( UserProfileController );
        }

        [Login]
        public void Index() {

            UserWeiboSetting x = db.find<UserWeiboSetting>( "WeiboType=:id and AccessToken=:token and AccessSecret=:secret" )
         .set( "id", 2 )
         .set( "token", "" )
         .set( "secret", "" ).first();

            //set( "sinaLink", to( new WeiboRegisterController().Connect ) + "?type=sina" );
            //set( "qqweiboLink", to( new WeiboRegisterController().Connect ) + "?type=qqweibo" );
            IBlock block = getBlock( "list" );
            List<WeiboType> types = WeiboType.GetAll();
            foreach (WeiboType a in types) {

                UserWeiboSetting _setting = _weiboService.Find( ctx.viewer.Id, a.Id );
                IBlock unSyncBlock = block.GetBlock( "unSync" );
                IBlock syncBlock = block.GetBlock( "sync" );
                if (_setting == null) {

                    unSyncBlock.Set( "w.Logo", a.Logo );
                    unSyncBlock.Set( "w.Name", a.FriendName );
                    unSyncBlock.Set( "w.ConnectUrl", to( Connect, a.Id ) );
                    unSyncBlock.Next();
                }
                else {

                    syncBlock.Set( "w.Logo", a.Logo );
                    syncBlock.Set( "w.Name", a.FriendName );
                    string isSync = _setting.IsSync == 1 ? "checked='selected'" : string.Empty;
                    syncBlock.Set( "w.isSync", isSync );
                    syncBlock.Set( "w.UnbindLink", to( Unbind, a.Id ) );
                    syncBlock.Set( "w.SyncLink", to( Sync, a.Id ) );
                    syncBlock.Next();
                }
                block.Next();
            }
        }

        [Login]
        [HttpDelete]
        public void Unbind( int id ) {
            UserWeiboSetting setting = _weiboService.Find( ctx.viewer.Id, id );
            if (setting == null) {
                echoRedirect( "你并没绑定该微博", to( Index ) );
            }
            else {
                if (setting.delete() == 1)
                    echoRedirect( "你已经解绑了该微博,操作成功!", to( Index ) );
                else
                    echoRedirect( "操作失败,请重试", to( Index ) );
            }
        }

        [Login]
        [HttpPost]
        public void Sync( int id ) {
            UserWeiboSetting setting = _weiboService.Find( ctx.viewer.Id, id );
            int isSync = ctx.PostInt( "isSync" );
            if (setting == null) {
                echoError( "你并没绑定该微博" );
            }
            else {

                if (isSync == 0) {
                    _weiboService.UnSync( ctx.viewer.Id, id );
                }
                else
                    _weiboService.Sync( ctx.viewer.Id, id );

                echoAjaxOk();
            }
        }

        [Login]
        public void Connect( int id ) {
            WeiboType type = WeiboType.GetById( id );
            if (type == null) {
                echoRedirect( "not found" );
                return;
            }

            String callbackUrl = getCallbackUrl( type );

            IOAuthRequestStrategy strategy = OAuthRequestFactory.GetStrategy( type.Name );
            if (strategy != null) {
                String url = strategy.GetAuthorizationUri( callbackUrl );
                if (url == null) {
                    echoError( "对不起，绑定失败。请稍后重试。" );
                }
                else {
                    ctx.web.SessionSet( type.Name, getOauthKey( type ) );
                    redirectUrl( url );
                }
            }
        }

        private OauthKey getOauthKey( WeiboType type ) {
            return new OauthKey( type.AppKey, type.AppSecret );
        }

        private String getCallbackUrl( WeiboType type ) {
            String callbackUrl = "";
            if (type.Name == "qqweibo") {
                callbackUrl = Link.To( Site.Instance, new ConnectController().QQWeiboCallback );
            }
            else if (type.Name == "sina") {
                callbackUrl = Link.To( Site.Instance, new ConnectController().SinaWeiboCallback );
            }

            callbackUrl = strUtil.Join( ctx.url.SiteUrl, callbackUrl );
            return callbackUrl;
        }


    }
}
