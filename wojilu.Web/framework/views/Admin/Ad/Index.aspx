
<style>
tr.stopped { color:Red;}
</style>

	<div class="adminMainTitle"><div class="adminSidebarTitleInternal">广告管理</div></div>

	<div class="adminMainPanel">
	


		<table id="dataAdminList" border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
			<tr class="adminBar">
				<td colspan="5">
				
				    <span class="menuMore" list="adMenu"><img src="~img/add.gif" /> 添加广告 <img src="/static/img/downWhite.gif" />
				        <ul class="menuItems" id="adMenu" style="width:280px;">
				            <!-- BEGIN adcategories -->
				            <li><a href="#{item.AddUrl}" class="frmBox" title="添加：#{item.Name}">添加：#{item.Name}</a></li>
				            <!-- END adcategories -->
				        </ul>				    
				    </span>
				    
				    <span class="menuMore left20" list="listMenu"><img src="~img/list.gif" /> 分类浏览 <img src="/static/img/downWhite.gif" />
				        <ul class="menuItems" id="listMenu" style="width:230px;">
				            <li><a href="#{item.IndexUrl}">_{all}</a></li>
				            <!-- BEGIN listcat -->
				            <li><a href="#{item.ListUrl}">#{item.Name}</a></li>
				            <!-- END listcat -->
				        </ul>				    
				    </span>
				    
				</td>
			</tr>
			<tr class="tableHeader">
				<td>&nbsp;广告类型</td>
				<td>状态</td>
				<td>_{name}</td>
				<td>创建者</td>
				<td>_{admin}</td>
			</tr>
			<!-- BEGIN list -->
			<tr class="tableItems #{item.StatusClass}">

				<td>#{item.CategoryName}</td>
				<td>#{item.StatusStr}</td>
				<td><strong>#{item.Name}</strong></td>
				<td>#{item.CreatorName}</td>
				<td>
				<a href="#{item.LinkEdit}" class="frmBox">修改</a>
				<a href="#{item.LinkStop}" class="postCmd left10">#{item.StopCmd}</a>
				<a href="#{item.LinkDelete}" class="deleteCmd left10">删除</a>
				</td>
			</tr>
			<!-- END list -->
			<tr><td colspan="5">#{page}</td></tr>
		</table>


</div>

