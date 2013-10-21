	<div class="" style="padding:20px;">
		<table cellspacing="0" cellpadding="2" border="0" style="width: 100%;" id="dataAdminList" data-action="#{ActionLink}">
		<tr class="adminBar"><td colspan="3">	
			<span id="btnUnPick" class="btnCmd left10 " cmd="unpick">:{unRecommend}</span>
		</td>		
		<td style="text-align:right;padding-right:10px;"><div class="hide"><span id="btnDeleteTrue" class="btnCmd" cmd="deletetrue"><img src="~img/delete.gif" /> _{deleteTrue}</span></div></td>		
		</tr>
			<tr class="tableHeader">
				<td align="center" style="width:4%;"><input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" /></td>

				<td style="width:50%;">_{title}</td>
				<td align="center" style="width:9%;">_{view}/_{comment}</td>
				<td align="center" style="width:14%;">_{created}</td>
			</tr>
			<!-- BEGIN list -->
			<tr class="tableItems">
				<td align="center"><input name="selectThis" id="checkbox#{post.Id}" type="checkbox"> </td>
				<td class="">#{post.Status}<a href="#{post.Url}" target="_blank">#{post.Title}</a></td>
				<td align="center">#{post.Hits} <span class="note">|</span> #{post.ReplyCount}</td>
				<td align="center">#{post.CreateTime}</td>
			</tr>
			<!-- END list -->
			<tr>
				<td colspan="4" class="adminPage">#{page}</td>
			</tr>
		</table>
	</div>

