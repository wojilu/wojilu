<style>
#detailView p {margin:10px 0px; text-indent:30px; line-height:150%;}
</style>

<div id="detailView">
    <div style="padding:5px 10px; border-bottom:1px #ccc solid;">查看文章详情
    
                <img src="~img/s/back.png" class="left20" /><a href="javascript:history.back();">_{return}</a>

    </div>
    
    <div style="margin:10px;">
        <div style="font-size:16px; font-weight:bold; padding:5px; text-align:center;">#{post.Title}</div>
        <div class="note" style="text-align:center; font-size:12px;">数据来源:#{post.SpiderTemplate.SiteName} <span class="left10">采集时间:#{post.Created}</span></div>
        <div style="font-size:12px; color:#666; text-align:center;">原文网址：<a href="#{post.Url}" target="_blank">#{post.Url}</a></div>
        <div style="text-align:center; margin:10px auto;"><span id="refreshPage" class="btn">重新抓取本页</span> <a href="#{editLink}" class="left20"><img src="~img/edit.gif" /> 修改</a></div>
        <div style="padding:10px 0px 20px 0px;" id="articleContent">#{post.Body}</div>
    
    </div>

</div>

<script>

_run( function() {

    var isClick = false;
    var btnText;
    $('#refreshPage').click( function() {
    
        if( isClick ) {
            return;
        }
        
        var btn = $(this);
        btnText = btn.text();
        $(this).text( '正在刷新，请稍后……' );
        
        
        isClick = true;
        $.post( '#{refreshPageLink}'.toAjax(), function(data) {
            var msg = data;
            if( msg.IsValid) {
                wojilu.tool.reloadPage();
            }
            else {
                alert( msg.Msg );
            }
            isClick = false;
            btn.text( btnText );
        });
        
    });

});

</script>