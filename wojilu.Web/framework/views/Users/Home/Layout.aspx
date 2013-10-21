<link href="~css/wojilu.core.feed.css?v=#{cssVersion}" rel="stylesheet" type="text/css" />
<style>
#user-menu img{ max-width:140px; max-height:140px;}

#user-link { margin-top:10px;}
#user-link li { margin:5px 10px; background:#fff; border-radius:4px;}
#user-link li a { display:block; padding:3px 10px; border:1px solid #fff;border-radius:4px;}
#user-link li a:hover {border:1px solid #ddd; text-decoration:none;border-radius:4px;}

#space-feedback-wrap {border:1px solid #ccc;width1:95%;margin:15px 18px;padding:0px;}
#space-feedback-text{width:96%; height:16px;border:none; resize:none;outline:none; overflow:hidden; overflow-y:auto; padding:8px 8px 3px 8px; font-size:14px;}
#space-feedback-tool {padding:5px 5px; display:none; border-top:1px solid #ccc; background:#f2f2f2;}
#formResult { margin:10px 18px;}

.mblogOne {margin:0px 0px 10px 0px !important;}
.mblog-user-line { display:none;}
.section-title {margin:10px; font-size:14px; font-weight:bold; color:#333; border-radius:4px; background:#eee; padding:5px 10px;}
.space-info-item { margin-right:10px;}
.section-wrap {margin:10px;}
.feed-pic-item { background:#f2f2f2; border:none;}

a.feed-more-btn { color:#666;}
.feed-more-btn:hover {border:1px solid #ccc; text-decoration:none;}
.feed-more-btn 
{
    display:block; border-radius:2px; margin:5px 15px; text-align:center; padding:5px;
    border:1px solid #eee;
    background: rgb(255,255,255); /* Old browsers */
background: -moz-linear-gradient(top,  rgba(255,255,255,1) 0%, rgba(229,229,229,1) 100%); /* FF3.6+ */
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(255,255,255,1)), color-stop(100%,rgba(229,229,229,1))); /* Chrome,Safari4+ */
background: -webkit-linear-gradient(top,  rgba(255,255,255,1) 0%,rgba(229,229,229,1) 100%); /* Chrome10+,Safari5.1+ */
background: -o-linear-gradient(top,  rgba(255,255,255,1) 0%,rgba(229,229,229,1) 100%); /* Opera 11.10+ */
background: -ms-linear-gradient(top,  rgba(255,255,255,1) 0%,rgba(229,229,229,1) 100%); /* IE10+ */
background: linear-gradient(to bottom,  rgba(255,255,255,1) 0%,rgba(229,229,229,1) 100%); /* W3C */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#e5e5e5',GradientType=0 ); /* IE6-9 */

}

.space-feed-pager {margin:10px 0px 0px 15px; background:#f2f2f2; padding:3px 5px; border-radius:3px;}
</style>

<table style="width:100%;" cellpadding="0" cellspacing="0">
<tr>

    <td style="width:190px; vertical-align:top; padding:10px; background:#f2f2f2;">
        <div id="user-menu">#{userMenu}</div>
        <ul id="user-link">
            <li><a href="#{lnkHomepage}"><img src="~img/home.gif" /> 我的主页</a></li>
            <li><a href="#{lnkBlog}"><img src="~img/app/s/wojilu.Apps.Blog.Domain.BlogApp.png" /> 我的博客</a></li>
            <li><a href="#{lnkPhoto}"><img src="~img/app/s/wojilu.Apps.Photo.Domain.PhotoApp.png" /> 我的相册</a></li>
            <li><a href="#{lnkMicroblog}"><img src="~img/app/s/feeds.png" /> 我的微博</a></li>
            <li><a href="#{lnkForumPost}"><img src="~img/app/s/wojilu.Apps.Forum.Domain.ForumApp.png" /> 我的论坛帖子</a></li>
            <li><a href="#{lnkFeedback}"><img src="~img/app/s/microblog.png" /> 我的留言</a></li>            
            <li><a href="#{lnkFriend}"><img src="~img/app/s/group.png" /> 我的好友</a></li>
            <li><a href="#{lnkVisitor}"><img src="~img/users.gif" /> 我的访客</a></li>
            <li><a href="#{lnkProfile}"><img src="~img/profile.gif" /> 关于我</a></li>
            
        </ul>
    </td>
    <td style=" vertical-align:top; padding-bottom:20px; ">
        #{layout_content}        
    </td>
    <td style="width:230px; vertical-align:top;">
    
		<div class="section-title">_{recentVisitors}</div>
		<ul class="userAvatar">
			<!-- BEGIN visitors --><li>
			<a href="#{user.Link}"><img src="#{user.Face}"/></a>
			<div><a href="#{user.Link}">#{user.Name}</a></div></li>
			<!-- END visitors -->		
		</ul>
		<div class="clear"></div>
			
		<div class="section-title">_{myFriends}</div>	
		<ul class="userAvatar">
			<!-- BEGIN myfriendList --><li>
			<a href="#{user.Link}"><img src="#{user.Face}"/></a>
			<div><a href="#{user.Link}">#{user.Name}</a></div></li>
			<!-- END myfriendList -->		
		</ul>
		<div class="clear"></div>

		<div class="section-title">_{myFollowing}</div>	
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


<script>
    _run(function () {

        $('#space-feedback-wrap').click(function () {
            $('#space-feedback-text').height(60);
            $('#space-feedback-tool').show();
        });

        wojilu.ui.ajaxUpdateForm();

        require(['wojilu.app.microblog.view'], function (x) {
            x.init();
            x.addBlogEvent();
        });
    });
</script>