

    <div class="mbMainWrap">
	    <div class="mbMain">
	        <div style="padding:10px 20px 5px 25px;;">
    	    
    	    
	            <table style="width:100%;">
                    <tr>
                        <td style="width:120px;vertical-align:top; padding:5px 20px 0px 5px;"><img src="#{user.Pic}" style="border:1px #efefef solid;" class="avm" /></td>
                        <td style="vertical-align:top;">
                            <div>
                                <div style=" float:left; font-size:22px; font-weight:bold; margin:0px 0px 10px 0px;">#{user.Name}</div>
                                <div style="float:right; margin:10px 10px 0px 0px;">#{adminBlogCmd}</div>
                                <div class="clear"></div>
                            </div>
                            <div>_{microblog}：<a href="#{user.MLink}">#{user.MLink}</a></div>
                            <div style="#{userLinkStyle}">空间：<a href="#{user.Link}">#{user.Link}</a></div>
                            <div style="margin:10px 0px 10px 0px; color:#666;">#{user.Description}</div>
                            <div>#{cmdFollow}</div>
                            <div id="myBox" class="ebox" style="width:200px; ">
                                <div style="margin:20px;">确实关注？ 
                                    <input id="btnFollow" type="button" value="确定" class="left20 btn" url="#{followUrl}" /></div>
                            </div>
                            <div id="cancelBox" class="ebox" style="width:200px; ">
                                <div style="margin:10px 20px; line-height:250%;">确实取消关注？<br /> 
                                    <input id="btnCancelFollow" type="button" value="确定" class=" btn" url="#{cancelUrl}" />
                                    <input id="bntClose" type="button" value="不了" class="left10 btnCancel" />
                                    </div>
                            </div>
                        </td>
                    </tr>
                </table>
    	    
    	    
	        </div>

	        <div style="margin:0px;margin-bottom:50px; ">	#{layout_content}</div>
	    </div>
    	
    </div>


    <div style="float:left; " class="mbSidebarWrap">
	    <div class="mbSidebar">
    	
		    <div>
    		
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
		    
		    <div style="margin:10px 10px 10px 20px; display:none" >
		        <div class="font14 strong">个人资料</div>
		        <div style="margin:10px 5px 0px 5px;width:180px; color:#666;">
		            #{user.Signature}
		        </div>
		    </div>
		    
		    <div style="margin:10px 10px 10px 20px;">
		        <div class="font14 strong">我的标签</div>
		        <div style="margin:10px 5px 0px 5px;width:180px;">
		        <!-- BEGIN tags -->
		        <a href="#{tag.Link}">#{tag.Name}</a>
		        <!-- END tags -->
		        </div>
		    </div>
		        		
		    <div>
    			
                <div style="margin:30px 10px 5px 10px; font-weight:bold; border-bottom:1px #ccc solid; padding:5px 10px;">#{user.Name} 的粉丝</div>
             
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

