<div style="margin:20px;">
<form method="post" action="#{ActionLink}">
	
<table class="">
	<tr>
		<td style="width:100px">:{sectionName}</td>
		<td><input name="Title" type="text" style="width: 250px" value="#{section.Title}" class="tipInput" tip="_{exName}"><span class="valid" mode="border"></span></td>
	</tr>
	<tr>
		<td style="vertical-align:top">:{moreLink}</td>
		<td><textarea name="MoreLink" style="width: 450px;height:60px;" >#{section.MoreLink}</textarea><br />
		<span class="note">:{sectionMoreLinkTip}</span>
		</td>
	</tr>
	<tr>
	    <td>其他配置</td>
	    <td>#{settingList}</td>	
	</tr>

	<tr>
		<td>&nbsp;</td>
		<td style="padding-top:10px">
		    <input name="Submit1" type="submit" value=":{editSection}" class="btn">
		    <input type="button" value="_{cancel}" class="btnCancel" />
		</td>
	</tr>
</table>

</form>

</div>
