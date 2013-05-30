<link href="~css/wojilu.app.forum.css?v=#{cssVersion}" rel="stylesheet" />

<div id="adminTool" class="row appAdminTool" style=" display:none;">
    <div class="admin-menu-wrap clearfix alert alert-block">

        <div class="admin-pointer">            
            <span class="btn btn-primary">
                论坛管理 <i class="icon-hand-right icon-white"></i>
            </span>
        </div>

        <ul class="admin-menu unstyled">	
	        <li><a href="#{noticeLink}" rel="nofollow" class="frmLink" loadTo="forumContainer" nolayout=4><i class="icon-th-list"></i> 头条</a></li>
            <li><a href="#{headlineLink}" rel="nofollow" class="frmLink" loadTo="forumContainer" nolayout=4><i class="icon-volume-up"></i> 公告</a></li>
	        <li><a href="#{forumBoardList}" rel="nofollow" class="frmLink" loadTo="forumContainer" nolayout=4><i class="icon-list-alt"></i> :{boardAndModerator}</a></li>
	        <li><a href="#{friendList}" rel="nofollow" class="frmLink" loadTo="forumContainer" nolayout=4><i class="icon-retweet"></i> :{friendForum}</a></li>
	        <li><a href="#{recyclebin}" rel="nofollow" class="frmLink" loadTo="forumContainer" nolayout=4><i class="icon-trash"></i> _{trash}</a></li>
	        <li><a href="#{security}" rel="nofollow" class="frmLink" loadTo="forumContainer" nolayout=4><i class="icon-remove-circle"></i> :{defaultPermission}</a></li>
	        <li><a href="#{forumLog}" rel="nofollow" class="frmLink" loadTo="forumContainer" nolayout=4><i class="icon-edit"></i> :{adminLog}</a></li>
	        <li><a href="#{dataCombine}" rel="nofollow" class="frmLink" loadTo="forumContainer" nolayout=4><i class="icon-resize-small"></i> :{dataCombine}</a></li>
	        <li><a href="#{settings}" rel="nofollow" class="frmLink" loadTo="forumContainer" nolayout=4><i class="icon-wrench"></i> _{setting}</a></li>
	    </ul>

    </div>
</div>

<div id="forumContainer">
#{layout_content}
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
            else {

                $.post('#{adminCheckUrl}'.toAjax(), function (data) {
                    if ('ok' == data) $('#adminTool').show();
                });

            }

        };

        wojilu.site.load(showAdminTool);

        wojilu.ui.frmLink();
    });
</script>
