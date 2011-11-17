/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Controller.Users.Admin;
using wojilu.Serialization;

using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.Onlines;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Common.Menus.Interface;
using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;
using wojilu.Common;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Msg.Domain;
using wojilu.Common.Feeds.Domain;
using wojilu.Config;
using wojilu.Web.Controller.Security;

namespace wojilu.Web.Controller.Layouts {

    public partial class TopNavController : ControllerBase {

        public IMemberAppService userAppService { get; set; }
        public IMessageService msgService { get; set; }
        private IMenuService menuService { get; set; }
        public ICurrencyService currencyService { get; set; }

        public TopNavController() {
            userAppService = new UserAppService();
            msgService = new MessageService();
            menuService = new SiteMenuService();
            currencyService = new CurrencyService();
        }


        public void Index() {

            if (config.Instance.Site.TopNavDisplay == TopNavDisplay.Hide ||

                (config.Instance.Site.RegisterType == RegisterType.Close && config.Instance.Site.TopNavDisplay == TopNavDisplay.NoRegHide)

                ) {
                utils.setCurrentView( new Template() );
                return;
            }


            set( "site.Name", config.Instance.Site.SiteName );
            set( "site.Url", SystemInfo.SiteRoot );

            set( "LoginAction", Link.T2( Site.Instance, new MainController().CheckLogin ) );
            set( "RegLink", Link.T2( Site.Instance, new RegisterController().Register ) );
            set( "resetPwdLink", Link.T2( Site.Instance, new wojilu.Web.Controller.Common.ResetPwdController().StepOne ) );

            String emailCredit = getEmailConfirmCredit( 18 );
            String uploadCredit = getEmailConfirmCredit( 17 );

            set( "emailCredit", emailCredit );
            set( "uploadCredit", uploadCredit );

            //IBlock cblock = getBlock( "Captcha" );
            //if (config.Instance.Site.LoginNeedImgValidation) {
            //    cblock.Set( "ValidationCode", Html.Captcha );
            //    cblock.Next();
            //}

            set( "ValidationCode", Html.Captcha );

            //ajax
            //set( "navUrl", Link.T2( Site.Instance, Nav ) );
            set( "navUrl", t2( Nav ) );

        }



        private string getEmailConfirmCredit( int actionId ) {
            KeyIncomeRule rule = currencyService.GetKeyIncomeRulesByAction( actionId ); // 获取当前操作action收入规则。这里获取的是中心货币，你也可以使用 GetRulesByAction(actionId) 获取其他所有货币的收入规则            
            return string.Format( "可奖励{0}{1}", rule.Income, rule.CurrencyUnit );
        }

        //------------------------------------------------------------------------------------------------------

        public void Nav() {

            // TODO 如果是在访问用户空间，则判断：是否好友、是否关注
            echoJson( getLoginJsonString() );
        }

        private String getLoginJsonString() {

            User user = ctx.viewer.obj as User;

            Boolean isViewerOwnerSame = (ctx.owner.Id == ctx.viewer.Id && ctx.owner.obj.GetType() == ctx.viewer.obj.GetType());
            Boolean isAlertActivation = config.Instance.Site.EnableEmail && config.Instance.Site.AlertActivation;

            Dictionary<String, object> dic = new Dictionary<string, object>();

            Dictionary<String, object> viewer = new Dictionary<string, object>();
            viewer.Add( "Id", user.Id );
            viewer.Add( "IsLogin", ctx.viewer.IsLogin );
            viewer.Add( "IsAdministrator", ctx.viewer.IsAdministrator() );
            viewer.Add( "IsInAdminGroup", SiteRole.IsInAdminGroup( ctx.viewer.obj.RoleId ) );
            viewer.Add( "HasPic", user.HasUploadPic() );
            viewer.Add( "EmailConfirm", user.IsEmailConfirmed == 1 );
            viewer.Add( "IsAlertActivation", isAlertActivation );
            viewer.Add( "IsAlertUserPic", config.Instance.Site.AlertUserPic );

            Dictionary<String, object> objViewer = new Dictionary<string, object>();
            objViewer.Add( "Id", user.Id );
            objViewer.Add( "Name", user.Name );
            objViewer.Add( "FriendlyUrl", user.Url );
            objViewer.Add( "Url", Link.ToMember( user ) );
            objViewer.Add( "PicMedium", user.PicMedium );

            viewer.Add( "obj", objViewer );

            dic.Add( "viewer", viewer );
            dic.Add( "viewerOwnerSame", isViewerOwnerSame );

            Dictionary<String, object> owner = new Dictionary<string, object>();
            owner.Add( "IsSite", ctx.owner.obj.GetType() == typeof( Site ) );
            owner.Add( "Id", ctx.owner.Id );

            dic.Add( "owner", owner );
            dic.Add( "navInfo", loginNavInfo() );
            dic.Add( "online", getOnlineDic() );

            return JsonString.Convert( dic );
        }

