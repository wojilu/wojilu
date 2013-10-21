		
	<div  style="padding:0px;">

<style>
.max100 {max-width:100px; max-height:100px;width:expression(width>100?"100px":width+"px");height:expression(height>100?"100px":height+"px");}

.picked {border:5px solid red;}
</style>

	
		<table cellspacing="0" cellpadding="2" border="0" style="width: 100%;" id="dataAdminList" data-action="#{ActionLink}">
			<tr class="adminBar">
				<td>
					<input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" />
					<span id="btnPick" class="btnCmd left10" cmd="pick"><img src="~img/pick.gif" /> _{recommend}</span>
					<span style="margin:0px 5px 0px 2px;">#{adminDropCategoryList}</span>
					<span id="btnDelete" class="btnCmd left20" cmd="delete"><img src="~img/delete.gif" /> _{toTrash}</span>
				</td>		
				<td style="text-align:right;padding-right:10px;"><div class="hide"><span id="btnDeleteTrue" class="btnCmd" cmd="deletetrue"><img src="~img/delete.gif" /> _{deleteTrue}</span></div></td>
			</tr>
			
			<tr>
			<td colspan="2">

				<!-- BEGIN list -->
				<div style=" float:left;width:120px;height:130px;margin:5px; padding:5px; text-align:center;color:#666; background:#fff">
				    <div>
					<input name="selectThis" id="checkbox#{photo.Id}" type="checkbox"><label for="checkbox#{photo.Id}">#{photo.SysCategoryName}</label><br/>
					<a href="#{photo.LinkShow}" target="_blank"><img src="#{photo.ImgThumbUrl}"  class="max100 #{photo.PickedClass}"/></a>
					</div>		
				</div>
				<!-- END list -->
				
				<div class="clear"></div>		
		
			</td>
			</tr>
		</table>
		
		<div>#{page}</div>
		
		
		
	</div>

