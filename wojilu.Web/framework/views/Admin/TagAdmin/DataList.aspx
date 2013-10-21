<style>
#tagResultsPanel { padding:10px 10px 15px 15px; margin:10px 10px 10px 0px; background:#fff; border2:1px #f2f2f2 solid;}
#tagDataList {margin:15px;}
#tagDataList li { list-style: disc; margin:5px 5px 5px 15px; font-size:14px;}
</style>

<div  style="padding:10px 10px 10px 10px;">
    <div>#{toolbar}</div>    
    
	<table cellspacing="0" cellpadding="4" border="0" style="width: 100%;" id="dataAdminList" data-action="#{OperationUrl}">
		    
		<tr class="adminBar"><td colspan="3" style="padding-left:10px;">
			<img src="~img/list.gif" /> 所有打上 <strong>"#{tagName}"</strong> 标签的数据</span>
		</td>		
		</tr>

		<tr class="tableHeader">
			<td style="width:10%;">数据类型</td>
			<td style="width:;">_{title}</td>
			<td style="width:30%;">_{admin}</td>
		</tr>
		<!-- BEGIN list -->
		<tr class="tableItems" id="data#{data.Id}">
			<td style="text-align2:center;">#{data.TypeName}</td>
			<td><a href="#{data.Link}" target="_blank">#{data.Title}</a></td>
			<td><a href="#{data.DeleteLink}" class="ajaxDeleteCmd" removeId="data#{data.Id}"><img src="~img/delete.gif" /> 删除tag</a></td>
		</tr>
		<!-- END list -->
		<tr>
			<td colspan="3" class="adminPage">#{page}</td>
		</tr>
	</table>
        
        
</div>