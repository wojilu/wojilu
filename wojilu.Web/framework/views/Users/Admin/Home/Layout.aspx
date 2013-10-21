<style>

#myAdminNav {width:240px; }
#myAdminNav li { width:100%; font-size:14px;border-bottom:1px #aaa dotted;padding:5px 0px;}
#myAdminNav li a { margin-left:20px;}

.currentMyMbNav { background:#fff;}
</style>

<link href="~css/wojilu.core.feed.css?v=#{cssVersion}" rel="stylesheet" type="text/css" />

<table style="width:100%;" cellpadding="0" cellspacing="0">
<tr>
<td style=" vertical-align:top;width:70%;">#{layout_content}</td>
<td style=" vertical-align:top;width:30%;">

    <div style="float:left; " class="mbSidebarWrap">
	    <div class="mbSidebar"> 
		    <div style=" margin-top:25px;">
		        <ul style="" id="myAdminNav">
		            <li><a href="#{atmeLink}"><span>@提到我的</span></a></li>
		            <li><a href="#{favoriteLink}"><span>我的收藏</span></a></li>
		            <li><a href="#{myCommentLink}"><span>我的评论</span></a></li>
		        </ul>
		    </div>
    		
		    <div>
    			
                <div style="margin:50px 10px 5px 10px; font-weight:bold; border-bottom:1px #ccc solid; padding:5px 10px;">#{user.Name} 的粉丝</div>
             
			    <ul class="userAvatar" style="width:230px; margin-left:10px">
				    <!-- BEGIN users --><li style="margin-right:5px;">
				    <a href="#{user.Link}" target="_blank"><img src="#{user.Face}" class="avs" /></a>
				    <div><a href="#{user.Link}" target="_blank">#{user.Name}</a></div></li>
				    <!-- END users -->		
			    </ul>
			    <div class="clear"></div>
			    <div style="text-align:right;padding-right:10px;"><a href="#{moreFollowers}" target="_blank">&rsaquo;&rsaquo; _{more}</a></div>

    			
                <div style="margin:10px; font-weight:bold; border-bottom:1px #ccc solid; padding:5px 10px;">最近访问</div>
             
			    <ul class="userAvatar" style="width:220px; margin-left:10px">
				    <!-- BEGIN visitor --><li style="margin-right:5px;">
				    <a href="#{user.Link}" target="_blank"><img src="#{user.Face}" class="avs" /></a>
				    <div><a href="#{user.Link}" target="_blank">#{user.Name}</a></div></li>
				    <!-- END visitor -->		
			    </ul>
			    <div class="clear"></div>
			    <div style="text-align:right;padding-right:10px;margin-bottom:30px;"><a href="#{moreVisitors}" target="_blank">&rsaquo;&rsaquo; _{more}</a></div>


		    </div>
	    </div>
    </div>
	    

</td>
</tr>
</table>


<script>
_run( function() {
    wojilu.tool.makeTab('#myAdminNav', 'currentMyMbNav', '');
    require(['wojilu.app.microblog.view'], function (x) {
        x.init();
        x.addBlogEvent();
    });
});
</script>

