

<table cellspacing="0" cellpadding="2" border="0" style="width: 98%;margin:10px auto;" id="dataAdminList" >

	<tr class="adminBar"><td colspan="4">
	<span><a href="#{addLink}"><img src="~img/list.gif" /> :{addLink}</a></span>
	</td>
	</tr>


	<tr class="tableHeader">
		<td style="width:10%;">_{orderId}</td>
		<td style="width:25%;">:{linkName}</td>
		<td style="width:50%;">:{linkUrl}</td>
		<td style="width:15%;">_{admin}</th>
	</tr>
	<!-- BEGIN list -->
	<tr class="tableItems">
		<td>#{roll.OrderId}</td>
		<td>#{roll.Name}</td>
		<td><a href="#{roll.Link}">#{roll.Link}</a></td>
		<td class="underline"><a href="#{roll.EditUrl}" class="edit">_{edit}</a>
		
		<a href="#{roll.DeleteUrl}" class="deleteCmd left10">_{delete}</a></td>
	</tr>
	<!-- END list -->
</table>


