<div class="hforumList clearfix">
	<div class="hfbHeader">
		<div style="float:left; margin-left:48px;">
            <img src="~static/skin/site/new/forum-category-arrow.jpg" />
        </div>
		<div class="board-category-name"><a href="#{forumCategory.Url}">#{forumCategory.Name}</a></div>			
	</div>
	<div class="hforumBoard">
		<div class="hfbContainer">
		    <!-- BEGIN forum -->
		    <table class="hforumBoardOne">
			    <tr>
				    <td rowspan="3" class="hfbLogo"><img class="fbStatus" src="#{statusImg}" /></td>
				    <td><div class="fbTitle"><a href="#{f.Url}" class="strong">#{f.Name}</a>#{f.TodayPosts}</div></td>
			    </tr>
			    <tr>
				    <td>:{topic}:#{f.Topics} <span class="left5">:{post}:#{f.Posts}</span></td>
			    </tr>
			    <tr>
				    <td>:{lastUpdate}:#{f.Updated}</td>
			    </tr>
		    </table>
		    <!-- END forum -->
		    <div style="clear:both;"></div>
		</div>
	</div>
</div>