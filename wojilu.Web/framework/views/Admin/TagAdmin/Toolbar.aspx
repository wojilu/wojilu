<style>
.currentLink a{color:Red; font-size:18px;}
.otherLink a{font-size:12px;}
</style>


<div style="margin:0px 0px 10px 0px; padding:8px 0px; background2:#fff;" class="tagToolbar">
    <form method="get" action="#{ActionLink}">
    tag 列表排序：
    <span><a href="#{recentLink}" class="right10 strong">按时间</a></span>
    <span><a href="#{countAscLink}" class="right10 strong">由少到多</a></span>
    <span><a href="#{countDescLink}" class="right10 strong">由多到少</a></span>
    <span class="left20">搜索：<input name="tag" type="text" value="#{tagName}" />
    <input class="btn btns" type="submit" value="_{search}tag" /></span>
    </form>
</div>

<script>
_run( function() {
    wojilu.tool.makeTab( '.tagToolbar', 'currentLink', 'otherLink' );
});
</script>