<style>

#myAdminNav {width:240px; }
#myAdminNav li { width:100%; font-size:14px;border-bottom:1px #aaa dotted;padding:5px 0px;}
#myAdminNav li a { margin-left:20px;}

.currentMyMbNav { background:#fff;}
</style>

    <div class="mbMainWrap">
	    <div class="mbMain">	
	        <div style="margin:0px;margin-bottom:50px; ">	#{layout_content}</div>
	    </div>
    	
    </div>


    <div style="float:left; " class="mbSidebarWrap">
	    <div class="mbSidebar">
    	
		    <div>
		    
	            <table style="margin:20px;">
                    <tr>
                        <td rowspan="2"><a href="#{user.MLink}"><img src="#{user.PicSmall}" class="avm" /></a></td>
                        <td><a href="#{user.MLink}" style="font-size:14px; margin-left:5px;">#{user.Name}</a></td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                </table>
    		
		        <table style="" cellpadding="0" cellspacing="0" class="mbStats">
                    <tr>
                        <td class="mbStatsNo" style="border-right:1px #aaa solid; "><a href="#{user.FollowingLink}">#{user.FollowingCount}</a></td>
                        <td class="mbStatsNo" style="border-right:1px #aaa solid;"><a href="#{user.FollowerLink}">#{user.FollowersCount}</a></td>
                        <td class="mbStatsNo" style=""><a href="#{user.MLink}">#{user.MicroblogCount}</a></td>
                    </tr>
                    <tr>
                        <td style="border-right:1px #aaa solid;"><a href="#{user.FollowingLink}">_{follow}</a></td>
                        <td style="border-right:1px #aaa solid;"><a href="#{user.FollowerLink}">_{fans}</a></td>
                        <td style=""><a href="#{user.MLink}">_{microblog}</a></td>
                    </tr>
                </table>
    		
		    </div>
		    
		    <div>
		        <ul style="" id="myAdminNav">
		            <li><a href="#{homeLink}"><span>我的首页</span></a></li>
		            <li><a href="#{atmeLink}"><span>@提到我的</span></a></li>
		            <li><a href="#{favoriteLink}"><span>我的收藏</span></a></li>
		            <li><a href="#{myCommentLink}"><span>我的评论</span></a></li>
		        </ul>
		    </div>
    		
		    <div>
    			
                <div style="margin:50px 10px 5px 10px; font-weight:bold; border-bottom:1px #ccc solid; padding:5px 10px;">#{user.Name} 的粉丝</div>
             
			    <ul class="userAvatar" style="width:230px; margin-left:10px">
				    <!-- BEGIN users --><li style="margin-right:5px;">
				    <a href="#{user.Link}"><img src="#{user.Face}" class="avs" /></a>
				    <div><a href="#{user.Link}">#{user.Name}</a></div></li>
				    <!-- END users -->		
			    </ul>
			    <div class="clear"></div>
			    <div style="text-align:right;padding-right:10px;"><a href="#{moreFollowers}">&rsaquo;&rsaquo; _{more}</a></div>

    			
                <div style="margin:10px; font-weight:bold; border-bottom:1px #ccc solid; padding:5px 10px;">最近访问</div>
             
			    <ul class="userAvatar" style="width:220px; margin-left:10px">
				    <!-- BEGIN visitor --><li style="margin-right:5px;">
				    <a href="#{user.Link}"><img src="#{user.Face}" class="avs" /></a>
				    <div><a href="#{user.Link}">#{user.Name}</a></div></li>
				    <!-- END visitor -->		
			    </ul>
			    <div class="clear"></div>
			    <div style="text-align:right;padding-right:10px;margin-bottom:30px;"><a href="#{moreVisitors}">&rsaquo;&rsaquo; _{more}</a></div>


		    </div>
	    </div>
	    
    </div>


<script>
_run( function() {
    wojilu.tool.makeTab( '#myAdminNav', 'currentMyMbNav', '' );
});
</script>