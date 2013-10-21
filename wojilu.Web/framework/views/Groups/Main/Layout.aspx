<style>
.group-list .thumbnail h5 {margin-top:5px;width:105px; overflow: hidden; white-space:nowrap; text-overflow:clip; }</style>

<div id="adminTool" class="row appAdminTool" style=" display:none;">
    <div class="admin-menu-wrap clearfix alert alert-block">

        <div class="admin-pointer">            
            <span class="btn btn-primary">
                群组管理 <i class="icon-hand-right icon-white"></i>
            </span>
        </div>

	    <ul class="admin-menu unstyled">	
	        <li><a href="#{groupAdminHome}" rel="nofollow" class="frmLink" loadTo="group-main-wrap" nolayout=3><i class="icon-home"></i> _{adminGroup}</a></li>
	        <li><a href="#{postLink}" rel="nofollow" class="frmLink" loadTo="group-main-wrap" nolayout=3><i class="icon-list-alt"></i> _{allPosts}</a></li>
	        <li><a href="#{groupLink}" rel="nofollow" class="frmLink" loadTo="group-main-wrap" nolayout=3><i class="icon-th"></i> _{allGroup}</a></li>
	        <li><a href="#{groupCategoryLink}" rel="nofollow" class="frmLink" loadTo="group-main-wrap" nolayout=3><i class="icon-th-large"></i> _{groupCategory}</a></li>
            <li><a href="#{settingLink}" rel="nofollow" class="frmLink" loadTo="group-main-wrap" nolayout=3><i class="icon-wrench"></i> 群组配置</a></li>
	    </ul>

    </div>
</div>

<div id="group-main-wrap">

	<div class="span8" style="margin-left:0px;margin-top:20px;"><div style="padding-left:20px;">#{layout_content}</div></div>
	
	<div class="span4" style="margin-top:20px;">
        <div style="padding-right:20px;">

		<div class="clearfix">
			<h3 style=" margin-bottom:10px; padding-left:5px;"><img src="~img/search2.gif" /> _{groupSearch}</h3>
			<form class="form-inline well" action="#{SearchAction}" method="get">
                <input name="name" type="text" value="#{s.Name}" class="input-small" placeholder="请输入关键词" />
				<select name="queryType" style="width:65px;">
					<option value="name">_{name}</option>
					<option value="creator">_{creator}</option>
				</select>
                <button type="submit" class="btn btn-primary">
                    <i class="icon-search icon-white"></i>
                    _{search}
                </button>
			</form>
				
		</div>

		<div style="margin-top:10px;">
            <div class="pull-right" style="margin:5px 10px 0px 0px;"><a href="#{allGroupLink}">&rsaquo;&rsaquo; _{allGroup}</a></div>
			<h3 style=" margin-bottom:10px; padding-left:5px;"><img src="~img/s/interest.png" /> _{viewByCategory}</h3>
            
			<ul class="unstyled hero-unit" style="padding:20px;">
			    <li><a href="#{allGroupLink}">_{all}</a></li>
			    <!-- BEGIN categories -->
			    <li><a href="#{c.Link}">#{c.Name}</a></li><!-- END categories -->			
			</ul>
		</div>		
	
	</div>
    </div>

</div>

<div style="clear:both;"></div>

<div id="tempie6" ></div>
<script>
_run(function () {
    $('#tempie6').html( '');    
});
</script>

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