        public Dictionary<String, Object> loginNavInfo() {

            Dictionary<String, Object> dic = new Dictionary<String, Object>();

            dic.Add( "topNavDisplay", config.Instance.Site.TopNavDisplay );

            dic.Add( "siteName", config.Instance.Site.SiteName );

            User user = (User)ctx.viewer.obj;

            dic.Add( "viewerName", user.Name );
            dic.Add( "viewerPicSmall", user.PicSmall );
            dic.Add( "viewerFeeds", Link.T2( user, new FeedController().My, -1 ) );

            dic.Add( "viewerSpace", Link.ToMember( user ) );
            dic.Add( "viewerMicroblogHome", Link.T2( user, new Microblogs.My.MicroblogController().Home ) );

            Boolean isUserHomeClose = Component.IsClose( typeof( UserHome ) );
            Boolean isMicroblogClose = Component.IsClose( typeof( MicroblogApp ) );
            Boolean isFriendClose = Component.IsClose( typeof( FriendApp ) );
            Boolean isSkinClose = Component.IsClose( typeof( SkinApp ) );
            Boolean isMessageClose = Component.IsClose( typeof( MessageApp ) );
            Boolean isFeedClose = Component.IsClose( typeof( FeedApp ) );
            Boolean isUserInviteClose = Component.IsClose( typeof( UserInviteApp ) );
            Boolean isUserAppAdminClose = Component.IsClose( typeof( UserAppAdmin ) );
            Boolean isUserMenuAdminClose = Component.IsClose( typeof( UserMenuAdmin ) );
            Boolean isUserLinksClose = Component.IsClose( typeof( UserLinks ) );
            Boolean isUserPrivacyClose = Component.IsClose( typeof( UserPrivacy ) );

            dic.Add( "isEnableUserSpace", Component.IsEnableUserSpace() );
            dic.Add( "isEnableGroup", Component.IsEnableGroup() );

            dic.Add( "isUserHomeClose", isUserHomeClose );
            dic.Add( "isMicroblogClose", isMicroblogClose );
            dic.Add( "isFriendClose", isFriendClose );
            dic.Add( "isSkinClose", isSkinClose );
            dic.Add( "isMessageClose", isMessageClose );
            dic.Add( "isFeedClose", isFeedClose );
            dic.Add( "isUserInviteClose", isUserInviteClose );
            dic.Add( "isUserAppAdminClose", isUserAppAdminClose );
            dic.Add( "isUserMenuAdminClose", isUserMenuAdminClose );
            dic.Add( "isUserLinksClose", isUserLinksClose );
            dic.Add( "isUserPrivacyClose", isUserPrivacyClose );

            dic.Add( "viewerTemplateUrl", Link.T2( user, new Users.Admin.SkinController().My ) );

            dic.Add( "viewerMsg", Link.T2( user, new MsgController().All ) );
            dic.Add( "viewerNewMsgCount", this.getMsgCount() );
            dic.Add( "viewerNewNotificationCount", this.getNewNotificationCount() );
            dic.Add( "viewerNewMicroblogAtCount", this.getNewMicroblogAtCount() );

            dic.Add( "viewerSiteNotification", getSiteNotification() );

            dic.Add( "viewerProfileUrl", Link.T2( user, new UserProfileController().Profile ) );
            dic.Add( "viewerContactLink", Link.T2( user, new UserProfileController().Contact ) );
            dic.Add( "viewerInterestUrl", Link.T2( user, new UserProfileController().Interest ) );
            dic.Add( "viewerTagUrl", Link.T2( user, new UserProfileController().Tag ) );
            dic.Add( "viewerFaceUrl", Link.T2( user, new UserProfileController().Face ) );
            dic.Add( "viewerPwdUrl", Link.T2( user, new UserProfileController().Pwd ) );

            dic.Add( "viewerInviteLink", Link.T2( user, new Users.Admin.InviteController().Index ) );

            dic.Add( "uploadAvatarLink", Link.T2( user, new Users.Admin.UserProfileController().Face ) );
            dic.Add( "confirmEmailLink", Link.T2( user, new UserProfileController().Contact ) );

            dic.Add( "viewerFriends", Link.T2( user, new Users.Admin.Friends.FriendController().List, 0 ) );
            dic.Add( "viewerCurrency", Link.T2( user, new Users.Admin.CreditController().My ) );
            dic.Add( "viewerSettings", Link.T2( user, new Users.Admin.UserProfileController().Privacy ) );

            dic.Add( "logoutLink", Link.T2( Site.Instance, new MainController().Logout ) );

            String siteAdminCmd = getAdminCmd();
            dic.Add( "siteAdminCmd", siteAdminCmd );

            dic.Add( "siteOnlineCount", OnlineStats.Instance.Count );
            dic.Add( "siteOnlineUrl", Link.T2( Site.Instance, new Users.MainController().OnlineUser ) );

            dic.Add( "shareLink", Link.T2( user, new Users.Admin.ShareController().Index, -1 ) );
            dic.Add( "myGroupsLink", Link.T2( user, new MyGroupController().My ) );

            dic.Add( "appAdminUrl", Link.T2( user, new AppController().Index ) );
            dic.Add( "menuAdminUrl", Link.T2( user, new MenuController().Index ) );
            dic.Add( "myUrlList", Link.T2( user, new MyLinkController().Index ) );

            IList userAppList = userAppService.GetByMember( user.Id );
            dic.Add( "userAppList", getAppList( userAppList ) );

            return dic;
        }

