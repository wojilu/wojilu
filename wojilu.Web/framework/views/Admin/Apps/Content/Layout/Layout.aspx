<div class="mainBody" style="padding:3px;">
	<div style="padding:15px 20px; font-size:14px; font-weight:bold;color:#3764A0"><img src="~img/app/m/wojilu.Apps.Content.Domain.ContentApp.png" class="right10" />所有用户的文章</div>
	<table class="tabHeader" style="width:98%; font-size:12px;" cellpadding="0" cellspacing="0">
		<tr>
		    <td class="otherTab" style="width:5%">&nbsp;</td>
			<td class="tabCmd otherTab" style="width:20%"><a href="#{listLink}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/list.gif" /> :{allPosts}</a></td>
			<td class="tabCmd otherTab" style="width:20%"><a href="#{commentLink}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/talk.gif" /> _{comment}</a></td>
			<td class="tabCmd otherTab" style="width:20%"><a href="#{trashLink}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/trash.gif" /> _{trash}(#{trashCount})</a></td>
			
			<td class="otherTab" style="width:35%">&nbsp;</td>
		</tr>
	</table>
	
	<div class="tabMain" id="tabMain" style="width:98%;">
	#{layout_content}
	</div>

</div>
