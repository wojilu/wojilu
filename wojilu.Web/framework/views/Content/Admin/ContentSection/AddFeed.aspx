<div style="margin:10px;">

	<form action="#{ActionLink}" method="post" class="ajaxPostForm">
	
	<table style="width:750px" class="tabHeader" cellpadding="0" cellspacing="0">
		<tr>
			<td class="currentTab" style="width:25%;">:{addRss}</td>
			<td class="otherTab" style="width:25%;">&nbsp;</td>
			<td class="otherTab" style="width:25%;">&nbsp;</td>
			<td class="otherTab" style="width:25%;">&nbsp;</td>
		</tr>
	</table>
	
	<div class="tabMain" style="width:750px;height:200px;">
		<div style="padding:20px">
		<table style="width:99%;">
			<tr>
				<td style="width:80px;">:{rssUrl}</td>
				<td><input name="Url" type="text" style="width:550px;" class="tipInput" tip="_{exUrl}" /><span class="valid" mode="border"></span></td>
			</tr>
			<tr>
				<td>:{rssCount}</td>
				<td><input name="Count" type="text" value="5" style="width:90px" class="tipInput" tip="_{exCount}"><span class="valid" mode="border"></span></td>
			</tr>
			<tr>
				<td>:{rssName}</td>
				<td><input name="Title" type="text"> <span class="note">(_{optional})</span></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td style="padding:10px 0px;">
					<input name="Submit1" type="submit" value=":{addRss}" class="btn" />
					<input type="button" value="_{cancel}" class="btnCancel" />
				</td>
			</tr>
		</table></div>
	</div>
	
	</form>

</div>