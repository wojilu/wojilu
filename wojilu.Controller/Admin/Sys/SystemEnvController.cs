/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using wojilu.Web.Mvc;
using Microsoft.Win32;

namespace wojilu.Web.Controller.Admin.Sys {


    public class SystemEnvController : ControllerBase {

        public void Index() {
            StringBuilder builder = new StringBuilder();
            //builder.Append( "<div style='margin:10px;padding:10px;'>" );
            //builder.Append( "计算机名：" );
            //builder.Append( "http://" + ctx.context.Host + ctx.context.ApplicationPath + "<br />" );
            //builder.Append( "IP地址：" );
            //builder.Append( ctx.Request().ServerVariables["LOCAl_ADDR"] + "<br />" );
            //builder.Append( "域名：" );
            //builder.Append( ctx.Request().ServerVariables["SERVER_NAME"].ToString() + "<br />" );
            //builder.Append( "端口：" );
            //builder.Append( ctx.Request().ServerVariables["Server_Port"].ToString() + "<br />" );
            //builder.Append( "本文件所在路径：" );
            //builder.Append( ctx.Request().PhysicalApplicationPath + "<br />" );
            //builder.Append( "操作系统：" );
            //builder.Append( Environment.OSVersion.ToString() + "<br />" );
            //builder.Append( "操作系统所在文件夹：" );
            //builder.Append( Environment.SystemDirectory.ToString() + "<br />" );
            //builder.Append( "脚本超时时间：" );
            ////builder.Append( ((ctx.context.Server.ScriptTimeout / 1000)).ToString() + "秒<br />" );
            //builder.Append( "系统语言：" );
            //builder.Append( CultureInfo.InstalledUICulture.EnglishName + "<br />" );
            //builder.Append( ".NET版本：" );
            //builder.Append( string.Concat( new object[] { Environment.Version.Major, ".", Environment.Version.Minor, Environment.Version.Build, ".", Environment.Version.Revision } ) + "<br />" );
            //builder.Append( "IE版本：" );
            //RegistryKey key = Registry.LocalMachine.OpenSubKey( @"SOFTWARE\Microsoft\Internet Explorer\Version Vector" );
            //builder.Append( key.GetValue( "IE", "未检测到" ).ToString() + "<br />" );
            //builder.Append( "启动到现在已运行：" );
            //builder.Append( (((Environment.TickCount / 1000) / 60)).ToString() + "分钟<br />" );
            //builder.Append( "逻辑驱动器：" );
            //string[] logicalDrives = Directory.GetLogicalDrives();
            //for (int i = 0; i < (Directory.GetLogicalDrives().Length - 1); i++) {
            //    builder.Append( logicalDrives[i].ToString() );
            //}
            //builder.Append( "<br />" );
            //builder.Append( "CPU 数量：" );
            //builder.Append( Environment.GetEnvironmentVariable( "NUMBER_OF_PROCESSORS" ).ToString() + "<br />" );
            //builder.Append( "CPU类型：" );
            //builder.Append( Environment.GetEnvironmentVariable( "PROCESSOR_IDENTIFIER" ).ToString() + "<br />" );
            //builder.Append( "ASP.NET所站内存：" );
            //builder.Append( ((((double)Process.GetCurrentProcess().WorkingSet64) / 1048576.0)).ToString( "N2" ) + "M<br />" );
            //builder.Append( "ASP.NET所占CPU：" );
            //builder.Append( Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds.ToString( "N0" ) + "%<br />" );
            //builder.Append( "当前系统用户名：" );
            //builder.Append( Environment.UserName + "<br />" );
            //builder.Append( "</div>" );
            base.content( builder.ToString() );
        }
    }
}

