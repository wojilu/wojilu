
<style type="text/css">
.feedItemBody a {margin:5px 5px; vertical-align:top;}
.feedItemBody img {max-width:100px; max-height:100px;width:expression(width>100?"100px":width+"px");height:expression(height>100?"100px":height+"px");}
.feedTypes {float:right; margin-right:20px;}
.feedTypes li{float:left; list-style:none; margin-left:5px;}
.userHomeSidebar h4 {font-size:14px; color:#333333;}
</style>

<div class="adminMainPanel">

<div class="formPanel" style="background:#fff; border:0px;padding:0px;">
	<div style="clear:both"></div>			

	<table id="feedWrap" style="width: 100%" cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td style="width:75%;vertical-align:top;padding-right:10px;">
			    
			    <table style="width:95%;margin:10px; ">
			        <tr>
			            <td style="width:60px; display:none"><a href="#{user.Link}"><img src="#{user.PicSmall}" style="padding:3px; border:1px #ccc solid;" class="avs" /></a></td>
			            <td>#{publish}</td>
			        </tr>
			    </table>
									
	
				<table style=" width:100%">
					<tr>
						<td style="font-size:14px;font-weight:bold; padding-left:10px;">
						    <div class="frmUpdate" href="#{friendsFrame}" loadTo="friendsShare">_{friendFeeds}<img src="~img/down.gif"/></div>
						</td>
						<td style="font-size:12px;font-weight:bold">
						    <div class="frmUpdate" href="#{followingFrame}" loadTo="followingShare">_{feedsOfFollowing}<img src="~img/down.gif"/></div>
						</td>
				
						
						<td><a href="#{user.MyFeedLink}" style="font-weight:bold">_{myFeeds}</a></td>
						<td>
							<ul class="feedTypes">
								<li class="hide"><a href="#{feedListLink}"><img src="~img/list.gif" /> _{all}</a></li>
								<li><a href="#{blogFeedsLink}"><img src="~img/app/s/wojilu.Apps.Blog.Domain.BlogPost.gif" /> _{blog}</a></li>
								<li><a href="#{photoFeedsLink}"><img src="~img/app/s/wojilu.Apps.Photo.Domain.PhotoPost.gif" /> _{photo}</a></li>
								<li><a href="#{forumFeedsLink}"><img src="~img/app/s/wojilu.Apps.Forum.Domain.ForumTopic.gif" /> _{forumPost}</a></li>
								<li><a href="#{shareFeedsLink}"><img src="~img/app/s/wojilu.Common.Feeds.Domain.Share.gif" /> _{share}</a></li>
								<li><a href="#{friendsFeedsLink}"><img src="~img/app/s/wojilu.Members.Users.Domain.FriendShip.gif" /> _{friends}</a></li>
							</ul>					
						</td>
					</tr>
					<tr><td colspan="4">
					<div id="friendsShare" style="display:none; "></div>
					<div id="followingShare" style="display:none; "></div>
					</td></tr>
				</table>						
	
				<!-- BEGIN days -->
				<div style=" margin:10px; font-size:14px;font-weight:bold;color:#666; border-bottom:1px #ccc solid;">#{feedDay}</div>
				
			    <!-- BEGIN list -->
			    <table style="width: 90%; border-top:0px #fff solid; margin:5px 0px 5px 20px;">
				    <tr>
					    <td rowspan="2" style="width:70px;vertical-align:top;padding-top:5px;"><a href="#{feed.UserLink}"><img src="#{feed.UserFace}" class="avs" /></a></td>
					    <td style="height:30px;vertical-align:top"><img src="~img/app/s/#{feed.DataType}.gif"/> #{feed.Title} <span class="note left10">#{feed.Created}</span> #{feed.CommentCmd}</td>
				    </tr>
				    <tr>
					    <td style="vertical-align:top" class="feedItemBody" id="feedItemBody#{feed.Id}">
					    <div>#{feed.Body}#{feed.BodyGeneral}</div>
					    <div id="commentList#{feed.Id}" style="display:none;"></div>
					    </td>
				    </tr>
			    </table>
			    <div style="border-top:1px #ddd solid; margin-left:95px;"></div>
			    <!-- END list -->
				    
				<div style="margin-bottom:20px;">&nbsp;</div>
				<!-- END days -->
				<div style="margin:10px 20px;">#{page}</div>
			
			
			</td>
			<td style="vertical-align:top;width:25%; padding-top:20px;" class="userHomeSidebar">
			<div style="display:none;">
			<h4><a href="#{notification.ListLink}"><img src="~img/s/msg.png" /> _{notification}#{notification.NewCount}</a></h4>
			#{notification.NewList}
			</div>
			
			<h4 style="margin-top:15px; padding-top:8px; border-top:1px #ddd dotted;">_{recentVisitors}</h4>
			<ul class="userAvatar">
				<!-- BEGIN visitors --><li>
				<a href="#{user.Link}"><img src="#{user.Face}"/></a>
				<div><a href="#{user.Link}">#{user.Name}</a></div></li>
				<!-- END visitors -->		
			</ul>
			<div class="clear"></div>
			
			<h4 style="margin-top:15px; padding-top:8px; border-top:1px #ddd dotted;">_{myFriends}</h4>	
			<ul class="userAvatar">
				<!-- BEGIN myfriendList --><li>
				<a href="#{user.Link}"><img src="#{user.Face}"/></a>
				<div><a href="#{user.Link}">#{user.Name}</a></div></li>
				<!-- END myfriendList -->		
			</ul>
			<div class="clear"></div>

			<h4 style="margin-top:15px; padding-top:8px; border-top:1px #ddd dotted;">_{myFollowing}</h4>	
			<ul class="userAvatar">
				<!-- BEGIN following --><li>
				<a href="#{user.Link}"><img src="#{user.Face}"/></a>
				<div><a href="#{user.Link}">#{user.Name}</a></div></li>
				<!-- END following -->		
			</ul>
			<div class="clear"></div>			
			
			</td>
		</tr>
	</table>
	
</div>		


</div>

<script>
    _run(function () {
        $('#feedWrap a').click(function () {
            $(this).attr('href', wojilu.str.trimEnd($(this).attr('href'), '?nolayout=1'));
            $(this).attr('target', '_blank');
        });
    });
</script>
