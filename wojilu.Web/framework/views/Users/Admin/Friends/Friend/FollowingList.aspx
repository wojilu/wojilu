
<div style="padding:10px;">

<style>
.fItem {width: 285px; height:138px; float:left; display:inline; margin:5px 10px 5px 3px; border-radius:3px; box-shadow:2px 3px 9px #ccc;}
.fPic {border-radius:4px;width:100px; height:100px;}
</style>

<table style="width: 100%">
<tr>
	<td>
				
		<!-- BEGIN list -->
			<table class="fItem cborder" cellpadding="4">
			<tr>
				<td style="width:80px; text-align:center"><a href="#{m.UrlFull}" target="_blank"><img src="#{m.FaceFull}" class="avm"></a><br/>
				</td>
				<td style="vertical-align:top;">
					<div>
					    #{m.Name}<br />
						_{regTime}: #{m.CreateTime}<br/>
						_{lastLoginTime}: #{m.LastLoginTime}<br/>
						<span href="#{m.DeleteFollowingLink}" class="deleteCmd"><img src="~img/delete.gif" /> _{delete}</span>
					</div>
				</td>
			</tr>
		</table>
		<!-- END list -->
		<div class="clear"></div>
		<div class="page" style="margin-top:15px;">#{page}</div>
				
	</td>
	<td style="background:#;width:250px;">
	
	</td>
</tr>
</table>

</div>

