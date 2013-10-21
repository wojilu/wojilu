<div style="width:700px; padding:10px;">


<style>
.max150 { max-width:50px; max-height:50px; width:50px; height:50px;}
</style>

		<table cellspacing="0" cellpadding="2" border="0" style="width: 100%;" id="dataAdminList" data-action="#{ActionLink}">
			<tr class="adminBar"><td colspan="4">
				<img src="~img/list.gif" /> 图片列表	
			</td>		
			</tr>

            <tr class="tableHeader"><td>图片</td><td>帖子标题</td><td>创建时间</td><td>管理</td></tr>

            <!-- BEGIN list -->
            <tr class="tableItems"><td><a href="#{f.Url}" target="_blank"><img src="#{f.ImgUrl}" class="max150" /></a></td><td><a href="#{f.Url}" target="_blank">#{f.Title}</a></td><td>#{f.Created}</td>
            <td>
                <a href="#{f.EditLink}" class="frmBox" xwidth="580"><img src="~img/edit.gif" /> _{edit}</a>
                <a href="#{f.DeleteLink}" class="deleteCmd left10"><img src="~img/delete.gif" /> _{delete}</a>
            </td></tr>
            <!-- END list -->
			<tr>
				<td colspan="4" class="adminPage">#{page}</td>
			</tr>
		</table>



</div>
