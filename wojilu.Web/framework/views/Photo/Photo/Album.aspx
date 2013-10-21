<div>
<h4 class="photoAlbumTitle">#{album.Name} <a href="#{appLink}" class="left10">›› 返回相册列表</a></h4>

<div style="margin:0px 0px 20px 5px">查看方式：
    <a href="#{viewNormalLink}">普通</a>
    <a href="#{viewBigLink}" class="left10">照片墙</a>
    <a href="#{viewSliderLink}" class="left10">幻灯片</a>
</div>

</div>

<div>

    <ul id="photoThumbList">
        <!-- BEGIN list -->
        <li>
            <div><a href="#{data.Url}"><img src="#{data.ImgThumbUrl}" class="photoThumbItem"  /></a></div>
            <div><div><a href="#{data.Url}">#{data.Title}</a></div></div>
            <div><div class="note">#{data.ViewsAndReplies}</div></div>
        </li>    
        <!-- END list -->
    </ul>
</div>
<div style="clear:both;"></div>
<div style="clear:both;">#{page}</div>

