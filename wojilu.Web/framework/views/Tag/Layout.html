<div id="adminTool" class="row appAdminTool" style=" display:none;">
    <div class="admin-menu-wrap clearfix alert alert-block">

        <div class="admin-pointer">            
            <span class="btn btn-primary">
                Tag管理 <i class="icon-hand-right icon-white"></i>
            </span>
        </div>

        <ul class="admin-menu unstyled">	
	        <li><a href="#{tagAdminLink}" rel="nofollow" class="frmLink" loadTo="tag-main-wrap" nolayout=4> Tag管理</a></li>
	    </ul>

    </div>
</div>

<div id="tag-main-wrap">#{layout_content}</div>

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

        $('#tagForm').submit( function() {
    
            var ext = wojilu.str.getExt( location.href );
    
            var txtTag = $('#txtTag').val();
            if( !txtTag ) {
                alert( '请输入tag' );
                return false;
            }
        
            window.location.href = '/tag/'+txtTag+ext;
        
            return false;    
        });
    });
</script>

<div id="tempie6" ></div>
<script>
_run( function() {
    $('#tempie6').html( '');    
});
</script>
