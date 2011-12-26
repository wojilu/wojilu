/****************************************************************************
 * 功能说明：
 *  1、使用 HttpModel与现有基于共享登录信息( Cookie )的网站进行集成
 *  2、用户自动登录，自动注册，，延时注册，
 *  3、同步退出
 * 
 * 使用方法：
 *  见示例web.config
 * 
 *  设计编码：shiningrise@gmail.com
 *  
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using wojilu.Open;

namespace wojilu.AuthenticationModule
{
    public class AuthenticationModule : System.Web.IHttpModule
    {
        public void Dispose()
        {
            //         throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(Application_BeginRequest);
            context.EndRequest += new EventHandler(Application_EndRequest);
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            HttpRequest req = context.Request;

            //定义起来测试用
            HttpResponse response = context.Response;

            OpenService openService = new OpenService();
            if (req.IsAuthenticated)
            {
                if (openService.UserIsLogin(context) != true)
                {
                    string username = context.User.Identity.Name;

                    //           response.Write("username=" + username);

                    if (!openService.UserIsRegister(username))
                    {
                        string userPwd = GetRandomNumberString(16);
                        string userEmail = string.Format("{0}@lcsyzx.cn", username);
                        openService.UserRegister(username, userPwd, userEmail, null, "home,blog,photo,microblog,friend,visitor,forumpost,about,feedback,share");
                    }
                    openService.UserLogin(username, context);
                    this.UpdateUserProfile(username);
                }
            }
            else
            {
                //         response.Write(" IsAuthenticated = false ");
                //        openService.UserLogout(context);
            }
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        public void Application_EndRequest(object sender, EventArgs e)
        {
            //HttpApplication application = sender as HttpApplication;
            //HttpContext context = application.Context;
            //HttpResponse response = context.Response;

            ////    response.Write("这是来自自定义HttpModule中有EndRequest");
            //response.Write(config.Instance.Site.ValidateUserByMembership.ToString() );
        }

        /// <summary>
        /// 更新 Profile 的真实名到 wojilu
        /// </summary>
        /// <param name="usr"></param>
        private void UpdateUserProfile(string username)
        {
            OpenService openService = new OpenService();
            UserProfile userProfile = new UserProfile();//(UserProfile)ProfileBase.Create(usr.UserName, true);
            userProfile.Initialize(username, true);
            wojilu.Members.Users.Domain.User usr = openService.getUserByName(username);
            //UserName  LoginUserName  FullName  UserGroup  CurJi  CurBh  CurBnbh
            if (userProfile.UserGroup == "学生")
                usr.Title = string.Format("{0}({1})班{2}", userProfile.CurJi, userProfile.CurBh, userProfile.FullName);
            else if (userProfile.UserGroup == "教师")
                usr.Title = userProfile.FullName;
            else
                usr.Title = string.Format("{0}{1}", userProfile.UserGroup, userProfile.FullName);

            usr.RealName = userProfile.FullName;

            //if (username == "admin")
            //    openService.UserChangePwd("admin", "admin");
            //else
                usr.Pwd = GetRandomNumberString(6);

            usr.update();
            //    db.update(usr, "RealName");
            //    db.update(usr, "Pwd");
        }

        /// <summary>
        /// 生成随机数字字符串
        /// </summary>
        /// <param name="int_NumberLength">数字长度</param>
        /// <returns></returns>
        private string GetRandomNumberString(int int_NumberLength)
        {
            bool onlyNumber = false;
            Random random = new Random();
            string strings = "123456789";
            if (!onlyNumber) strings += "abcdefghjkmnpqrstuvwxyz";
            char[] chars = strings.ToCharArray();
            string returnCode = string.Empty;
            for (int i = 0; i < int_NumberLength; i++)
                returnCode += chars[random.Next(0, chars.Length)].ToString();
            return returnCode;
        }
    }
}
