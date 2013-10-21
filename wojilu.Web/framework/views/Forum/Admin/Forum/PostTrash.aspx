

		<table class="tabHeader" style="width:96%; font-size:12px;" cellpadding="0" cellspacing="0">
		    <td class="otherTab" style="width:3%">&nbsp;</td>
			<td class="otherTab" style="width:16%"><a href="#{topicTrashLink}">:{topicList}</a></td>
			<td class="currentTab" style="width:16%"><a href="#{postTrashLink}">:{postList}</td>
			<td class="otherTab">&nbsp;</td>
		</table>
		
		<div class="tabMain" style="width:96%">
		<div style="padding:10px;">
			
			<table cellspacing="1" cellpadding="4" style="width: 100%;" id="dataAdminList" data-action="#{ActionLink}">
				<tr class="adminBar"><td colspan="7">
				_{op}: 
				<span id="btnRestore" class="btnCmd" cmd="restore"><img src="~img/reply.gif" /> _{restore}</span>
			
				<span id="btnDeleteTrue" class="btnCmd left10" cmd="deletetrue"><img src="~img/delete.gif" /> _{deleteTrue}</span>
				
				</td></tr>
				<tr class="tableHeader">
					<td align="center">
					<input id="selectAll" type="checkbox" title="_{checkAll}" /></td>
					<td align="center">:{boardId}</td>
					<td>:{itsTopic}</td>
					<td align="center">_{author}</td>
					<td>_{title}</td>
					<td align="center">_{created}</td>
					<td align="center">IP</td>
				</tr>
				<!-- BEGIN list -->
				<tr class="tableItems">
					<td align="center"><input name="selectThis" id="checkbox#{post.Id}" type="checkbox" class="selectSingle"> </td>
					<td align="center">#{post.ForumBoardName}</td>
					<td class="strong">#{post.TopicTitle}</td>
					<td align="center"><a href="#{post.MemberUrl}">#{post.MemberName}</a></td>
					<td><a href="#{post.LinkShow}">#{post.Title}</a></td>
					<td align="center">#{post.Created}</td>
					<td align="center">#{post.Ip}</td>
				</tr>
				<!-- END list -->
				<tr>
					<td colspan="7" align="left" class="adminPage">#{page}</td>
				</tr>
			</table>
		
		</div>
		</div>


