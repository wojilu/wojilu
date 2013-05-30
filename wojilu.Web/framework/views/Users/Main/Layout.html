<style>
.section-panel { margin-bottom:20px;}
.section-panel h3 { margin-bottom:10px; padding:5px 5px 5px 5px;}

#user-search select {width:173px;}
#user-search-age select {width:80px;}

.user-list { border:1px #eee solid; border-radius:5px; margin:0px 5px; padding:10px 0px;}
.user-list li {width:65px; height:70px; overflow:hidden;}
.user-list .thumbnail {border:0px; text-align:center; box-shadow:0px 0px 0px #fff; }
.user-list li img { border-radius: 3px; width:48px; height:48px;}
.user-list .thumbnail div {margin-top:5px;width:55px; overflow: hidden; white-space:nowrap; text-overflow:clip; }


</style>

<div id="adminTool" class="row appAdminTool" style=" display:none;">
    <div class="admin-menu-wrap clearfix alert">
        <div class="admin-pointer">            
            <span class="btn btn-primary">
                用户管理 <i class="icon-hand-right icon-white"></i>
            </span>
        </div>

        <ul class="admin-menu unstyled">
        
            <li><a href="#{feedLink}" rel="nofollow" class="frmLink" loadTo="user-main-wrap" nolayout=3><i class="icon-fire"></i> 用户动态</a></li>
            <li><a href="#{userListLink}" rel="nofollow" class="frmLink" loadTo="user-main-wrap" nolayout=3><i class="icon-user"></i> 用户管理</a></li>
            <li><a href="#{siteMsgLink}" rel="nofollow" class="frmLink" loadTo="user-main-wrap" nolayout=3><i class="icon-envelope"></i> _{msgToAll}</a></li>
            <li><a href="#{regLink}" rel="nofollow" class="frmLink" loadTo="user-main-wrap" nolayout=3><i class="icon-plus-sign"></i> 快速注册</a></li>
            <li><a href="#{importLink}" rel="nofollow" class="frmLink" loadTo="user-main-wrap" nolayout=3><i class="icon-circle-arrow-right"></i> 用户批量导入</a></li>
            <li><a href="#{settingLink}" rel="nofollow" class="frmLink" loadTo="user-main-wrap" nolayout=3><i class="icon-wrench"></i> 用户设置</a></li>
            <li><a href="#{templateLink}" rel="nofollow" target="_blank"><i class="icon-edit"></i> 激活模板</a></li>      
            
        </ul>

    </div>
</div>

<div id="user-main-wrap">
    <div class="span8" style="margin-left:0px;margin-top:20px;">
        <div style="padding-left:15px;">
	    #{layout_content}
        </div>
    </div>

	
    <div class="span4">	
	    <div style="padding-right:20px;margin-top:20px;">
		
		    <h3 style="margin-bottom:10px;"><img src="~img/search2.gif" /> _{searchUser}</h3>
		    <div class="userPanelMain">
			    <form action="#{SearchAction}" method="get" class="form-inline well" id="user-search">
				
				    <table style="width: 100%">
					    <tr>
						    <td>_{gender}</td>
						    <td><div id="user-search-gender">#{g}</div></td>
					    </tr>
					    <tr>
						    <td>_{age}</td>
						    <td><div id="user-search-age">#{a1}&nbsp;-&nbsp;#{a2}</div></td>
					    </tr>
					    <tr>
						    <td>_{userLocation}</td>
						    <td>#{p2}</td>
					    </tr>
					    <tr>
						    <td colspan="2" style="text-align:center; padding-top:10px;">
                            
                                <button type="submit" class="btn btn-primary">
                                    <i class="icon-search icon-white"></i>
                                    _{searchUser}
                                </button>    
                
                                <span class="help-inline"><a href="#{SearchAction}" style="margin:5px;">_{advancedUserSearch}</a></span>        
                            
                            </td>
					    </tr>
				    </table>		
					
			    </form>
		    </div>
	    </div>
		
	    <div>
		    <h3><img src="~img/s/menus.png" /> _{pickedUser}</h3>
		    <div class="userPanelMain">				
			    <!-- BEGIN picked -->
			    <div class="uItem"><a href="#{u.Link}"><img src="#{u.Face}"/><br/>
		    #{u.Name}</a></div>
			    <!-- END picked -->
			    <div class="clear"></div>		
		    </div>
		
	    </div>
	
    </div>
    <div class="clear"></div>

</div>

<script>
    _run(function () {

        $('.admin-menu li').click(function () {
            $('.admin-menu li').removeClass('admin-menu-current');
            $(this).addClass('admin-menu-current');
        });

        var showAdminTool = function () {
            if (typeof (ctx) == 'undefined') return;
            if (ctx.viewer.IsLogin == false) return;

            if (ctx.owner.IsSite) {

                if (ctx.viewer.IsAdministrator) {
                    $('#adminTool').show();
                }
                else if (ctx.viewer.IsInAdminGroup) {
                    $.post('#{adminCheckUrl}'.toAjax(), function (data) {
                        if ('ok' == data) $('#adminTool').show();
                    });
                }
            }
            else if (ctx.viewerOwnerSame) {
                $('#adminTool').show();
            }
        };

        wojilu.site.load(showAdminTool);

        wojilu.ui.frmLink();
    });
</script>