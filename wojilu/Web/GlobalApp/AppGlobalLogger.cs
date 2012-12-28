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
using System.Collections.Generic;
using System.Text;
using System.Web;
using wojilu.Web.Mvc;
using System.Reflection;

namespace wojilu.Web.GlobalApp {

    /// <summary>
    /// 集中处理系统中所有的异常日志
    /// </summary>
    public class AppGlobalLogger : AppGlobalHelper {


        public AppGlobalLogger() {
        }

        public AppGlobalLogger( HttpApplication app ) {
            base.app = app;
        }

        public override void LogError( Boolean responseError ) {

            StringBuilder sb = getErrorInfo( app );

            try {
                logException( sb );
                addHttpStatus();
                String output = getOutput( getExInfo( sb, responseError ) );
                app.Response.Write( output );
            }
            catch {
                throw new Exception( sb.ToString() );
            }
        }

        private void logException( StringBuilder sb ) {

            ILog logger = LogManager.GetLogger( typeof( AppGlobalLogger ) );

            if (isLogError()) {
                logger.Error( sb.ToString() );
                LogManager.Flush();
            }
            else {
                logger.Info( sb.ToString() );
                LogManager.Flush();
            }
        }

        private String getOutput( String exInfo ) {
            String template = base.getXHtmlTemplate();
            template = template.Replace( "#{pageTitle}", config.Instance.Site.SiteName );
            template = template.Replace( "#{exTitle}", lang.get( "exPageError" ) );
            template = template.Replace( "#{navLink}", getOtherLink() );
            template = template.Replace( "#{exDetail}", exInfo );
            template = template.Replace( "#{exMsg}", ex.Message );

            template = template.Replace( "~js/", sys.Path.Js );
            template = template.Replace( "#{jsVersion}", MvcConfig.Instance.JsVersion );
            template = template.Replace( "#{langStr}", wojilu.lang.getLangString() );

            return template;
        }

        private string getExInfo( StringBuilder sb, bool responseError ) {
            if (responseError == false) return "";
            return sb.ToString().Replace( "\n", "<br/>" );
        }

        private string getOtherLink() {
            if (isFrame()) {
                return "";
            }
            else {
                return "<div style=\"margin:10px 20px;\"><a href=\"javascript:history.back();\">" + lang.get( "returnLast" ) + "</a> <a href=\"" + SystemInfo.SiteRoot + "\" style=\"margin-left:30px;\">" + lang.get( "siteHome" ) + "</a></div>";
            }
        }

        private Boolean isFrame() {
            return Params( "frm" ) != null && Params( "frm" ).Equals( "true" );
        }

        private Object Params( String key ) {
            return app.Request.Params[key];
        }

        public override void MailError() {
        }

        public override void ClearError() {
            app.Server.ClearError();
        }

    }

}
