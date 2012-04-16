using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.weibo.Data.Sina;
using wojilu.weibo.Domain;
using wojilu.Common.Resource;
using wojilu.Web;
using wojilu.Common;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Interface;
using wojilu.weibo.Controller;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Config;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase;
using wojilu.weibo.Interface;
using wojilu.weibo.Service;
using wojilu.Members.Users.Service;
using wojilu.weibo.Core.QQWeibo;
using wojilu.weibo.Core;

namespace wojilu.weibo.Controller.Weibo
{
    public class WeiboRegisterController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(WeiboRegisterController));

        public IUserService userService { get; set; }
        public IUserConfirmService confirmService { get; set; }
        public IInviteService inviteService { get; set; }
        public ILoginService loginService { get; set; }
        public IFriendService friendService { get; set; }

        public IUserWeiboSettingService _weiboSettingService = new UserWeiboSettingService();

        public virtual IMemberAppService appService { get; set; }

        public virtual IMenuService menuService { get; set; }

        public WeiboRegisterController()
        {
            userService = new UserService();
            confirmService = new UserConfirmService();
            inviteService = new InviteService();
            loginService = new LoginService();
            friendService = new FriendService();

            appService = new UserAppService();
            menuService = new UserMenuService();
        }

        public void Connect()
        {
            string type = ctx.Get("type");
            if (ctx.viewer.IsLogin)
            {
                echoRedirect("你已经有帐号并且已经登陆了","/");
                return;
            }
            if (string.IsNullOrEmpty(type) == null)
            {
                echoRedirect("请不要直接进入此页面");
                return;
            }
            IOAuthRequestStrategy strategy = OAuthRequestFactory.GetStrategy(type);
            if (strategy != null)
            {
                strategy.RedirectToAuthorizationUri(this);
            }
        }

        /// <summary>
        /// 用其它微博账号进行注册
        /// </summary>
        public void Bind()
        {
            if (ctx.viewer.IsLogin)
            {
                echo("您有帐号，并且已经登录");
                return;
            }

            WeiboSession session = ctx.web.SessionGet(WeiboSession.SessionName) as WeiboSession;
            if(session==null)
            {
                redirectUrl("/");
            }

            UserWeiboSetting setting = _weiboSettingService.Find(session.Setting.WeiboType, session.Setting.AccessToken, session.Setting.AccessSecret);

            //如果用户已经微博绑定此帐户
            if (setting != null)
            {
                //这里做用户登陆处理
               User user = userService.GetById(setting.UserId);
                if (user != null)
                {
                    loginService.Login(user, LoginTime.OneMonth, ctx.Ip, ctx);
                    redirectUrl("/");
                    return;
                }
                else
                {
                    echoRedirect("发生未知错误，请重试");
                    return;
                }
            }

            target(bindSave);

            Page.Title = lang("userReg");


            String userNameNote = string.Format(lang("userNameNote"),
                config.Instance.Site.UserNameLengthMin,
                config.Instance.Site.UserNameLengthMax);

            set("userNameNote", userNameNote);

            set("m.Name", ctx.Post("Name"));

            set("m.ScreenName", session.ScreenName);
            set("m.ProfileImg", session.ProfileImg);
            set("m.WeiboFriendName", session.WeiboFriendName);

            set("m.Password1", ctx.Post("Password1"));
            set("m.Password2", ctx.Post("Password2"));
            set("m.Email", ctx.Post("Email"));
            set("m.FriendUrl", ctx.Post("FriendUrl"));

            int gender = ctx.PostInt("Gender");
            if (gender <= 0) gender = 1;

            String genderStr = Html.RadioList(AppResource.Gender, "Gender", "Name", "Value", gender);

            set("m.Gender", genderStr);

            IBlock urlBlock = getBlock("friendlyUrl");
            IBlock subdomainBlock = getBlock("subDomainFriendlyUrl");


            if (Component.IsEnableUserSpace())
            {

                if (MvcConfig.Instance.CheckDomainMap())
                {
                    subdomainBlock.Set("userUrlPrefix", SystemInfo.HostNoSubdomain);
                    subdomainBlock.Set("isUrlValidLink", to(CheckUrlExist));
                    subdomainBlock.Next();
                }
                else
                {
                    urlBlock.Set("userUrlPrefix", strUtil.TrimStart(strUtil.Append(ctx.url.SiteAndAppPath, "/"), "http://"));
                    urlBlock.Set("urlExt", MvcConfig.Instance.UrlExt);
                    urlBlock.Set("isUrlValidLink", to(CheckUrlExist));
                    urlBlock.Next();

                }
            }

            // validation
            set("isEmailValidLink", to(CheckEmailExist));
            set("isNameValidLink", to(CheckUserExist));

            bindCaptcha();
        }

        [HttpPost, DbTransaction]
        public void CheckUrlExist()
        {

            String url = ctx.Post("FriendUrl");
            if (userService.IsUrlReservedOrExist(url))
            {
                echoJsonMsg(lang("exUrlFound"), false, "");
            }
            else
            {
                echoJsonMsg("ok", true, "");
            }
        }

        [HttpPost, DbTransaction]
        public void CheckEmailExist()
        {
            String email = ctx.Post("Email");

            if (userService.IsEmailExist(email))
            {
                echoError(lang("exEmailFound"));
                return;
            }

            echoRedirect("ok");
        }

        [HttpPost, DbTransaction]
        public void CheckUserExist()
        {

            String name = ctx.Post("Name");

            if (userService.IsNameReservedOrExist(name))
            {
                echoError(lang("exNameFound"));
                return;
            }

            echoRedirect("ok");
        }

        private void bindCaptcha()
        {

            IBlock cblock = getBlock("Captcha");
            if (config.Instance.Site.RegisterNeedImgValidateion)
            {
                cblock.Set("ValidationCode", Html.Captcha);
                cblock.Next();
            }
        }

        public User validateUser()
        {

            if (config.Instance.Site.RegisterNeedImgValidateion) Html.Captcha.CheckError(ctx);

            String name = ctx.Post("Name");
            String pwd = ctx.Post("Password1");
            String pageUrl = ctx.Post("FriendUrl");
            String email = ctx.Post("Email");


            if (strUtil.IsNullOrEmpty(name))
            {
                errors.Add(lang("exUserName"));
            }
            else if (name.Length < config.Instance.Site.UserNameLengthMin)
            {
                errors.Add(string.Format(lang("exUserNameLength"), config.Instance.Site.UserNameLengthMin));
            }
            else
            {
                name = strUtil.SubString(name, config.Instance.Site.UserNameLengthMax);
            }
            if (strUtil.IsAbcNumberAndChineseLetter(name) == false) errors.Add(lang("exUserNameError"));


            if (strUtil.IsNullOrEmpty(pwd))
                errors.Add(lang("exPwd"));
            else
                pwd = strUtil.CutString(pwd, 20);


            if (Component.IsEnableUserSpace())
            {
                if (strUtil.IsNullOrEmpty(pageUrl))
                {
                    errors.Add(lang("exUrl"));
                }
                else if (pageUrl.IndexOf("http:") >= 0)
                {
                    errors.Add(lang("exUserUrlHttpError"));
                }
                else
                {
                    pageUrl = strUtil.SubString(pageUrl, config.Instance.Site.UserNameLengthMax);
                    pageUrl = pageUrl.ToLower();
                }

                if (strUtil.IsUrlItem(pageUrl) == false) errors.Add(lang("exUserUrlError"));

            }

            if (strUtil.IsNullOrEmpty(email))
                errors.Add(lang("exEmail"));
            else
            {
                if (RegPattern.IsMatch(email, RegPattern.Email) == false) errors.Add(lang("exUserMail"));
            }

            User user = new User();
            user.Name = name;
            user.Pwd = pwd;
            user.Url = pageUrl;
            user.Email = email;
            user.Gender = ctx.PostInt("Gender");
            return user;
        }

        [HttpPost, DbTransaction]
        public void bindSave() 
        {
           
            WeiboSession session = ctx.web.SessionGet(WeiboSession.SessionName) as WeiboSession;

            if (session == null)
            {
                redirectUrl("/");
            }

            UserWeiboSetting setting = _weiboSettingService.Find(session.Setting.WeiboType, session.Setting.AccessToken, session.Setting.AccessSecret);
            User user=null;
            //如果用户已经微博绑定此帐户
            if (setting != null)
            {
                //这里做用户登陆处理
                user = userService.GetById(setting.UserId);
                if (user != null)
                {
                    loginService.Login(user, LoginTime.OneMonth, ctx.Ip, ctx);
                    redirectUrl("/");
                    return;
                }
                else
                {
                    echoRedirect("发生未知错误，请重试");
                    return;
                }
            }


             user = validateUser();
            if (errors.HasErrors)
            {
                run(Bind);
                return;
            }

            user = userService.Register(user, ctx);
            if ((user == null) || errors.HasErrors)
            {
                run(Bind);
                return;
            }

            session.Setting.UserId = user.Id;
            Result result =  session.Setting.insert();
           if (result.HasErrors)
           {
               errors.Join(result);
               return;
           }
           ctx.web.SessionSet("weiboSetting", null);

            if (Component.IsEnableUserSpace())
            {
                addUserAppAndMenus(user);
            }

            if (config.Instance.Site.UserNeedApprove)
            {

                user.Status = MemberStatus.Approving;
                user.update("Status");

                view("needApproveMsg");
                set("siteName", config.Instance.Site.SiteName);

            }
            else if (config.Instance.Site.EnableEmail)
            {
                if (config.Instance.Site.LoginType == LoginType.Open)
                {
                    loginService.Login(user, LoginTime.Forever, ctx.Ip, ctx);
                }
            }
            else
            {
                loginService.Login(user, LoginTime.Forever, ctx.Ip, ctx);
                echoRedirect(lang("registerok"), getSavedReturnUrl());
            }
        }

        private String getSavedReturnUrl()
        {
            String returnUrl = ctx.Post("returnUrl");
            if (strUtil.IsNullOrEmpty(returnUrl))
            {
                returnUrl = sys.Path.Root;
            }
            return returnUrl;
        }

        private void addUserAppAndMenus(User user)
        {
            //todo 以后可以将这个项目直接集成在我记录系统中,由于没有对wojilu.controller引用,这里没法写
        }
    }
}
