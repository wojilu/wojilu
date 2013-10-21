
<div style="padding:50px 0px 30px 30px;width:580px;margin:auto;">

<style>
div.lgReginfo {padding:20px 0px 10px 20px;}
.lgTitle{font-size:14px;font-weight:bold; border-bottom:1px #ccc solid; margin-bottom:10px; padding:5px 10px;}

.loginTable { border:0px #ccc solid; border-collapse:collapse;  width:400px; margin:auto;}
.loginTable td{font-size:14px; padding:3px}
.loginTable .right {width:80px; padding-right2:3px; vertical-align:top;}
.loginTable .lgsubmit {padding:5px 10px 5px 0px}
.loginTable .lgInput, #validationRow input {width:180px;height2:20px;font-size:14px; padding:3px}
.validationRow input {width:120px;}
.loginTable .valid {font-size:12px}
</style>


<div class="lgTitle">重新发送激活邮件</div>
<table class="loginTable" border="0">
<tr>
<td>
<span class="red"><img src="~img/number/1.gif" /> 验证您的注册信息</span>
<span class="note" style="margin-left:30px;"><img src="~img/number/2.gif" /> 重发激活邮件</span>
</td>
</tr>
</table>


<form method="post" action="#{ActionLink}" class="ajaxPostForm" style="margin-top:30px;margin-bottom:30px;">
<table class="loginTable" border="0" style="background:#f2f2f2; ">
	<tr>
		<td class="right" style="padding-top:30px;">_{userName}</td>
		<td style="padding-top:30px;"><input name="regName" type="text" class="lgInput"  /> <span class="valid" msg="_{exPlsFill}"></span></td>
	</tr>
	<tr>
		<td class="right">_{pwd}</td>
		<td><input name="regPwd" type="password" class="lgInput" /> <span class="valid" msg="_{exPwd}"></span>     </td>
	</tr>
	<!-- BEGIN Captcha -->
	<tr id="validationRow">
	    <td class="right">_{validationCode}</td><td>#{ValidationCode}</td>
	</tr><!-- END Captcha -->

	<tr>
		<td></td><td class="lgsubmit" style="padding-bottom:30px;">
		<input type="submit" name="btnSubmit" class="btn" value="验证注册信息" id="btnSubmit"  />
		</td>
	</tr>
</table>
</form>

<script type="text/javascript">

_run( function() { 
	wojilu.ui.valid();
	wojilu.ui.ajaxPostForm();

});

</script>

</div>
