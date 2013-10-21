<div>
<div class="adminMainTitle"><div class="adminSidebarTitleInternal">_{sendConfirmEmail}</div></div>

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
				<td><input name="Title" style="width: 500px" type="text" value="#{title}"><span class="valid" mode="border"></span></td>
			</tr>
			<tr>
				<td style="vertical-align:top; font-weight:bold;">_{contentPreview}</td>
				<td style="padding:3px 10px; background:#fff; border:1px #ccc inset;">#{emailBody}
				</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td>
				<div style="margin-bottom:10px;">#{confirmTemplateTip}</div>
				<div>
				<input name="Submit1" type="submit" value="_{sendConfirmEmail}" class="btn">&nbsp;
				<input name="Button1" type="button" value="_{return}" class="btnReturn" />
				<input name="UserIds" type="hidden" value="#{userIds}" />
				</div>
				</td>
			</tr>
		</table>
	
		</form>	
	
	
	</div>


</div>

</div>