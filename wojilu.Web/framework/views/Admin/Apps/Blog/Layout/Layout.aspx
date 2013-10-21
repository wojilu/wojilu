
<div class="mainBody" style="padding:3px;">
	<div style="padding:15px 20px; font-size:14px; font-weight:bold;color:#3764A0"><img src="~img/app/m/wojilu.Apps.Blog.Domain.BlogApp.png" class="right10" />所有用户的博客</div>
	<table class="tabHeader" style="width:98%; font-size:12px;" cellpadding="0" cellspacing="0">
		<tr>
		    <td class="otherTab" style="width:5%">&nbsp;</td>
			<td class="tabCmd otherTab" style="width:16%"><a href="#{listLink}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/list.gif" /> :{newBlog}</a>
            </td>


			<td class="tabCmd otherTab" style="width:16%"><a href="#{pickedLink}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/users.gif" /> :{recommendBlog}</a></td>
			<td class="tabCmd otherTab" style="width:16%"><a href="#{commentLink}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/talk.gif" /> _{comment}</a></td>
			<td class="tabCmd otherTab" style="width:16%"><a href="#{trashLink}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/trash.gif" /> _{trash}(#{trashCount})</a></td>
            <td class="tabCmd otherTab" style="width:16%"><a href="#{categoryLink}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/tools.gif" /> 系统分类管理</a></td>
		
			
			<td class="otherTab" style="width:15%">&nbsp;</td>
		</tr>
	</table>
	
	<div class="tabMain" id="tabMain" style="width:98%; ">
	#{layout_content}
	</div>

</div>

