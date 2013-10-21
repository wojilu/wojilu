<style>


#register{ border:0px red solid; border-collapse:collapse;  width:580px; margin-top:20px;}
#register td{font-size:12px; padding:3px;height:20px;}
#register .right {width:80px; padding-right:5px}
#register .lgTitle{padding:15px 10px 10px 0px; font-size:28px;font-weight:bold;}
#register .lgsubmit {padding:15px 10px 10px 0px}
#register .lgInput {width:180px;height:20px;font-size:16px; padding:3px}
#register .valid {font-size:12px; color:#aaa}

#refreshImg {color:blue;text-decoration:underline;cursor:pointer;}
#lbl-Gender0 {width:28px;}
</style>

<div style="padding:10px 0px 30px 30px; border:0px solid blue;">



<div style="float:left;width:580px;">
<form method="post" action="#{ActionLink}" class="registerForm">
	<table id="register">
	<tr><td></td>
		<td class="lgTitle">_{userReg}</td>
	</tr>
		<tr>
			<td class="right"><strong>Email</strong></td>
			<td><input name="Email" type="text" value="#{m.Email}"> <span class="valid" msg="_{emailNote}" show="true" rule="email" ajaxAction="#{isEmailValidLink}"></span></td>
		</tr>
		<tr>
			<td class="right">_{userName}</td>
			<td><input name="Name" id="userName" type="text" value="#{m.Name}"> <span class="valid" msg="#{userNameNote}" show="true" rule="name_cn" ajaxAction="#{isNameValidLink}"></span>
			</td>
		</tr>

        <!-- BEGIN friendlyUrl -->
		<tr>
			<td class="right">_{friendlyUrl}</td>
			<td><span class="note">#{userUrlPrefix}</span><input name="FriendUrl" type="text" value="#{m.FriendUrl}" style="width:80px; height:15px;"> <span class="note">#{urlExt}</span>
			<span class="valid" to="FriendUrl" msg="_{furlNote}" show="true" rule="name" ajaxAction="#{isUrlValidLink}"></span>
			</td>
		</tr><!-- END friendlyUrl -->
		
        <!-- BEGIN subDomainFriendlyUrl -->
		<tr>
			<td class="right">_{friendlyUrl}</td>
			<td><input name="FriendUrl" type="text" value="#{m.FriendUrl}" style="width:80px;"><span class="note">.#{userUrlPrefix}</span>
			<span class="valid" to="FriendUrl" msg="_{furlNote}" show="true" rule="name" ajaxAction="#{isUrlValidLink}"></span>
			</td>
		</tr><!-- END subDomainFriendlyUrl -->
		
		<tr><td colspan="2" style="height:1px;"></td></tr>
		<!-- BEGIN Captcha -->
		<tr id="validationRow">
		    <td class="right">_{validationCode}</td><td>#{ValidationCode}</td>
		</tr><!-- END Captcha -->
		
		<tr>
			<td class="right">_{pwd}</td>
			<td><input name="Password1" type="password" value="#{m.Password1}"> <span class="valid" msg="_{pwdNote}" show="true" rule="password"></span></td>
		</tr>
		<tr>
			<td class="right">_{confirmPwd}</td>
			<td><input name="Password2" type="password" value="#{m.Password2}"> <span class="valid" msg="_{confirmPwdNote}" show="true" rule="password2"></span></td>
		</tr>
		<tr>
			<td class="right">_{gender}</td>
			<td><div class="inline reg-gender">#{m.Gender}</div></td>
		</tr>
		<tr><td></td><td class="lgsubmit">
		<input name="btnRegister" type="submit" class="btn btn-primary" value="_{submitReg}">
		<input name="Reset1" type="reset" value="_{regReset}" class="btn btnReset left10" id="btnReset" />
		</td></tr>
		<tr><td></td><td><label for="readLow" style="color:#aaa;">_{readAndAgree} <a href="/agreement.html" style="text-decoration:underline;" target="_blank">_{regService}</a></label></td></tr>
	</table>
	
</form>
</div>

<div style="float:left;width:250px; height:350px; margin-top:30px;" class="alert alert-warning hide1">

	<div class="" style="margin:0px 0px;width:100%; margin-top:20px;">
	<ul style="padding-left:0px; margin-left:0px;">
	    <li style="font-size:18px; font-weight:bold; font-family:Microsoft YaHei; list-style:none; margin-left:0px; padding-left:0px;">#{loginTip}</li>
	    <!-- BEGIN sendConfirmEmail -->
	    <li style="list-style-type:disc;  margin-top:10px; margin-left:30px;">已注册，但尚未收到激活邮件，请 <a href="#{resendLink}" style="">点击此处</a> 重发激活邮件</li>
	    <!-- END sendConfirmEmail -->
	</ul>

    <div>#{connectLogin}</div>

	</div>
</div>

<script type="text/javascript">

_run( function() {
	wojilu.ui.valid();
	var vimg = $('#validationRow img');
	$('#btnReset').click( function() {wojilu.tool.refreshImg(vimg);} );
	

});

</script>

<div style="clear:both;"></div>

</div>
