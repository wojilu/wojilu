<link href="~css/wojilu.core.waterfall.css?v=#{cssVersion}" rel="stylesheet" />

<div class="wfnav clearfix">
    <a id="pHome" href="#{lnkHome}" class=""><i class="icon-home"></i> 图片首页</a>
    <a id="pFollowing" href="#{lnkFollowing}" class=""><i class="icon-picture"></i> 我关注的</a>
    <a id="pHot" href="#{lnkHot}" class=""><i class="icon-fire"></i> 热门</a>
    <a id="pPick" href="#{lnkPick}" class=""><i class="icon-thumbs-up"></i> 推荐</a>

    <span style="margin-left:30px; font-weight:bold;"><i class="icon-th"></i> 分类</span>
    <!-- BEGIN categories -->
        <a id="pCat#{x.Name}" href="#{x.LinkShow}" class="pCat">#{x.Name}</a>
    <!-- END categories -->
    <a id="pAdd" href="#{lnkAdd}" target="_blank" class="frmLink1" loadTo="wfAddWrap" nolayout=999 style="margin-left:50px;"><img src="~img/s/upload.png" /> 上传</a>
    #{adminCmd}
</div>

<div id="adminTool" class="row appAdminTool" style=" display:none;">
    <div class="admin-menu-wrap clearfix alert alert-block">

        <div class="admin-pointer">            
            <span class="btn btn-primary">
                全站图片管理 <i class="icon-hand-right icon-white"></i>
            </span>
        </div>

        <ul class="admin-menu unstyled">	
	        <li><a href="#{listLink}" rel="nofollow" class="frmLink" loadTo="wfMain" nolayout=4><i class="icon-list-alt"></i> :{newPhoto}</a></li>
            <li><a href="#{pickedLink}" rel="nofollow" class="frmLink" loadTo="wfMain" nolayout=4><i class="icon-star"></i> :{recommendedPhoto}</a></li>
            <li><a href="#{commentLink}" rel="nofollow" class="frmLink" loadTo="wfMain" nolayout=4><i class="icon-comment"></i> _{comment}</a></li>
            <li><a href="#{trashLink}" rel="nofollow" class="frmLink" loadTo="wfMain" nolayout=4><i class="icon-trash"></i> _{trash}</a></li>
            <li><a href="#{categoryLink}" rel="nofollow" class="frmLink" loadTo="wfMain" nolayout=4><i class="icon-th-large"></i> :{sysCategoryAdmin}</a></li>
            <li><a href="#{settingLink}" rel="nofollow" class="frmLink" loadTo="wfMain" nolayout=4><i class="icon-wrench"></i> 图片配置</a></li>
	        
	    </ul>

    </div>
</div>

<style>
.wfnav {padding:0px 10px;}
.wfnav a { display:block;margin:0px; padding:10px 10px; float:left; text-decoration:none;}
.wfnav span { display:block;margin:0px; padding:10px 10px; float:left;}

.wfNavCurrent { background:#61c0d6; color:#fff; font-weight:bold;}
.wfNavCurrent a { color:#fff;}
</style>
<script>
    _run(function () {
        var href = window.location.href;

        if (href.indexOf('home') > -1) {
            $('#pHome').addClass('wfNavCurrent');
            $('i', '#pHome').addClass('icon-white');
        }
        else if (href.indexOf('Following') > -1) {
            $('#pFollowing').addClass('wfNavCurrent');
            $('i', '#pFollowing').addClass('icon-white');
        }
        else if (href.indexOf('/hot') > -1) {
            $('#pHot').addClass('wfNavCurrent');
            $('i', '#pHot').addClass('icon-white');
        }
        else if (href.indexOf('pick') > -1) {
            $('#pPick').addClass('wfNavCurrent');
            $('i', '#pPick').addClass('icon-white');
        }
        else if (href.indexOf('category') > -1) {
            $('.pCat').each(function () {
                var thisUrl = $(this).attr('href');
                if (href.indexOf(thisUrl) > -1) {
                    $(this).addClass('wfNavCurrent');
                }
            });
        }
        else {
            //$('#pHome').addClass('wfNavCurrent');
            //$('i', '#pHome').addClass('icon-white');
        }

        $('#pAdd').click(function () {

            /*
            $('i', '.wfNavCurrent').removeClass('icon-white');
            $('.wfnav a').removeClass('wfNavCurrent');
            $(this).addClass('wfNavCurrent');

            
            $('#wfAddWrap').show();
            $('#wfMain').hide();
            */
        });

        wojilu.ui.frmLink();

    });
</script>

<div id="wfAddWrap" style="width:800px;margin:20px auto; display:none;">
</div>
<div id="wfMain">
#{layout_content}
</div>

<script type="text/javascript">
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

        require(['lib/jquery.infinitescroll.min', 'lib/jquery.masonry.min'], function () {

            var $container = $('#wfContainer');
            $container.infinitescroll({

                loading: {
                    msgText: "<div style='font-size:26px;color:#aaa;margin-top:20px;margin-bottom:20px;'>loading...</div>",
                    finishedMsg: "<div style='font-size:26px;font-weight:bold;color:#aaa;margin-top:20px;margin-bottom:20px;'>加载完毕</div>",
                    img: "~img/ajax/big.gif",
                    selector: '#loadingWrap'
                },

                navSelector: "div.turnpage",
                nextSelector: "div.turnpage a:first",
                itemSelector: ".wfpost"

            }, function (newElements) {
                var $newElems = $(newElements);
                $container.masonry('appended', $newElems);
            });


            $container.masonry({
                itemSelector: '.wfpost'
                //columnWidth: 200
            });

        });

        //---------------------------------------------------------

        var wftool = '.wfpost-tool';
        $('.wfpost').live({

            mouseenter: function () {
                $(wftool, this).show();
            },

            mouseleave: function () {
                $(wftool, this).hide();
            }

        });

        //---------------------------------------------------------

        // 点击喜欢
        $('.wfpost-like').live('click', function () {
            var lnk = $(this);
            var url = $(this).attr('data-like-href').toAjax();
            $.post(url, function (data) {

                if (data == 'ok') {
                    lnk.removeClass('wfpost-like').addClass('wfpost-liked').addClass('disabled').remove('i').text('已喜欢');
                }
                else {
                    alert(data);
                }
            });

            return false;
        });

        // 取消喜欢
        $('.wfpost-liked').live('click', function () {
            var lnk = $(this);
            var url = $(this).attr('data-unlike-href').toAjax();
            $.post(url, function (data) {

                if (data == 'ok') {
                    lnk.removeClass('wfpost-liked').addClass('wfpost-like')
                        .text('喜欢').prepend('<i class="icon-heart icon-white"></i>');
                }
                else {
                    alert(data);
                }
            });

            return false;
        });

    });
</script>