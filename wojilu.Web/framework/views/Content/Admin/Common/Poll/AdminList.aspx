<div>
	<table cellspacing="1" cellpadding="4" border="0" style="width:98%;margin:5px auto;" id="dataAdminList">
	    <tr class="adminBar"><td colspan="4"><img src="~img/list.gif" />&nbsp;投票管理
        </td></tr>
		<tr class="tableHeader">
			<td>_{orderId}</td>
			<td>_{title}</td>
			<td align="center">_{created}</td>
			<td align="center">_{admin}</td>
		</tr>
		<!-- BEGIN list -->
		<tr class="tableItems">
			<td>#{x.OrderId}</td>
			<td><a href="#{x.data.show}" target="_blank">#{x.Title}</a></td>
			<td align="center">#{x.Created}</td>
			<td align="center">
                <a href="#{x.data.edit}" class="">_{edit}</a>
			    <a href="#{x.data.delete}" class="deleteCmd">_{delete}</a>
			</td>
		</tr>
		<!-- END list -->
		<tr><td colspan="4">#{page}</td></tr>
	</table>
</div>
