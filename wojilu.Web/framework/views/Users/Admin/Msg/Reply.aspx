
<div>

	<form id="dataForm" method="post" action="#{ActionLink}" class="ajaxPostForm">
		<table style="width:100%;">
			<tr>
				<td width="80">_{receiver}</td>
				<td>#{m.ToName}</td>
			</tr>
			<tr>
				<td>_{title}</td>
				<td><input name="Title" type="text" style="width:450px" value="#{m.Title}"> </td>
			</tr>
			<tr><td style="vertical-align:top">_{content}</td>
				<td>#{Editor}
				</td>
			</tr>
			<tr>
			    <td>&nbsp;</td>
				<td>
				<input type="hidden" name="MsgId" value="#{m.MsgId}" />
				<input name="Submit1" type="submit" id="btnSendMsg" value="_{send}" class="btn">&nbsp;
				<input name="Button1" type="button" value="_{return}" class="btnReturn" />
				&nbsp;<span id="lblInfo" class="note"></span></td>
			</tr>
		</table>
	</form>

</div>

