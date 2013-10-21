
<table style="width: 100%; " cellpadding="4" id="dataAdminList" data-action="#{adminAction}">
	<tr class="adminBar">
		<td colspan="3">
		<span id="btnDelete" class="btnCmd" cmd="delete"><img src="~img/trash.gif" /> _{toTrash}</span>
		</td>
		<td colspan="2" style="text-align:right">
		<span id="btnDeleteTrue" class="btnCmd" cmd="deletetrue"><img src="~img/delete.gif" /> _{deleteTrue}</span>		
		</td>
	</tr>
	<tr class="tableHeader">
		<td align="center"><input id="selectAll" type="checkbox" title="_{checkAll}"/></td>
		<td>_{status}</td>
		<td>_{sender}</td>
		<td>_{mailTitle}</td>
		<td>_{receiveTime}</td>
	</tr>
	<!-- BEGIN list -->
	<tr class="tableItems">
		<td align="center"><input name="selectThis" id="checkbox#{m.Id}" type="checkbox"> </td>
		<td>#{m.StatusString}</td>
		<td>#{m.SenderName}</td>
		<td><a href="#{m.ReadUrl}">#{m.Title}</a> </td>
		<td>#{m.ReceiveTime}</td>
	</tr>
	<!-- END list -->
	<tr>
		<td colspan="5" align="left" class="adminPage">#{page}</td>
	</tr>
</table>
