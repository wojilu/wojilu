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
    /// MailService mail = new MailService( "smtp.gmail.com", "aaa@gmail.com", "123456" );
    /// mail.setSender( "岳不群" ); // 此行(即发送人)可省略
    /// mail.send( "aaa@126.com", "岳老二的邮件标题", "此处内容，此处<strong style='color:red;font-size:36px;'>html部分</strong>"
    /// </code>
    /// </example>
    public class MailService {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MailService ) );

        private String _smtpUrl;
        private String _smtpUser;
        private String _smtpPwd;

        private String _senderName;

        private Boolean _enableSsl = true;
        private MailPriority _mailPriority = MailPriority.Normal;
        private Boolean _isBodyHtml = true;


        private ISuccessCallback sendSuccessCallback;

        /// <summary>
        /// 创建一个发送对象
        /// </summary>
        /// <param name="smtpUrl">smtp 地址</param>
        /// <param name="user">登录名</param>
        /// <param name="pwd">密码</param>
        public MailService( String smtpUrl, String user, String pwd ) {
            _smtpUrl = smtpUrl;
            _smtpUser = user;
            _smtpPwd = pwd;
        }

        /// <summary>
        /// 是否启用 ssl 链接(默认是启用的)
        /// </summary>
        /// <param name="isSsl"></param>
        public void enableSsl( Boolean isSsl ) {
            _enableSsl = isSsl;
        }

        /// <summary>
        /// 默认启用 html
        /// </summary>
        /// <param name="isHtml"></param>
        public void isBodyHtml( Boolean isHtml ) {
            _isBodyHtml = isHtml;
        }

        /// <summary>
        /// 设置高优先级
        /// </summary>
        public void priorityHight() {
            _mailPriority = MailPriority.High;
        }

        /// <summary>
        /// 设置低优先级
        /// </summary>
        public void priorityLow() {
            _mailPriority = MailPriority.Low;
        }

        /// <summary>
        /// 设置普通优先级
        /// </summary>
        public void priorityNormal() {
            _mailPriority = MailPriority.Normal;
        }

        /// <summary>
        /// 设置发送者名称
        /// </summary>
        /// <param name="name"></param>
        public void setSender( String name ) {
            _senderName = name;
        }

        /// <summary>
        /// 设置发送成功之后执行的方法
        /// </summary>
        /// <param name="action"></param>
        public void successCallback( ISuccessCallback action ) {
            sendSuccessCallback = action;
        }

        /// <summary>
        /// 发送方法
        /// </summary>
        /// <param name="to">接收方的email</param>
        /// <param name="title">邮件标题</param>
        /// <param name="htmlBody">邮件内容</param>
        /// <returns>是否成功</returns>
        public Boolean send( String to, String title, String htmlBody ) {

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
                    if (sendSuccessCallback != null)
                        sendSuccessCallback.SuccessRun();
                    return true;
                }
                catch (SmtpException ex) {
                    String info = "send mail to " + to + " : " + title;
                    logger.Error( "[" + info + "] error : " + ex.ToString() );
                    return false;
                }

            }

        }



    }

}
