
<div style="padding:10px;">

<table cellspacing="0" cellpadding="5" border="0" style="width: 100%;" id="dataAdminList" data-action="#{OperationUrl}">
    <tr class="adminBar">
        <td colspan="4" style="padding-left:10px;">
            <img src="~img/list.gif" /> 等待审核的投递
        </td>
    </tr>

	<tr class="tableHeader">
	        <td>状态</td>
            <td style="width:60%">标题</td>
            <td>投递时间</td>
            <td>管理</td>
        </tr>
        <!-- BEGIN list -->
	<tr class="tableItems">
	        <td>#{post.StatusStr}</td>
            <td><a href="#{post.ShowLink}">#{post.Title}</a></td>
            <td>#{post.Created}</td>
            <td>
                <img src="~img/s/delete.png" /> <a href="#{post.DeleteLink}" class="deleteCmd">取消投递</a>
                &nbsp;</td>
        </tr><!-- END list -->
	<tr>
		<td colspan="4" class="adminPage">#{page}</td>
	</tr>

</table>



</div>

<style>
.waiting {color:;}
.unpass {color:Red;}
</style>
