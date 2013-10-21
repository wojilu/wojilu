

<form method="post" action="#{ActionLink}">
<table style="width:90%; margin:15px;" border="0">
	<tr>
		<td style="width:100px">:{sectionName}</td>
		<td><input name="Title" type="text" style="width: 250px" value="#{section.Title}" class="tipInput" tip="_{exName}"><span class="valid" mode="border"></span></td>
	</tr>

	<tr>
		<td style="width:100px">SEO Keywords</td>
		<td><input name="MetaKeywords" type="text" style="width: 450px" value="#{section.MetaKeywords}" /></td>
	</tr>
	<tr>
		<td style="width:100px;vertical-align:top">SEO Description</td>
		<td style=""><textarea name="MetaDescription" style="width: 450px; height:50px;">#{section.MetaDescription}</textarea></td>
	</tr>

	<tr>
		<td style="vertical-align:top">:{moreLink}</td>
		<td><textarea name="MoreLink" style="width: 450px;height:60px;" >#{section.MoreLink}</textarea><br />
		<span class="note">:{sectionMoreLinkTip}</span>
		</td>
	</tr>

    <tr>
        <td style="width:80px;">
            :{listCount}:</td>
        <td>
            <input name="Count" type="text" style="width: 40px" value="#{count}" /></td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            <input id="Submit1" class="btn" type="submit" value="_{submit}" />&nbsp;
            <input id="Button1" class="btnCancel" type="button" value="_{cancel}" /></td>
    </tr>
</table>

</form>