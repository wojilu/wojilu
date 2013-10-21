<div class="articleLocation">_{location}: #{location}</div>
<div style=" padding:10px 10px 10px 10px;">
<table style="width:100%" class="post-detail-wrap"><tr><td>

    <h1 class="post-detail-title">#{post.Title}</h1>	

    <div class="post-detail-info postInfo">
	    <span>#{post.Created}</span>
	    <span class="left10">_{view}:<span id="post-detail-hits">0</span></span> 
	    <span class="left10">#{post.Author}</span>
	    <span class="left10">_{comment}:<span id="contentReplies">0</span></span>
	    <span class="left10 fontBig" fontTarget="postContent">_{fontBig}</span>
	    <span class="fontSmall" fontTarget="postContent">_{fontSmall}</span>
	    <span class="left5 print" onclick="window.print();">_{print}</span>
    </div>

    <!-- BEGIN summary -->
    <div class="post-detail-summary">_{summary}: #{post.Summary}</div>
    <!-- END summary -->

    <div class="post-detail-body edui">#{detailContent}</div>

    <div class="post-detail-attachment">#{attachmentList}</div>
    <div class="post-detail-tag">#{post.Tag}<br />#{post.Source}</div>
    <div class="post-detail-submitter">投递人：#{post.Submitter}</div>
    
</td></tr></table>

<div class="diggPanel" id="diggPanel" style="width:410px; margin:20px auto; "><a name="digg"></a>

    <div id="btnDiggUp" class="btnDigg diggUp" onmousemove="this.style.backgroundPosition='left bottom';" onmouseout="this.style.backgroundPosition='left top';" style="background-position: 0% 100%; ">
		<div class="diggText">顶一下</div>
		<div class="diggNum">(0)</div>
		<div class="diggPer">
			<div class="diggPerBar"><span style="width:0"></span></div>
			<div class="diggPerNum">0%</div>
		</div>
	</div>
	
	<div id="btnDiggDown" class="btnDigg diggDown" onmousemove="this.style.backgroundPosition='right bottom';" onmouseout="this.style.backgroundPosition='right top';" style="background-position: 100% 0%; ">
		<div class="diggText">踩一下</div>
		<div class="diggNum">(0)</div>
		<div class="diggPer">
			<div class="diggPerBar"><span style="width:0"></span></div>
			<div class="diggPerNum">0%</div>
		</div>
	</div>
	
	<div class="clear"></div>	
</div>

<script>
    _run(function () {

        var lnkStats = '#{lnkStats}'.toAjax();
        $.post(lnkStats, function (data) {
            $('#post-detail-hits').text(data.hits);

            require(['wojilu.app.content.digg'], function (digg) {

                digg.setValue(data);

                digg.init('btnDiggUp', '#{lnkDiggUp}');
                digg.init('btnDiggDown', '#{lnkDiggDown}');

            });

        });

    });
</script>

<div class="shareCmd" style="border:1px #ccc solid; background:#fff; padding:5px 10px;"></div>

<div style="margin-top:10px;">
    <div style="margin:5px;">上一篇：#{prevPost}</div>
    <div style="margin:5px;">下一篇：#{nextPost}</div>
</div>

<div>
    <div style="margin:10px 5px 10px 5px; font-weight:bold;font-size:14px;">相关阅读: <span style="margin-left:5px; font-weight:normal;">#{post.Tag}</span></div>
    
    <ul style="margin:5px 5px 20px 30px;">
        <!-- BEGIN related -->
        <li style=" list-style-type:disc;"><a href="#{p.Link}">#{p.Title}</a><span class="note left5">(#{p.Created})</span></li><!-- END related -->    
    </ul>

</div>

<div>
    <div class="frmLoader" url="#{commentUrl}"></div>
</div>

</div>

<script>
    _run(function () {

        wojilu.ui.editFontSize();
        wojilu.tool.shareFull();

        wojilu.ctx.changeUrl = false;
        wojilu.ui.frmLoader();

    });
</script>
