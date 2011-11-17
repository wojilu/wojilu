<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="wojilu.OpenSample.login" %>

<%@ Register src="Header.ascx" tagname="Header" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>用户登录</title>
    <style type="text/css">
.loginWrap {

background:#eee; border-radius:4px; padding:20px;width:380px; box-shadow:0px 0px 10px #aaa;

background: rgb(255,255,255); /* Old browsers */
background: -moz-linear-gradient(top, rgba(255,255,255,1) 0%, rgba(246,246,246,1) 47%, rgba(237,237,237,1) 100%); /* FF3.6+ */
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(255,255,255,1)), color-stop(47%,rgba(246,246,246,1)), color-stop(100%,rgba(237,237,237,1))); /* Chrome,Safari4+ */
background: -webkit-linear-gradient(top, rgba(255,255,255,1) 0%,rgba(246,246,246,1) 47%,rgba(237,237,237,1) 100%); /* Chrome10+,Safari5.1+ */
background: -o-linear-gradient(top, rgba(255,255,255,1) 0%,rgba(246,246,246,1) 47%,rgba(237,237,237,1) 100%); /* Opera11.10+ */
background: -ms-linear-gradient(top, rgba(255,255,255,1) 0%,rgba(246,246,246,1) 47%,rgba(237,237,237,1) 100%); /* IE10+ */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ededed',GradientType=0 ); /* IE6-9 */
background: linear-gradient(top, rgba(255,255,255,1) 0%,rgba(246,246,246,1) 47%,rgba(237,237,237,1) 100%); /* W3C */
    
}
.loginTable {width:320px;}
.loginTable td { padding:3px 2px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <div><uc1:Header ID="Header1" runat="server" /></div>
    
    <div style="padding:30px;">
    
        <div>
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        </div>

        <div class="loginWrap">
        <table class="loginTable" border="0" style="" cellspacing="0" cellpadding="0">
            <tr>
                <td style="text-align:right;">
                    用户名</td>
                <td>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    密&nbsp;&nbsp;码</td>
                <td>
                    <asp:TextBox ID="txtPwd" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="登录系统" 
                        onclick="btnSubmit_Click" />
                        
                        <a href="default.aspx" style="margin-left:15px;">返回首页</a>
                </td>
            </tr>
        </table>
        </div>
        
    </div>
    </form>
</body>
</html>
