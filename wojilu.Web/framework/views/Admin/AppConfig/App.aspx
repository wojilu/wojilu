
<div class="formPanel" style="margin:10px;">
	
	
		<form action="#{ActionLink}" method="post" class="ajaxPostForm">
		<table style="width: 100%">			
			
			<tr>
				<td style="width:130px;vertical-align:top; font-weight2:bold;">用户注册时开启的app</td>
				<td>#{initApp}<br /><span class="note">(用户注册之后，默认将拥有的app)</span></td>
			</tr>
			<tr><td></td><td></td></tr>
			
			<tr>
				<td>&nbsp;</td>
				<td><input name="Submit1" type="submit" value="_{editSetting}" class="btn">&nbsp;</td>
			</tr>
		</table>
		</form>

</div>



<div class="" style="margin:10px;">

	<table cellspacing="0" cellpadding="2" border="0" style="width: 100%;" id="dataAdminList" data-action="#{ActionLink}">
		<tr class="adminBar"><td colspan="6">
			<span class="left10 btnCmd" cmd="disable"><img src="~img/list.gif" /> 应用程序部署</span>
		</td>		
		</tr>
		<tr class="tableHeader">
			<td align="center" style="width:4%;"><input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" /></td>
			<td style="width:9%;">启用状态</td>
			<td>名称</td>
			<td style="width:40%;">简介</td>
			<td>安装主题</td>
			<td align="center">_{admin}</td>
		</tr>
		<!-- BEGIN list -->
		<tr class="tableItems">
			<td align="center"><input name="selectThis" id="checkbox#{app.Id}" type="checkbox" class="selectSingle"> </td>
			<td class="appStatus">#{app.StatusName}</td>
			<td class="appName"><img src="#{app.LogoImg}" style="width:36px;" /> <strong>#{app.Name}</strong></td>
			<td><span class="note">#{app.Description}</span></td>
			<td>#{app.ThemeInfo} <a href="#{app.ThemeAdminLink}" class="frmBox left5">#{app.ThemeAdmin}</a></td>
			<td align="center"><a href="#{app.StatusAdminLink}" class="frmBox"><img src="~img/edit.gif" /> #{app.StatusAdminName}</a></td>
		</tr>
		<!-- END list -->
	</table>
</div>
<div class="" style="margin-left:10px;margin-right:10px; margin-bottom:50px;">

	<table cellspacing="0" cellpadding="2" border="0" style="width: 100%;" class="dataAdminList" data-action="#{ActionLink}">
		<tr class="adminBar"><td colspan="4">
			<span class="left10 btnCmd" cmd="disable"><img src="~img/list.gif" /> 基础组件部署</span>
		</td>		
		</tr>
		<tr class="tableHeader">
			<td align="center" style="width:4%;"><input id="Checkbox1" class="selectAll" type="checkbox" title="_{checkAll}" /></td>
			<td style="width:8%;">启用状态</td>
			<td>名称</td>
			<td align="center" style="width:9%;">_{status}</td>
		</tr>
		<!-- BEGIN clist -->
		<tr class="tableItems">
			<td align="center"><input name="selectThis" id="checkbox2" type="checkbox" class="selectSingle"> </td>
			<td class="componentStatus">#{c.StatusName}</td>
			<td><strong>#{c.Name}</strong></td>
			<td align="center"><a href="#{c.AdminLink}" class="frmBox"><img src="~img/edit.gif" /> 修改</a></td>
		</tr><!-- END clist -->
		

	</table>	
</div>	

<script>
_run( function() {

    $('.appStatus').each( function() {
        if( $(this).text()=='已禁用' ) {
            $(this).css( 'color', 'red' ).css( 'font-weight', 'bold' );            
            //$('.appName', $(this).parent() ).css( 'color', 'red' ).css( 'font-weight', 'bold' );
            //$(this).parent().css( 'background', '#ffe45c' );
        }
    });  
    
    $('.componentStatus').each( function() {
        if( $(this).text()=='禁用' ) {
            $(this).css( 'color', 'red' ).css( 'font-weight', 'bold' );            
        }
    });  
    
});
</script>	