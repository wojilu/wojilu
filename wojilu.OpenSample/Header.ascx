<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="wojilu.OpenSample.Header" %>

<style>
body {font-size:14px; line-height:150%; font-family:Verdana;}

a { text-decoration:none;}

#header {
border-radius:5px; color:#333;
background: rgb(0,183,234); /* Old browsers */
background: -moz-linear-gradient(top, rgba(0,183,234,1) 0%, rgba(0,158,195,1) 100%); /* FF3.6+ */
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(0,183,234,1)), color-stop(100%,rgba(0,158,195,1))); /* Chrome,Safari4+ */
background: -webkit-linear-gradient(top, rgba(0,183,234,1) 0%,rgba(0,158,195,1) 100%); /* Chrome10+,Safari5.1+ */
background: -o-linear-gradient(top, rgba(0,183,234,1) 0%,rgba(0,158,195,1) 100%); /* Opera11.10+ */
background: -ms-linear-gradient(top, rgba(0,183,234,1) 0%,rgba(0,158,195,1) 100%); /* IE10+ */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#00b7ea', endColorstr='#009ec3',GradientType=0 ); /* IE6-9 */
background: linear-gradient(top, rgba(0,183,234,1) 0%,rgba(0,158,195,1) 100%); /* W3C */
}
</style>

    <div id="header">
        <div style="padding:30px;">
        
            <h1 style="margin:0px 0px 20px 0px;padding:0px;font-size:22px;font-weight:bold;font-family:微软雅黑;">wojilu 网站集成测试</h1>
        
            <div>
                <a href="default.aspx">本站首页</a>
                <a href="import.aspx" style="margin-left:20px;">导入用户</a>
                <a href="http://bbs.usertest.com" style="margin-left:20px;">我记录网站综合系统首页</a>
            </div>
            
        </div>
    </div>