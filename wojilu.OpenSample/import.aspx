<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="import.aspx.cs" Inherits="wojilu.OpenSample.import" %>

<%@ Register src="Header.ascx" tagname="Header" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>导入用户</title>
</head>
<body>
    <form id="form1" runat="server">
    <div><uc1:Header ID="Header1" runat="server" /></div>
    <div style="padding:30px;">
    
        <div style="font-size:14px; font-weight:bold; margin:10px 0px;">导入用户到 wojilu 系统</div>
        
        <div style="color:#666;font-size:12px;">

        请每行输入一个用户的“<span style="color:Red; font-weight:bold;">用户名/密码/email</span>”(用户名和密码之间用斜线分割)，比如“<span style="color:blue; font-weight:bold;">张三/123456/zhsang@126.com</span>”，一行只能一个用户。
        </div>
        
        <div style="margin:10px 0px">    
        <asp:TextBox ID="txtBody" runat="server" Height="300px" TextMode="MultiLine" 
            Width="500px"></asp:TextBox>
        </div>
        
        <div>
            <asp:Button ID="btnSubmit" runat="server" Text="导入用户" 
                onclick="btnSubmit_Click" />        
        </div>
    
    </div>
    </form>
</body>
</html>
