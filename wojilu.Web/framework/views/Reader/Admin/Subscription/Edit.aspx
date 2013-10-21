
<h3>:{editChannel}</h3>
<div class="formPanel2">

<form method="post" action="#{ActionLink}">
<table style="width: 95%" >
	<tr>
		<td>:{oName}</td>
		<td>#{feed.Title}</td>
	</tr>
	<tr>
		<td>:{rssUrl}
		</td>
		<td>#{feed.RssLink}</td>
	</tr>
	<tr><td></td>
		<td><span class="note">(:{editRssUrlTip})</span></td>
	</tr>
	<tr>
		<td colspan="2">&nbsp;</td>
	</tr>
	<tr>
		<td>:{customName}</td>
		<td>
		<input name="Name" type="text" style="width: 400px;" value="#{feed.Name}"></td>
	</tr>
	<tr>
		<td>_{category}</td>
		<td>#{CategoryId}</td>
	</tr>
	<tr>
		<td></td>
		<td><input name="Submit1" type="submit" value=":{editChannel}" class="btn">
		<input name="Button1" type="button" class="btnReturn" value="_{return}" />
		</td>
	</tr>
</table>
</form>

