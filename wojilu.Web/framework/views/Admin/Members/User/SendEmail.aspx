<div>
<div class="adminMainTitle"><div class="adminSidebarTitleInternal">_{sendEmail}</div></div>

<div class="adminMainPanel">

	<div class="formPanel">
	
		<form action="#{ActionLink}" method="post" class="ajaxPostForm">

		<table style="width: 100%">
			<tr>
				<td style="width:60px;vertical-align:top">_{receiver}</td>
				<td>#{userList}</td>
			</tr>
			<tr>
				<td>_{title}</td>
				<td><input name="Title" style="width: 500px" type="text"><span class="valid" mode="border"></span></td>
			</tr>
			<tr>
				<td style="vertical-align:top">_{content}</td>
				<td>
<script type="text/plain" id="MsgBody" name="MsgBody"></script>
<script>
    _run(function () {
        wojilu.editor.bind('MsgBody').height(300).line(1).show();
    });
</script>
<span class="valid border" to="MsgBody"></span>

				</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td><input name="Submit1" type="submit" value="_{sendEmail}" class="btn">&nbsp;
				<input name="Button1" type="button" value="_{return}" class="btnReturn" />
				<input name="UserIds" type="hidden" value="#{userIds}" />
				</td>
			</tr>
		</table>
	
		</form>	
	
	
	</div>


</div>

</div>