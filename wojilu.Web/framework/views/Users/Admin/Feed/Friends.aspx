<div style="margin:0px;padding:0px;  height:120px; ">
    <!-- BEGIN shareFriends -->
    <div style="float:left;margin:10px;height:70px; width:60px; text-align:center;">
    <a href="#{user.ShareLink}" class="flink"><img src="#{user.Face}" class="avs" /></a><br/>
    <a href="#{user.ShareLink}" class="flink">#{user.Name}</a></div>
    <!-- END shareFriends -->
    <div class="clear"></div>
    <div>#{page}</div>
</div>

<script>
_run( function() {
    $('.flink').click( function() {
        wojilu.tool.getRootParent().location.href = wojilu.str.trimEnd($(this).attr('href'), '&frm=true');
        return false;
    });
});
</script>

