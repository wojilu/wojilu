<div class="formPanel" style="margin-left:10px; padding:5px 10px;">

	
	
	<div style="padding:10px 5px;" class="red">_{mailRequireTestSelfTip}</div>
	
	    <form action="#{EmailEnableLink}" method="post" class="ajaxPostForm">
		<table style="width: 100%">

			<tr>
				<td style="vertical-align:top;width:100px">开启邮件服务</td>
				<td>#{site.EnableEmail} <span class="note">比如发送激活邮件、在线发送email邀请、找回密码服务等；<br /><span class="red">如果你没有可靠的邮件发送服务器，建议不要开启。免费邮箱不推荐使用。</span></span></td>
			</tr>
		
			<tr>
				<td>&nbsp;</td>
				<td><input name="Submit1" type="submit" value="_{editSetting}" class="btn">&nbsp;</td>
			</tr>
			</table>
	    </form>
	
		<form action="#{ActionLink}" method="post" class="ajaxPostForm" style="margin-top:30px; margin-bottom:20px;">

		<table style="width: 100%">

			<tr>
				<td style="width:100px">_{mailSmtpServer}</td>
				<td><input name="SmtpUrl" type="text" value="#{site.SmtpUrl}" style="width: 250px">&nbsp;<span class="valid" msg="_{mailSmtpTip}" show="true"></span></td>
			</tr>
			<tr>
				<td>_{mailAccount}</td>
				<td><input name="SmtpUser" type="text" value="#{site.SmtpUser}" style="width: 250px">&nbsp;<span class="valid" msg="_{mailAccountTip}" show="true"></span></td>
			</tr>
			<tr>
				<td>_{mailPwd}</td>
				<td><input name="SmtpPwd" type="password" value="" style="width: 250px">&nbsp;<span class="valid" msg="_{exPlsFill}" rule="password"></span></td>
			</tr>
			<tr>
				<td>_{repeatPwd}</td>
				<td><input name="SmtpPwd2" value="" style="width: 250px" type="password">&nbsp;<span class="valid" msg="_{requirePwdAndSame}" rule="password2"></span></td>
			</tr>
			<tr>
				<td>_{isEnableSsl}</td>
				<td>#{site.CheckSmtpEnableSsl}</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td><input name="Submit1" type="submit" value="_{editSetting}" class="btn">&nbsp;</td>
			</tr>
		</table>
		</form>
	
	
	
	
	</div>	
