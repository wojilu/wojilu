
<div>


<table cellspacing="0" cellpadding="4" border="0" style="width:100%;margin:5px auto;" id="dataAdminList" data-action="#{OperationUrl}">

    <tr class="adminBar">
        <td colspan="4"><span class="btnCmd left10" cmd="delete"><img src="~img/delete.gif" /> 删除数据</span>
        <span class="left15">#{refreshCmd}</span>
        &nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>

	<tr class="tableHeader">
	    <td align="center" style="width:4%;"><input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" /></td>
		<td>_{id}</td>
		<td></td>
		<td>_{title}</td>
		<td align="center">_{created}</td>
		<td align="center">_{admin}</td>
	</tr>
	<!-- BEGIN list -->
	<tr class="tableItems">
	    <td align="center"><input name="selectThis" id="checkbox#{post.Id}" type="checkbox" class="selectSingle"> </td>
		<td>#{post.Id}</td>
		<td><a href="#{post.CategoryLink}">#{post.Category}</a></td>
		<td><a href="#{post.Link}">#{post.Title}</a>
		 
		</td>
		<td align="center">#{post.Created}</td>
		<td align="center">
		    <a href="#{post.EditUrl}" class="left5">_{edit}</a>
		</td>
	</tr>
	<!-- END list -->
	<tr>
	    <td colspan="6">#{page}</td>
	</tr>
</table>
</div>


