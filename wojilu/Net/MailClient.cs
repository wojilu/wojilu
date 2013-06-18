/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace wojilu.Net {

    /// <summary>
    /// 邮件发送成功之后执行的方法
    /// </summary>
    public interface ISuccessCallback {

        /// <summary>
        /// 邮件发送成功之后执行的方法
        /// </summary>
        void SuccessRun();
    }

    /// <summary>
    /// 邮件发送服务(如果因为网络不通等原因发送失败，则会自动记录日志)
    /// </summary>
    /// <example>
    /// 使用说明
    /// <code>
    /// MailService.Init( "smtp.gmail.com", "aaa@gmail.com", "123456" )
    ///     .SetSender( "岳不群" ); // 此行(即发送人)可省略
    ///     .Send( "aaa@126.com", "岳老二的邮件标题", "此处内容，此处<strong style='color:red;font-size:36px;'>html部分</strong>"
    /// </code>
    /// </example>
    public class MailClient {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MailClient ) );

        private String _smtpUrl;
        private String _smtpUser;
        private String _smtpPwd;

        private String _senderName;

        private Boolean _enableSsl = true;
        private MailPriority _mailPriority = MailPriority.Normal;
        private Boolean _isBodyHtml = true;

        private ISuccessCallback sendSuccessCallback;

        public MailClient() {
        }

        /// <summary>
        /// 创建一个发送对象
        /// </summary>
        /// <param name="smtpUrl">smtp 地址</param>
        /// <param name="user">登录名</param>
        /// <param name="pwd">密码</param>
        public MailClient( String smtpUrl, String user, String pwd ) {
            Init( smtpUrl, user, pwd );
        }

        public static MailClient New() {
            return ObjectContext.Create<MailClient>();
        }

        /// <summary>
        /// 根据网站配置(site.config)中的smtp网址、用户名、密码等，进行初始化
        /// </summary>
        /// <returns></returns>
        public static MailClient Init() {
            MailClient mail = New();
            mail.SetSmtp( config.Instance.Site.SmtpUrl, config.Instance.Site.SmtpUser, config.Instance.Site.SmtpPwd );
            mail.EnableSsl( config.Instance.Site.SmtpEnableSsl );
            mail.SetSender( config.Instance.Site.SiteName );
            return mail;
        }

        /// <summary>
        /// 根据smtp网址、用户名、密码等，进行初始化
        /// </summary>
        /// <param name="smtpUrl">smtp 地址</param>
        /// <param name="user">登录名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static MailClient Init( String smtpUrl, String user, String pwd ) {
            return New().SetSmtp( smtpUrl, user, pwd );
        }

        /// <summary>
        /// 初始化 smtp 服务器、用户、密码
        /// </summary>
        /// <param name="smtpUrl">smtp 地址</param>
        /// <param name="user">登录名</param>
        /// <param name="pwd">密码</param>
        public virtual MailClient SetSmtp( String smtpUrl, String user, String pwd ) {
            _smtpUrl = smtpUrl;
            _smtpUser = user;
            _smtpPwd = pwd;
            return this;
        }

        /// <summary>
        /// 是否启用 ssl 链接(默认是启用的)
        /// </summary>
        /// <param name="isSsl"></param>
        public virtual MailClient EnableSsl( Boolean isSsl ) {
            _enableSsl = isSsl;
            return this;
        }

        /// <summary>
        /// 默认启用 html
        /// </summary>
        /// <param name="isHtml"></param>
        public virtual MailClient IsBodyHtml( Boolean isHtml ) {
            _isBodyHtml = isHtml;
            return this;
        }

        /// <summary>
        /// 设置高优先级
        /// </summary>
        public virtual MailClient PriorityHight() {
            _mailPriority = MailPriority.High;
            return this;
        }

        /// <summary>
        /// 设置低优先级
        /// </summary>
        public virtual MailClient PriorityLow() {
            _mailPriority = MailPriority.Low;
            return this;
        }

        /// <summary>
        /// 设置普通优先级
        /// </summary>
        public virtual MailClient PriorityNormal() {
            _mailPriority = MailPriority.Normal;
            return this;
        }

        /// <summary>
        /// 设置发送者名称
        /// </summary>
        /// <param name="name"></param>
        public virtual MailClient SetSender( String name ) {
            _senderName = name;
            return this;
        }

        /// <summary>
        /// 设置发送成功之后执行的方法
        /// </summary>
        /// <param name="action"></param>
        public virtual MailClient SuccessCallback( ISuccessCallback action ) {
            sendSuccessCallback = action;
            return this;
        }

        /// <summary>
        /// 发送 Email
        /// </summary>
        /// <param name="to">接收方的email</param>
        /// <param name="title">邮件标题</param>
        /// <param name="htmlBody">邮件内容</param>
        /// <returns>Result.IsValid是否成功</returns>
        public virtual Result Send( String to, String title, String htmlBody ) {

            if (strUtil.IsNullOrEmpty( _senderName )) _senderName = _smtpUser;

            MailAddress addrFrom = new MailAddress( _smtpUser, _senderName, Encoding.UTF8 );
            MailAddress addrTo = new MailAddress( to, to, Encoding.UTF8 );

            using (MailMessage message = new MailMessage( addrFrom, addrTo )) {

                message.Subject = title;
                message.SubjectEncoding = Encoding.UTF8;

                message.Body = htmlBody;
                message.BodyEncoding = Encoding.UTF8;

                message.IsBodyHtml = _isBodyHtml;
                message.Priority = _mailPriority;

                NetworkCredential credential = new NetworkCredential( _smtpUser, _smtpPwd );

                SmtpClient client = new SmtpClient( _smtpUrl );
                client.Credentials = credential;
                client.EnableSsl = _enableSsl;

                try {
                    client.Send( message );
                    if (sendSuccessCallback != null) {
                        sendSuccessCallback.SuccessRun();
                    }
                    return new Result();
                }
                catch (SmtpException ex) {
                    String info = "[" + "send mail to " + to + " : " + title + "] error : " + ex.ToString();
                    logger.Error( info );
                    return new Result( info );
                }

            }

        }



    }

}
