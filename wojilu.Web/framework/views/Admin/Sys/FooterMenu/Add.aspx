<div style="padding:20px;width:580px;">

	<form method="post" action="#{ActionLink}">				
	
		<table style="width: 100%">
			<tr>
				<td style="width:60px;" class="right">_{name}</td>
				<td><input name="footerMenu.Name" type="text" /></td>
			</tr>
			<tr>
				<td class="right">_{link}</td>
				<td><input name="footerMenu.Link" type="text" style="width:520px;"></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td><input name="Submit1" type="submit" value="_{submit}"  class="btn"/> 
				<input name="Button1" type="button" value="_{cancel}" class="btnCancel" /></td>
			</tr>
		</table>
	
	</form>

</div>