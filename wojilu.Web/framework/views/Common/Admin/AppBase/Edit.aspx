
<div style="padding:20px;">


<form method="post" action="#{ActionLink}">

<table>

	<tr>
		<td>_{appName} </td>
		<td>
		<div class="moduleEdit_typeName highlight">#{appInfo.Type}</div>
		</td>
	</tr>

	<tr>
		<td>_{description}</td>
		<td>
		<div class="moduleEdit_description note">#{appInfo.Description}</div>
		</td>
	</tr>
	<tr>
		<td width="120">&nbsp;</td>
		<td>
		&nbsp;</td>
	</tr>
	<tr>
		<td width="120">_{appFriendlyName}</td>
		<td><input name="Name" type="text" value="#{app.Name}" /></td>
	</tr>


	<tr><td></td>
		<td style="padding:10px 0px;">
			<input name="Submit1" type="submit" value="_{editApp}" class="btn">
&nbsp;&nbsp;&nbsp; <input type="button" class="btnCancel" value="_{cancel}"><br/>
			


		</td>
	</tr>
</table>
		
</form>


</div>
