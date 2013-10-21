<div style="margin:20px;">#{publisher}</div>

<style>
.feed-pic-item {max-width:110px; max-height:110px;}
.feed-pic-item img{max-width:105px; max-height:105px;}
</style>

<div style="width:90%; margin:5px auto;">
<div class="font14 strong" style=" margin:5px auto;">我的评论</div>

<table style="width:100%; margin:10px auto;" border="0" id="my-comment-list">
<!-- BEGIN list -->

    <tr>
        <td rowspan="2" style="width:60px; vertical-align:top;"><a href="c.UserLink"><img src="#{c.UserFace}" style="padding:3px; border:1px #eee solid;" /></a></td>
        <td>
            <div style="float:left; font-size:14px;">评论人：<a href="#{c.UserLink}" target="_blank"><strong>#{c.UserName}</strong></a> <span class="note font12">#{c.Created}</span></div>
            <div style="float:right; text-align:right; font-size:12px;"><a href="#{c.MicroblogLink}">查看或回复</a></div>
            <div class="clear"></div>
        </td>
    </tr>
    <tr>
        <td style="font-size:14px;">
        <div>评论内容：<div style=" font-weight:bold;background:#f2f2f2;  border-radius:5px; padding:5px; margin:5px; ">#{c.Content}</div></div>
        <div>原微博：<div style=" font-size:12px; background:#f2f2f2;  border-radius:5px; padding:5px; margin:5px; color:#666;">#{c.Microblog}<div class="clear"></div></div></div>
        </td>
    </tr>



    <tr><td colspan="2" style="height:20px;"><div style="border-bottom:1px #ccc dotted;"></div></td></tr>
<!-- END list -->
</table>
<div>#{page}</div>
</div>
<script>
    _run(function () {
        $('#returnUrl').val('#{homeLink}');

        $('#my-comment-list a').click(function () {
            $(this).attr('href', wojilu.str.trimEnd($(this).attr('href'), '?nolayout=1'));
            $(this).attr('target', '_blank');
        });
    });
</script>
