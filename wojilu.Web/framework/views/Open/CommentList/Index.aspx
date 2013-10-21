<div class="" style="padding:10px;width:98%; margin:auto;">

<style>
.targetTitle { width:200px; overflow:hidden;  text-overflow:ellipsis; white-space:nowrap;}
</style>


	<table id="dataAdminList" class="dataAdminList" cellspacing="0" cellpadding="0" style="width:100%;" data-action="#{ActionLink}">
		<tr class="adminBar">
			<td colspan="4"><img src="~img/list.gif" /> 评论列表
            </td>
        </tr>
		<tr class="tableHeader">
		<td style="width:10%;">_{author}</td>
		<td style="width:40%;">_{content}</td>
		<td style="width:200px;">_{commentTarget}</td>
		<td style="width:19%;">_{created}</td>
		</tr>
		<!-- BEGIN list -->
		<tr class="tableItems">
		<td>#{c.Author}</td>
		<td><a href="#{c.Link}" target="_blank" class="link">#{c.sContent}</a></td>
		<td><div class="targetTitle">#{c.TargetTitle}</div></td>
		<td>#{c.Created}</td>
		</tr><!-- END list -->
			
		<tr>
			<td colspan="4" class="adminPage">#{page}</td>
		</tr>
		
	</table>
		
		
	</div>

