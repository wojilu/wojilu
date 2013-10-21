<div style="padding:10px;">
<div class="editPanel">
<h2 style="margin-bottom:5px;">_{customSkin} <a href="#{lnkList}" style="font-size:12px;font-weight:normal;text-decoration:underline;color:blue;margin-left:30px;">_{returnSkinList}</a></h2>
<div style="margin:0px 0px 20px 0px; color:#666;">_{editSkinCssNote}</div>


<form action="#{ActionLink}" method="post">

	<table style="width: 100%">
		<tr>
			<td style="width:20px;"></td>
			<td>_{skinName}<br/><input name="Name" type="text" style="width: 80%" maxlength="20" /></td>
		</tr>
		<tr>
			<td style="vertical-align:top"></td>
			<td>_{skinContent}<br/><textarea name="SkinContent" style="width:95%;height:500px;font-family:verdana;font-size:11px;">#{skinContent}</textarea></td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td><input name="Submit1" type="submit" value="_{createMySkin}" class="btn" /></td>
		</tr>
	</table>

</form>
</div>
</div>
