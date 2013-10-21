
<div>
<div class="adminMainTitle"><div class="adminSidebarTitleInternal">_{commonPage}</div></div>

<div class="adminMainPanel">
	<div class="formPanel2" style="background:#fff;">

	
	
	<table style="width:100%;">
	<tr>
	<td style="width:150px; vertical-align:top;" class="adminSidebar">
	
			<ul>
			
				<li style="margin-left:3px;"><a href="#{categoryLink}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/home.gif" /> _{pageTopicAdmin}</a></li>
				<li style="margin-top:10px;"><img src="~img/list.gif" /> _{pageTopicListing}</li>

					<ul style="margin:0px 0px 0px 0px;">
						<!-- BEGIN lists -->
						<li><a href="#{data.LinkByCategory}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/arrowRight.gif" />#{data.Name}</a></li>
						<!-- END lists -->
					</ul>
				
                
	
			</ul>
			
	
	
	</td>
	<td style="padding:0px; vertical-align:top;" id="tabMain">#{layout_content}</td>
	</tr>
	</table>
	
	</div>
	</div>
</div>
