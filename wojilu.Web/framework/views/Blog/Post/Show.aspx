#{blog.SaveStatus}

<style>
.linePic {margin-bottom:10px;}
.lnkDown {margin-left:10px; text-decoration:underline;}
.warning {width:95%; }
</style>

<div class="blogMain">
    <div>
	<h4 class="blogTitle">#{blog.Title}</h4>
	<div class="blog-hits note">#{blog.Author} 发布于 #{blog.CreateTime} <span class="left10" >_{view}:</span> #{blog.Hits} <span class="left10" >_{reply}: <span id="contentReplies">0</span></span> 
    <span class="left10">所在分类：<a href="#{blog.CategoryLink}">#{blog.CategoryName}</a></span>
    <span class="left10">#{blog.TagList}</span></div>
	
        <div class="blogBody edui clearBoth">#{blog.Body}</div>
        
        <!-- BEGIN attachmentPanel -->
        <div style="width: 95%;padding: 10px 10px 10px 10px;margin:5px 5px 10px 5px; background:#f2f2f2; border-radius: 4px; ">
        <table style="width: 100%;">
            <tr>
                <td style="vertical-align:top;width: 30px;padding-top: 2px; font-weight: bold;">附件:</td>
                <td>
                    <!-- BEGIN attachments -->
                    <div style="padding:0px 5px 0px -5px;margin:2px 0px;"><img src="~img/attachment.gif"/>#{obj.FileName}<span class="note left5">(#{obj.FileSizeKB}KB)</span>#{obj.DownloadLink}</div>
                    <div>#{obj.PicLink}</div>
                    <!-- END attachments -->
                </td>
            </tr>
        </table></div>
        <!-- END attachmentPanel -->

        <div class="blogInfo floatLeft"> #{blog.Author} #{blog.CreateTime}</div>

    </div>
    <div class="clear"></div>
	
	<div style=" margin:10px 5px;border:1px #ccc solid; background:#fff; padding:3px 10px;" class="shareCmd" ></div>
	
	#{visitorList}	
    <div><a name="comments"></a></div>
    <div class="frmLoader" url="#{commentUrl}"></div>

</div>

<script>
_run( function() {
    wojilu.tool.shareFull();
    wojilu.ctx.changeUrl = false;
    wojilu.ui.frmLoader();
});
</script>
