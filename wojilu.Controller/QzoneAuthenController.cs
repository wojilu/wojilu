/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Common;
using wojilu.Common.AppBase;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Common.Resource;

using System.Collections;
using System.Collections.Generic;
using wojilu.Config;
using wojilu.Serialization;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Utils;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller
{

    public class QzoneAuthenController : ControllerBase
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(QzoneAuthenController));
        private string apiid = config.Instance.Site.QQAPI_appId;
        private string apipass = config.Instance.Site.QQAPI_appPass;
        private string apiurl = config.Instance.Site.QQAPI_redirectUri;
        private string state = Guid.NewGuid().ToString().Replace("-", "");

        public IUserService userService { get; set; }
        public IUserConfirmService confirmService { get; set; }
        public IConfirmEmail confirmEmail { get; set; }
        public ILoginService loginService { get; set; }
        public IInviteService inviteService { get; set; }

        public virtual IMemberAppService appService { get; set; }
        public virtual IMenuService menuService { get; set; }


        public QzoneAuthenController()
        {
            userService = new UserService();
            confirmService = new UserConfirmService();
            confirmEmail = new ConfirmEmail();
            loginService = new LoginService();

            appService = new UserAppService();
            menuService = new UserMenuService();

            inviteService = new InviteService();

            HidePermission(typeof(SecurityController));

        }

        public override void CheckPermission()
        {
            if (config.Instance.Site.RegisterType == RegisterType.Close)
            {
                echo("对不起，网站已经关闭注册");
            }
        }

        public void QQlogin()
        {
            ctx.web.SessionSet("state",state);
            if (null == apiid || apiid == "")
            {
                echo("对不起，没有找到QQAPI信息");
                return;
            }
            if (null == apiurl || apiurl == "")
            {
                echo("对不起，没有找到QQAPI回调地址信息");
                return;
            }
            //string login_url = "https://graph.qq.com/oauth2.0/authorize?response_type=token&client_id=" + apiid + "&state=" + state + "&redirect_uri=" + ctx.web.UrlEncode(apiurl) + "&scope=get_user_info,get_info,get_other_info";
            string login_url = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id=" + apiid + "&state=" + state + "&redirect_uri=" + ctx.web.UrlEncode(apiurl) + "&scope=get_user_info,get_info,get_other_info";
            ctx.web.Redirect(login_url);
            //logger.Info("");

        }

        public void login()
        {
            logger.Info("url:" + System.Web.HttpContext.Current.Request.Url +"  ip:"+ctx.Ip);

            if (ctx.web.SessionGet("state") == null || ctx.web.SessionGet("state").ToString() == "" || ctx.web.SessionGet("state").ToString() != ctx.Get("state"))
            {
                echo("对不起，请求状态异常");
                logger.Info("QQlogin请求发现异常的state:" + ctx.Get("state") + "系统发出的state：" + ctx.web.SessionGet("state").ToString());
                return;
            }
            if (ctx.web.SessionGet("access_token") == null || ctx.web.SessionGet("access_token").ToString() == "")
            {
                try
                {
                    string code = ctx.Get("code");
                    System.Net.WebRequest wReq = System.Net.WebRequest.Create("https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id=" + apiid + "&client_secret=" + apipass + "&code=" + code + "&state=" + ctx.web.SessionGet("state") + "&redirect_uri=" + ctx.web.UrlEncode(apiurl));
                    System.Net.WebResponse wResp = wReq.GetResponse();
                    System.IO.Stream respStream = wResp.GetResponseStream();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("utf-8")))
                    {
                        string access_token = reader.ReadToEnd();
                        access_token = access_token.Substring(access_token.IndexOf("=") + 1, access_token.IndexOf("&") - access_token.IndexOf("=")-1);
                        ctx.web.SessionSet("access_token", access_token);
                    }
                }
                catch
                {
                    echo("对不起，QQlogin通讯失败");
                    logger.Info("QQlogin通讯失败");
                    return;
                }
            }
            string call_back = "";
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create("https://graph.qq.com/oauth2.0/me?access_token=" + ctx.web.SessionGet("access_token"));
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("utf-8")))
                {
                    call_back = reader.ReadToEnd();
                }
            }
            catch
            {
                echo("对不起，QQlogin通讯失败");
                logger.Info("QQlogin通讯失败");
                return;
            }
            string openid = "";
            try
            {
                Dictionary<String, object> openid_json = JSON.ToDictionary(call_back.Substring(call_back.IndexOf("(") + 1, call_back.Length - call_back.IndexOf("(") - 1));
                foreach (KeyValuePair<string, object> item in openid_json)
                {
                    if (item.Key == "openid") openid = item.Value.ToString();
                }
            }
            catch
            {
                echo("对不起，没有获得可识别的openid");
                logger.Info("没有获得可识别的openid");
                return;
            }
            ctx.web.SessionSet("openid", openid);
            redirectUrl(to(QQloginConfirm));
        }

        public void QQloginConfirm()
        {
            string openid = ctx.web.SessionGet("openid").ToString();
            string access_token = ctx.web.SessionGet("access_token").ToString();
            string user_info = QQlogin_get_user_info(openid, access_token);
            if (user_info == "-1")
            {
                echo("对不起，没有获得用户信息");
                logger.Info("没有获得用户信息");
                return;
            }
            ctx.web.SessionSet("user_info", user_info);
            ctx.web.SessionSet("openid", openid);
            User user = User.find("QQOpenId=:openid").set("openid", openid).first();
            if (user != null)
            {
                loginService.Login(user, LoginTime.Forever, ctx.Ip, ctx);
                redirectUrl(getSavedReturnUrl());//
                return;
            }

            view("/Register/QQloginConfirm");
            target(SaveBind);

            Page.Title = lang("userBind");

            String loginTip = string.Format(lang("loginTip"), to(new MainController().Login));
            set("loginTip", loginTip);

            String userNameNote = string.Format(lang("userNameNote"),
               config.Instance.Site.UserNameLengthMin,
               config.Instance.Site.UserNameLengthMax);

            set("userNameNote", userNameNote);

            set("m.Name", ctx.Post("Name"));

            set("m.Password1", ctx.Post("Password1"));
            set("isNameValidLink", to(CheckUserExist));

            IBlock confirmEmailBlock = getBlock("sendConfirmEmail");

            confirmEmailBlock.Set("resendLink", to(NewQQLogin));
            confirmEmailBlock.Next();

            bindCaptcha();

        }

        public void Register()
        {

            if (config.Instance.Site.RegisterType == RegisterType.CloseUnlessInvite)
            {
                echo("对不起，您必须接受邀请才能注册");
                return;
            }

            bindRegister();
        }

        private void bindRegister()
        {

            target(SaveReg);


            Page.Title = lang("userReg");

            String loginTip = string.Format(lang("loginTip"), to(new MainController().Login));
            set("loginTip", loginTip);

            String userNameNote = string.Format(lang("userNameNote"),
                config.Instance.Site.UserNameLengthMin,
                config.Instance.Site.UserNameLengthMax);

            set("userNameNote", userNameNote);

            set("m.Name", ctx.Post("Name"));

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

            IBlock confirmEmailBlock = getBlock("sendConfirmEmail");
            if (config.Instance.Site.EnableEmail)
            {
                confirmEmailBlock.Set("resendLink", to(new Common.ActivationController().SendEmailLogin));
                confirmEmailBlock.Next();
            }

            bindCaptcha();
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

        [HttpPost, DbTransaction]
        public void CheckEmailExist()
        {

            String email = ctx.Post("Email");

            if (userService.IsEmailExist(email))
            {
                echoError(lang("exEmailFound"));
                return;
            }

            echoJsonOk();
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

            echoJsonOk();
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
                echoJsonOk();
            }

        }

        //--------------------------------------------------------------------------------

        public void Done()
        {

            set("email", ctx.Get("email"));
            set("domainName", config.Instance.Site.GetSmtpUserDomain());

            String goUrl = WebUtils.getMailLink(ctx.Get("email"));
            IBlock mblock = getBlock("mailLink");
            if (strUtil.HasText(goUrl))
            {
                mblock.Set("goUrl", goUrl);
                mblock.Next();
            }

            set("resendLink", to(new Common.ActivationController().SendEmailLogin));
        }

        [HttpPost, DbTransaction]
        public void SaveReg()
        {

            if (ctx.viewer.IsLogin)
            {
                echo("您有帐号，并且已经登录");
                return;
            }

            if (config.Instance.Site.RegisterType == RegisterType.CloseUnlessInvite)
            {

                int friendId = ctx.PostInt("friendId");
                String friendCode = ctx.Post("friendCode");
                Result result = inviteService.Validate(friendId, friendCode);
                if (result.HasErrors)
                {
                    echo(result.ErrorsHtml);
                    return;
                }
            }

            // 验证
            User user = validateUser();
            if (errors.HasErrors)
            {
                run(Register);
                return;
            }

            // 用户注册
            user = userService.Register(user, ctx);
            if ((user == null) || errors.HasErrors)
            {
                run(Register);
                return;
            }

            // 是否开启空间
            RegUtils.CheckUserSpace(user, ctx);

            // 好友处理
            RegUtils.ProcessFriend(user, ctx);

            // 是否需要审核、激活
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

                redirectUrl(to(Done) + "?email=" + user.Email);

            }
            else
            {
                loginService.Login(user, LoginTime.Forever, ctx.Ip, ctx);
                echoRedirect(lang("registerok"), getSavedReturnUrl());
            }

        }

        public void SaveBind()
        {

            if (ctx.viewer.IsLogin)
            {
                echo("您有帐号，并且已经登录");
                return;
            }

            // 验证
            User user = validateUser();
            if (errors.HasErrors)
            {
                run(QQloginConfirm);
                return;
            }

            // 用户注册
            user = userService.Bind(user, ctx);
            if ((user == null) || errors.HasErrors)
            {
                echo("非常抱歉，您的帐号绑定失败了，请稍后再试");
                redirectUrl(getSavedReturnUrl());
                return;
            }
            loginService.Login(user, LoginTime.Forever, ctx.Ip, ctx);

            echoRedirect(lang("bindok"), getSavedReturnUrl());
            return;

        }

        public void needApproveMsg()
        {
            set("siteName", config.Instance.Site.SiteName);
        }

        public void SendConfirmEmail(int userId)
        {

            User user = userService.GetById(userId);
            Boolean sent = confirmEmail.SendEmail(user, null, null);

            if (sent)
            {
                echoAjaxOk();
            }
            else
                echoText("对不起，发送失败，请稍后再试");
        }

        //--------------------------------------------------------------------------------

        private String getSavedReturnUrl()
        {
            String returnUrl = ctx.Post("returnUrl");
            if (strUtil.IsNullOrEmpty(returnUrl))
            {
                returnUrl = sys.Path.Root;
            }
            return returnUrl;
        }

        public User validateUser()
        {

            if (config.Instance.Site.RegisterNeedImgValidateion) Html.Captcha.CheckError(ctx);

            String name = ctx.Post("Name");
            String pwd = ctx.Post("Password1");
            //String pageUrl = ctx.Post("FriendUrl");
            //String email = ctx.Post("Email");


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

            User user = new User();
            user.Name = name;
            user.Pwd = pwd;
            //user.Url = pageUrl;
            //user.Email = email;
            //user.Gender = ctx.PostInt("Gender");
            return user;
        }

        public void NewQQLogin()
        {
            if (ctx.viewer.IsLogin)
            {
                echo("您有帐号，并且已经登录");
                redirectUrl(getSavedReturnUrl());
                return;
            }
            User user = new User();
            user.Name = ctx.web.SessionGet("user_info").ToString(); ;
            user.QQOpenId = ctx.web.SessionGet("openid").ToString();
            user.Pwd = "QQ";
            user.Url = "qquser" + ctx.web.SessionGet("openid").ToString();
            user.Email = "QQ@QQ.com";
            user = userService.Register(user, ctx);
            if (user == null)
            {
                echo("非常抱歉，您的帐号创建失败了，请稍后再试");
                redirectUrl(getSavedReturnUrl());
                return;
            }
            //RegUtils.CheckUserSpace(user, ctx);

            // 好友处理
            //RegUtils.ProcessFriend(user, ctx);
            echo("您的帐号创建成功!");
            LoginTime expiration;
            if (ctx.PostIsCheck("RememberMe") == 1)
                expiration = LoginTime.Forever;
            else
                expiration = LoginTime.Never;

            loginService.Login(user, expiration, ctx.Ip, ctx);

            //echoToParent( lang( "loginok" ), getSavedReturnUrl() );
            echoRedirect(lang("loginok"), getSavedReturnUrl());

            //redirectUrl(getSavedReturnUrl());
        }

        private string QQlogin_get_user_info(string openid ,string access_token)
        {
            string user_info="";
            string call_back = "";
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create("https://graph.qq.com/user/get_user_info?access_token=" + access_token + "&oauth_consumer_key=" + apiid + "&openid=" + openid);
                // Get the response instance.
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream)
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("utf-8")))
                {
                    call_back = reader.ReadToEnd();
                }
            }
            catch
            {
                echo("对不起，QQlogin通讯失败");
                logger.Info("QQlogin通讯失败");
                return "-1";
            }
            try
            {
                Dictionary<String, object> openid_json = JSON.ToDictionary(call_back);
                foreach (KeyValuePair<string, object> item in openid_json)
                {
                    if (item.Key == "nickname") user_info = item.Value.ToString();
                }
            }
            catch
            {
                echo("对不起，没有获得可识别的openid");
                logger.Info("没有获得可识别的openid");
                return "-1";
            }
            return user_info;
        }

        private string add_share(string openid, string access_token,string url,string title,string site,string fromurl)
        {
            string result = "";
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create("https://graph.qq.com/user/get_user_info?access_token=" + access_token + "&oauth_consumer_key=" + apiid + "&openid=" + openid + "&url=" + url + "&title=" + title + "&site=" + site + "&fromurl=" + fromurl);
                // Get the response instance.
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream)
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("utf-8")))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch
            {
                echo("对不起，QQlogin通讯失败");
                logger.Info("QQlogin通讯失败");
                return "-1";
            }

            return result;
        }
    }

}
