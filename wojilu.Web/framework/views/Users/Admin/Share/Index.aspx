<style type="text/css">
.feedItemBody img {float:left;margin:5px;}
.feedTypes {float:right; margin-right:20px;}
.feedTypes li{float:left; list-style:none; margin-left:5px;}

.userHomeSidebar h4 {font-weight:bold; color:#666;}
</style>

<div style="font-weight:bold; font-size:14px; margin:20px 10px 15px 25px;"><img src="~img/app/m/share.png" /> _{share}</div>     

<div style="margin:5px 15px;">
<table style="width: 100%;">
	<tr>
		<td style="width:70%;vertical-align:top;padding-right:20px;">
		
		
<script>
    _run(function () {
        wojilu.tool.makeTab('.tabList', 'currentTab', '');
    });
</script>
    

<ul class="tabList">
    <li class="firstTab" style="width:120px;"><a href="#{shareLink}" >好友的分享</a><span></span></li>
    <li style="width:120px;"><a href="#{myShareLink}">我的分享</a><span></span></li>

</ul>

<div class="tabMain" style="width:100%">					

			<!-- BEGIN list -->
			<table style="width: 100%; border-top:0px #fff solid; margin:5px 10px 5px 0px;">
				<tr>
					<td rowspan="2" style="width:60px;vertical-align:top;padding-top:5px;"><a href="#{share.UserLink}"><img src="#{share.UserFace}" /></a></td>
					<td style="height:30px;vertical-align:top"><img src="~img/app/s/#{share.DataType}.gif"/> #{share.Title} <span class="note left10">#{share.Created}</span> 
					<span class="commentCmd link" parentId="0" rootId="#{share.Id}">_{reply}</span>
                    <span class="deleteCmd" href="#{share.DeleteLink}">_{delete}</span>
					</td>
				</tr>
				<tr>
					<td style="vertical-align:top" class="feedItemBody">#{share.Body}#{share.BodyGeneral}	#{commentList}
					</td>
				</tr>
			</table>
			<div style="border-top:1px #ccc solid; margin-left:0px;"></div>
			<!-- END list -->
            #{commentForm}
            <style>#commentForm{ display:none;}</style>

			<div style="margin:10px 20px;">#{page}</div>
</div>		
		
		</td>
		<td style="vertical-align:top;width:30%; padding-left:10px;" class="userHomeSidebar">
		
		    <div style="margin:10px 0px;" class="strong">现在分享</div>
		
		    <div style="margin:10px 0px 30px 0px;">
		        <form method="post" action="#{ActionLink}" class="ajaxPostForm">
		            <div>分享(网址/视频/mp3/flash)：</div>
		            <div><input name="shareLink" type="text" style="width:230px;" /></div>
		            <div style="margin-top:5px;">简介：</div>
		            <div><textarea name="shareDescription" style="width:230px;height:60px;"></textarea></div>
		            <div style="margin-top:5px;"><input type="submit" value="_{share}" class="btn btns" /></div>
			    
		        </form>			    
		    </div>		
		
            <div><strong>我的好友</strong></div>            
			<ul class="userAvatar">
				<!-- BEGIN shareFriends --><li>
				<a href="#{user.ShareLink}"><img src="#{user.Face}"/></a>
				<div><a href="#{user.ShareLink}">#{user.Name}</a></div></li>
				<!-- END shareFriends -->		
			</ul>
			<div class="clear"></div>
            
            <div style="margin-top:10px"><strong>我关注的人</strong></div>
			<ul class="userAvatar">
				<!-- BEGIN shareFollowing --><li>
				<a href="#{user.ShareLink}"><img src="#{user.Face}"/></a>
				<div><a href="#{user.ShareLink}">#{user.Name}</a></div></li>
				<!-- END shareFollowing -->		
			</ul>
			<div class="clear"></div>
		
		</td>
	</tr>
</table>

</div>
