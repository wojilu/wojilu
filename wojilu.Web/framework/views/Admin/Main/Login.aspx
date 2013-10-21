<!DOCTYPE html>
<html lang="zh-CN">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>_{userLogin}</title>
<script>var __funcList = []; var _run = function (aFunc) { __funcList.push(aFunc); }; var require = { urlArgs: 'v=#{jsVersion}' };</script>

<style>
body {margin:0px; padding:0px; font-size:14px;}
#loginWrap {width:550px;height:326px; margin:30px auto;padding:0px; background:url("~img/admin/login.jpg") no-repeat; }
#loginWrap form { padding-top:50px;}

#loginTable {margin:0px 0px 0px 180px; line-height:150%; }
#loginTable input.txt {border:1px #aaa inset;width:180px;height:22px;padding:2px 5px;}
.tdL {width:80px; text-align:right;color:#0d4070; color:#fff; }

h1 {font-size:28px;font-family:Microsoft YaHei;margin-bottom:20px;color:#0d4070; color:#fff;}

.btn {padding:0px 10px;font-size: 14px; height: 24px; cursor:pointer; }
.btn {color:#fff;border-top:1px #ccc solid;border-right:1px #799048 solid;border-bottom:1px #799048 solid;border-left:1px #ccc solid;background:#b1d464; }

#lnkHome {margin-right:10px; margin-bottom:5px; font-size:11px;text-decoration:none;color:#05507f;}

</style>
</head>

<body>
<div id="loginWrap">
<form action="#{ActionLink}" method="post" class="ajaxPostForm">

	<table id="loginTable">
        <tr>
            <td></td>
            <td><h1>_{userLogin}</h1></td>
        </tr>
		<tr>
			<td class="tdL">_{userName}</td>
			<td><input name="Name" type="text" class="txt" id="txtUserName" /></td>
		</tr>
		<tr>
			<td class="tdL">_{pwd}</td>
			<td><input name="Password1" type="password" class="txt" /></td>
		</tr>
		<tr>
			<td colspan="2" style="padding-left:86px; padding-top:10px">
			<input name="Submit1" type="submit" class="btn" value="_{userLogin}" />

            <a href="~/" id="lnkHome" target="_blank" style="display:none;">_{siteHome}</a>
            <input name="returnUrl" type="hidden" value="#{returnUrl}"/>

			</td>
		</tr>
	</table>
	
</form>
</div>

<script data-main="~js/main" src="~js/lib/require-jquery-wojilu.js?v=#{jsVersion}"></script>
<script>require(["wojilu._admin"])</script>
<script>require(["wojilu._nolayout"]);</script>
<script>
    _run(function () {
        $('#txtUserName').focus();
    });
</script>
</body>

</html>