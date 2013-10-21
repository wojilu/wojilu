

<div style="margin:20px 20px;">
	<form method="post" action="#{ActionLink}">		
	<table style="width:95%; margin-bottom:10px; text-align:left;">
		<tr>
			<td style="width:60px;">_{name}</td>
			<td><input name="photoAlbum.Name" type="text" /></td>
		</tr>

		<tr>
			<td style=" vertical-align:top;">_{description}</td>
			<td><textarea name="photoAlbum.Description" style="height: 60px; width: 280px"></textarea></td>
		</tr>
		<tr>
			<td></td>
			<td>
			<input type="submit" value="_{submit}" class="btn" />
			<input name="Button1" type="button" value="_{cancel}" class="btn btnCancel" />
			</td>
		</tr>
	</table>
			
	</form>
</div>