        private Dictionary<String, object> getOnlineDic() {

            OnlineStats o = OnlineStats.Instance;

            Dictionary<String, object> dic = new Dictionary<string, object>();
            dic.Add( "count", o.Count );
            dic.Add( "member", o.MemberCount );
            dic.Add( "guest", o.GuestCount );
            dic.Add( "max", o.MaxCount );
            dic.Add( "maxTime", o.MaxTime.ToShortDateString() );

            return dic;
        }

        private List<String> getAppList( IList userAppList ) {

            List<String> list = new List<String>();

            foreach (IMemberApp app in userAppList) {

                if (app.AppInfo.IsInstanceClose( ctx.viewer.obj.GetType() )) continue;

                list.Add( getNameAndUrl( app ) );
            }

            return list;
        }


        private string getSiteNotification() {

            if (ctx.viewer.obj.Id != SiteRole.Administrator.Id) return "";

            int newCount = new NotificationService().GetUnReadCount( Site.Instance.Id, typeof( Site ).FullName );
            if (newCount <= 0) return "";

            User user = (User)ctx.viewer.obj;

            String lnk = Link.T2( user, new Users.Admin.SiteNfController().List );
            return string.Format( "<a href=\"{0}\">通知(<span id=\"siteNotificationText\">{1}</span>)</a>", lnk, newCount );
        }

        //-------------------------------------------------------------------------------------------------------------

        public void Header() {

            set( "site.Name", Site.Instance.Name );
            set( "site.Logo", config.Instance.Site.GetLogoHtml() );

            set( "adBanner", AdItem.GetAdById( AdCategory.Banner ) );
            set( "adNavBottom", AdItem.GetAdById( AdCategory.NavBottom ) );

            List<IMenu> list = menuService.GetList( Site.Instance );
            IMenu currentRootMenu = bindSubMenus( list );
            bindNavList( list, currentRootMenu );

            List<Dictionary<string, string>> langs = wojilu.lang.GetSupportedLang();
            bindList( "langs", "lang", langs, bindLang );
        }

        private void bindLang( IBlock block, String lbl, object obj ) {
            Dictionary<string, string> map = (Dictionary<string, string>)obj;
            block.Set( "lang.SetLink", t2( new LangController().Switch ) + "?lang=" + map["Value"] );

            String img = "<img src=\"" + sys.Path.Img + "oks.gif\" {0} />";
            String currentStyle = map["Value"].Equals( wojilu.lang.getLangString() ) ? string.Format( img, "" ) : string.Format( img, "class=\"noCurrentLang\"" );
            block.Set( "lang.CurrentStyle", currentStyle );
        }


        private void bindNavList( List<IMenu> menus, IMenu currentRootMenu ) {

            List<IMenu> list = MenuHelper.getRootMenus( menus );

            IBlock block = getBlock( "navLink" );

            for (int i = 0; i < list.Count; i++) {

                String itemId = (i == list.Count - 1 ? "menuItemLast" : "menuItem" + i);
                block.Set( "menu.ItemId", itemId );

                IMenu menu = list[i];

                String currentClass = "";
                if (currentRootMenu != null && menu.Id == currentRootMenu.Id) currentClass = " class=\"currentRootMenu\" ";
                block.Set( "menu.CurrentClass", currentClass );

                IBlock subNavBlock = block.GetBlock( "subNav" );
                IBlock rootBlock = block.GetBlock( "rootNav" );
                List<IMenu> subMenus = MenuHelper.getSubMenus( menus, menu );

                if (subMenus.Count == 0) {
                    MenuHelper.bindMenuSingle( rootBlock, menu, ctx );
                }
                else {
                    IBlock subBlock = subNavBlock.GetBlock( "subMenu" );
                    MenuHelper.bindSubMenus( subBlock, subMenus, ctx );
                    MenuHelper.bindMenuSingle( subNavBlock, menu, ctx );
                }

                block.Next();

            }
        }


        private IMenu bindSubMenus( List<IMenu> list ) {

            IMenu currentRootMenu = MenuHelper.getCurrentRootMenu( list, ctx );
            List<IMenu> subMenus = MenuHelper.getSubMenus( list, currentRootMenu );

            IBlock subMenusPanel = getBlock( "subMenusPanel" );

            if (subMenus.Count > 0) {
                IBlock block = subMenusPanel.GetBlock( "subMenus" );
                MenuHelper.bindSubMenus( block, subMenus, ctx );

                subMenusPanel.Next();
            }

            return currentRootMenu;
        }


    }

}
