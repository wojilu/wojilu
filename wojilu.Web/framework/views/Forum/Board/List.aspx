<table class="forumList">
	<tr class="fbHeader">
        <td colspan="2" class="fbCategory">
            <div class="forum-category-arrow"></div>
            <div class="board-category-name"><a href="#{forumCategory.Url}">#{forumCategory.Name}</a></div>
        </td>
		<td class="fbtopics">:{topic}</td>
		<td class="fbposts">:{post}</td>
		<td class="fblastupdate">:{lastUpdate}</td>
	</tr>
	<!-- BEGIN forum -->
	<tr class="forumBoard">
		<td class="fbIcon"><img class="fbStatus" src="#{statusImg}" /></td>
		<td class="fbInfo" style="border-left:0px;">
			<div class="fbTitle"><a href="#{f.Url}">#{f.Name}</a>#{f.TodayPosts}</div>
			<div class="fbDescription">#{f.Description}</div>
			<div class="fbModerator">#{f.Moderator}</div>
		</td>
		<td class="fbTopics">#{f.Topics}</td>
		<td class="fbPosts">#{f.Posts}</td>
		<td class="fbLastUpdate">#{f.UpdateInfo}</td>
	</tr>
	<!-- END forum -->
</table>