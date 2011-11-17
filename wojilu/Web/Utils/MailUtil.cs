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
using System.Text;

using wojilu.Net;

namespace wojilu.Web.Utils {

    /// <summary>
    /// 网站的默认邮件工具，已经填充了网站的smtp网址和用户名、密码等
    /// </summary>
    public class MailUtil {

        public static MailService getMailService() {
            MailService mail = new MailService( config.Instance.Site.SmtpUrl, config.Instance.Site.SmtpUser, config.Instance.Site.SmtpPwd );
            mail.enableSsl( config.Instance.Site.SmtpEnableSsl );
            mail.setSender( config.Instance.Site.SiteName );
            return mail;
        }

    }

}
