<div style="padding:20px">
		
			<style>
			td.pwdLeftTd {	width:80px;}
			</style>
			
			<form method="post" action="#{ActionLink}" class="">
			<table style="width:500px">

				<tr>
					<td class="pwdLeftTd">_{pwd} <span class="red">*</span></td>
					<td><input name="Pwd" id="OldPwd" type="password" /></td>
				</tr>
				<tr>
					<td></td>
					<td style="height:10px;"></td>
				</tr>
				<tr>
					<td class="pwdLeftTd">Email <span class="red">*</span></td>
					<td><input name="Email" type="text" value="#{userEmail}" /></td>
				</tr>

				<tr>
                    <td></td>
					<td style="height:40px">
					    <input name="btnSubmit" id="pwdSubmit" type="submit" value="保存" class="btn"/>					
					</td>
				</tr>

			</table>
			</form>

</div>

