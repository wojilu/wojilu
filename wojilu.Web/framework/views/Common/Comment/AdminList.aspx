<style type="text/css">
.deleteCmd { background:url(~img/delete.gif) no-repeat left center; padding-left:13px; margin-left:5px}
</style>

<div class="" style="padding:10px;width:98%; margin:auto;">
		
		<table id="dataAdminList" cellspacing="0" cellpadding="4" border="0" style="width:95%;margin:10px auto;">
		    <tr class="adminBar">                <td colspan="4"><img src="~img/s/reply.png" />&nbsp;_{allComments}</td>
		    </tr>
			<tr class="tableHeader">
				<td style="width:80px;">_{author}</td>
				<td>_{content}</td>
				<td>_{created}</td>
				<td>_{admin}</td>
			</tr>
			<!-- BEGIN comment -->
			<tr class="tableItems">
				<td>#{c.Author}</td>
				<td><a href="#{c.Url}">#{c.Content}</a></td>
				<td>#{c.Created} </td>
				<td><a href="#{c.ReplyUrl}">_{reply}</a> <span href="#{c.DeleteUrl}" class="deleteCmd">_{delete}</span></td>
			</tr>
			<!-- END comment -->	
			<tr>
			    <td colspan="4"><div>#{page}</div></td>
			</tr>
		</table>

</div>
