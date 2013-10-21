
<div>
	<div></div>
	<table class="tabHeader" style="width:98%; font-size:12px;" cellpadding="0" cellspacing="0">
		<tr>
		    <td class="otherTab" style="width:5%">&nbsp;</td>
			<td class="tabCmd otherTab" style="width:20%"><a href="#{roleLink}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/list.gif" /> _{roleAndRank}</a></td>
			<td class="tabCmd otherTab" style="width:20%"><a href="#{frontPermission}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/addcategory.gif" /> _{frontSecuritySetting}</a></td>
			<td class="tabCmd otherTab" style="width:20%"><a href="#{backPermission}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/addcategory.gif" /> _{adminSecuritySetting}</a></td>

			
			<td class="otherTab" style="width:40%">&nbsp;</td>
		</tr>
	</table>
	
	<div class="tabMain" id="tabMain" style="width:98%; padding:20px;">
	#{layout_content}
	</div>

</div>
