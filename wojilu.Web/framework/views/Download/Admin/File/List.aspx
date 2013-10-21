<div>

	<table id="dataAdminList" data-action="#{ActionLink}" border="0" cellpadding="2" cellspacing="0" style="width: 98%; margin:10px auto;">
		<tr class="adminBar">
			<td colspan="12">
                <span><a href="#{addLink}"><img src="~img/add.gif" /> 添加文件</a></span> 
                <span class="left10"><a href="#{lnkCateShow}"><img src="~img/list.gif" /> 分类浏览</a></span>
            </td>
		</tr>
		<tr class="tableHeader">
			<td style="width:">_{id}</td>
			<td>分类</td>
			<td>_{name}</td>
			<td>大小</td>
			<td>版本</td>
			<td>评级</td>
			<td style="width:">_{view}</td>
			<td style="width:">_{comment}</td>
			<td>下载次数</td>
			<td>_{created}</td>
			<td>预览图</td>
			<td>_{admin}</td>
		</tr>
		<!-- BEGIN list -->
		<tr class="tableItems click2" href="#{data.LinkEdit}">
			<td>#{data.Id}</td>
			<td>#{data.CategoryName} - #{data.SubCategoryName}</td>
			<td><a href="#"><strong>#{data.Title}</strong></a></td>
			<td>#{data.SizeMB} MB</td>
			<td>#{data.Version}</td>
			<td>#{data.RankStar}</td>
			<td>#{data.Hits}</td>
			<td>#{data.Replies}</td>
			<td>#{data.Downloads}</td>
			<td>#{data.Created}</td>
			<td><a href="#{data.PreviewPicLink}" class="frmBox"><img src="~img/s/upload.png" /> 预览图</a></td>
			<td>
				<a href="#{data.LinkEdit}"><img src="~img/edit.gif" /> _{edit}</a>					
				<span href="#{data.LinkDelete}" class="deleteCmd left10"><img src="~img/delete.gif" /> _{delete}</span>
			</td>
		</tr>
		<!-- END list -->
		<tr>
		    <td colspan="12" class="adminPage">#{page}</td>
	    </tr>

	</table>

</div>
