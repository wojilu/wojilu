<div style="padding:20px;">
		<form method="post" action="#{ActionLink}" class="">
		<table style="width: 500px;">
			<tr>
				<td>_{name}</td>
				<td><input name="Name" type="text" /></td>
			</tr>
			<tr>
				<td>_{description}</td>
				<td><textarea name="Description" style="height: 100px; width: 380px"></textarea></td>
			</tr>
			<tr>
				<td></td>
				<td>
				<input type="submit" value="_{submit}" class="btn" />&nbsp;
				<input name="Button1" type="button" class="btnCancel" value="_{cancel}" /></td>
			</tr>
		</table>
		</form>
</div>