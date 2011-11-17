<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="wojilu.OpenSample._default" %>

<%@ Register src="Header.ascx" tagname="Header" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>wojilu 网站集成测试</title>
</head>
<body>
    <form id="form1" runat="server">
    
            <uc1:Header ID="Header1" runat="server" />

    
    
    <div style="padding:30px;">

    
        <div style="padding:10px; background:#f2f2f2;">这里是欢迎信息：<asp:Label ID="lblWelcome" runat="server" Font-Bold="True" 
                ForeColor="Red" Text="您好，您尚未登录"></asp:Label>  
                <span style="margin-left:20px;"><asp:HyperLink ID="lnkLogin" runat="server" 
                NavigateUrl="login.aspx">登录系统</asp:HyperLink></span>
                <span style="margin-left:10px;"><asp:HyperLink ID="lnkRegister" 
                runat="server" NavigateUrl="register.aspx"><span>注册</span></asp:HyperLink>
            </span>
            
            <span style="margin-left:20px;">
            <asp:LinkButton ID="lnkLogout" runat="server" onclick="lnkLogout_Click">注销</asp:LinkButton></span>
            
            
            
            
        </div>
        
        <div style="margin:20px 0px;">
       
        </div>
        
        <div>
            <div style="margin:10px 0px; font-size:14px; font-weight:bold;">示例：论坛帖子调用——</div>
            <ul>
            <asp:Repeater ID="rptForumTopic" runat="server">
            <ItemTemplate>
            <li><a href="http://bbs.usertest.com/Forum<%#Eval("AppId") %>/Topic/<%#Eval("Id") %>.aspx"><%#Eval("Title") %></a></li>
            </ItemTemplate>
            </asp:Repeater>
            </ul>
        </div>
    
    </div>
    </form>
</body>
</html>
