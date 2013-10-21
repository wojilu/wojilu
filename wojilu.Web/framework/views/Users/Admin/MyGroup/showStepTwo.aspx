

	<table class="tabHeader" style=" width:96%;" cellpadding="0" cellspacing="0">
		<tr>
		    <td class="otherTab" style="width:5%;"></td>
			<td class="otherTab" style="width:20%;">_{createGroup1}</td>
			<td class="currentTab" style="width:20%">_{createGroup2}</td>
			<td class="otherTab" style="width:20%">_{createGroup3}</td>
			<td class="otherTab" style="width:35%">&nbsp;</td>
		</tr>
	</table>
	<div class="tabMain" style=" width:96%;padding:10px" >
		<form enctype="multipart/form-data" method="post" action="#{ActionLink}">
		<div style="width:500px;border:1px #ffe222 solid; background:#fffbe2; padding:10px 20px; margin:20px 0px; font-size:12px;">_{createGroupOkMsg}</div>
		
		<table style="width:500px; margin-bottom:50px;">
			<tr>
				<td><input name="File1" type="file" style="width: 350px;" /><br/>
				<span class="note">_{groupLogoNote}</span> </td>
			</tr>
			<tr>
				<td><input name="Submit1" type="submit" value="_{uploadPic}" class="btn">
				<input type="hidden" name="newGroupId" value="#{newGroupId}" />
				</td>
			</tr>
		</table>
		</form>
	</div>

