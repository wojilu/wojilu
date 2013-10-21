
<table cellspacing="1" cellpadding="4" border="0" style="width: 98%; margin:auto;" id="dataAdminList">
	<tr class="adminBar"><td colspan="5"><img src="~img/list.gif"/> :{forumLog}</td></tr>
	<tr class="tableHeader">
		<td align="center" style="width:8%;">_{orderId}</td>
		<td style="width:12%;">:{operator}</td>
		<td style="width:53%;">:{logInfo}</td>
		<td style="width:15%;">_{created}</td>
		<td align="center" style="width:12%;">IP</td>
	</tr>
	<!-- BEGIN list -->
	<tr class="tableItems">
		<td align="center">#{forumlog.Id} </td>
		<td>#{forumlog.UserName}</td>
		<td>#{forumlog.MsgInfo}</td>
		<td class="font12">#{forumlog.Created}</td>
		<td style="text-align:center">#{forumlog.Ip}</td>
	</tr>
	<!-- END list -->
	<tr>
		<td colspan="5" align="left" class="adminPage">#{page}</td>
	</tr>
</table>

