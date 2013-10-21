<div style="margin:20px;">

<div style="font-size:18px; font-weight:bold; margin-bottom:20px;">_{resetPwd}</div>



	<div class="formPanel">

    <div></div>

<script type="text/javascript">
<!--
_run( function() {
	$('#btnMakePwd').click( function() {
		var pwd = Math.random()*100000000000 + "";
		pwd = pwd.substr( 0, 8 );
		$('#pwd1').val( pwd );
		$('#pwd2').val( pwd );
	});
});
//-->
</script>	
	
	
	
		<form action="#{ActionLink}" method="post">

		<table style="width: 100%">
			<tr>
				<td style="width:60px;vertical-align:top">_{user}</td>
				<td>#{userList}</td>
			</tr>
			<tr>
				<td>_{newPwd}</td>
				<td><input name="Pwd" id="pwd1" style="width: 200px" type="text"><span class="valid" to="Pwd" msg="_{exNotEmpty}"></span>&nbsp;
				<input name="btnMakePwd" id="btnMakePwd" type="button" value="_{useRandomPwd}" class="btns" />
				</td>
			</tr>
			<tr>
				<td>_{repeatNewPwd}</td>
				<td><input name="Pwd2" id="pwd2" style="width: 200px" type="text"><span class="valid" rule="same" target="Pwd" msg="_{requirePwdAndSame}"></span>&nbsp;</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td><input name="Submit1" type="submit" value="_{submitNewPwd}" class="btn">&nbsp;
				<input name="Button1" type="button" value="_{return}" class="btnReturn" />
				<input name="UserIds" type="hidden" value="#{userIds}" />
				<input name="chkSendEmail" type="checkbox" />_{sendNewPwdToUserEmail}
				
				</td>
			</tr>
		</table>
	
		</form>	
	
	
	</div>


</div>
