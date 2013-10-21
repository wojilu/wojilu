	<div class="" style="padding:5px 20px;">
		<table cellspacing="0" cellpadding="2" border="0" style="width: 100%;" id="dataAdminList" data-action="#{ActionLink}">
		<tr class="adminBar"><td colspan="4">
				<span class="left10 btnCmd" cmd="undelete"><img src="~img/s/restore.png" /> _{fromTrash}</span>

		</td>		
		<td colspan="2" style="text-align:right;padding-right:10px;"><div><span id="btnDeleteTrue" class="btnCmd" cmd="deletetrue"><img src="~img/delete.gif" /> _{deleteTrue}</span></div></td>		
		</tr>
			<tr class="tableHeader">
				<td align="center" style="width:4%;"><input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" /></td>
				<td>_{author}</td>
				<td style="width:50%;">_{title}</td>
				<td align="center" style="width:9%;">_{view}/_{comment}</td>
				<td align="center" style="width:14%;">_{created}</td>
				<td align="center" style="width:9%;">_{admin}</td>
			</tr>
			<!-- BEGIN list -->
			<tr class="tableItems">
				<td align="center"><input name="selectThis" id="checkbox#{post.Id}" type="checkbox"> </td>
				<td><a href="#{post.UserLink}" target="_blank">#{post.UserName}</a></td>
				<td class=""><a href="#{post.Url}" target="_blank">#{post.Title}</a></td>
				<td align="center">#{post.Hits} <span class="note">|</span> #{post.ReplyCount}</td>
				<td align="center">#{post.CreateTime}</td>
				<td align="center" class="underline"><span href="#{post.UnDeleteLink}" class="putCmd">_{restore}</span></td>
			</tr>
			<!-- END list -->
			<tr>
				<td colspan="6" class="adminPage">#{page}</td>
			</tr>
		</table>
	</div>

