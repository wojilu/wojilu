<link href="#{skinPath}?v=#{cssVersion}" rel="stylesheet" type="text/css" />
<script>_run(function () { wojilu.ui.slider(); }); </script>

<div id="adminTool" class="row appAdminTool" style=" display:none;">
    <div class="admin-menu-wrap clearfix alert">
        <div class="admin-pointer">            
            <span class="btn btn-primary">
                内容管理 <i class="icon-hand-right icon-white"></i>
            </span>
        </div>

        <ul class="admin-menu unstyled">	
            <li><a href="#{defaultLink}" rel="nofollow" class="frmLink" loadTo="contentPage" nolayout=3><i class="icon-home"></i> 页面设计(可拖拽)</a></li>
            <li><a href="#{allPostsLink}" rel="nofollow" class="frmLink" loadTo="contentPage" nolayout=3><i class="icon-list-alt"></i> 所有数据一览</a></li>
            #{submitterLink}
            <li><a href="#{commentLink}" rel="nofollow" class="frmLink" loadTo="contentPage" nolayout=3><i class="icon-comment"></i> 评论管理</a></li>
            <li><a href="#{trashPostsLink}" rel="nofollow" class="frmLink" loadTo="contentPage" nolayout=3><i class="icon-trash"></i> _{trash}</a></li>
            <li><a href="#{settingLink}" rel="nofollow" class="frmLink" loadTo="contentPage" nolayout=3><i class="icon-wrench"></i> _{setting}</a></li>
            <!-- BEGIN html -->
            <li><a href="#{staticLink}" rel="nofollow" class="frmLink" loadTo="contentPage" nolayout=3><i class="icon-file"></i> 生成html</a></li><!-- END html -->
        </ul>

    </div>
</div>

<div id="contentPage">#{layout_content}</div>

<script>
_run(function () {

    $('.admin-menu li').click(function () {
        $('.admin-menu li').removeClass('admin-menu-current');
        $(this).addClass('admin-menu-current');
    });
    
    var showAdminTool = function() {
        if( typeof(ctx) == 'undefined' ) return;
        if( ctx.viewer.IsLogin==false  ) return;
        
        if( ctx.owner.IsSite) {
            
            if( ctx.viewer.IsAdministrator ) {
                $('#adminTool').show();
            }
            else if( ctx.viewer.IsInAdminGroup ) {
                $.post( '#{adminCheckUrl}'.toAjax(), function(data) {
                    if( 'ok'==data ) $('#adminTool').show();
                });
            }            
        }
        else if( ctx.viewerOwnerSame ) {
            $('#adminTool').show();
        }        
    };
    
    wojilu.site.load( showAdminTool );

    wojilu.ui.frmLink();
});
</script>