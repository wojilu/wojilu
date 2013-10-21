
<h3 style="font-size:20px; font-weight:bold; margin:20px 0px 15px 5px;">相册列表</h3>

<div>

    <ul id="photoAlbumList">
        <!-- BEGIN list -->
        <li>
            <div class="photoAlbumItem"><a href="#{album.Link}"><img src="#{album.Cover}" /></a></div>
            <div style="width:180px; overflow:hidden; text-overflow:clip; white-space:nowrap;"><a href="#{album.Link}">#{album.Name}</a></div>
            #{album.Description}
            <div class="gray">#{album.DataCount} 张照片</div>
        </li>
        <!-- END list -->
    </ul>
    
</div>
<div style="clear:both;"></div>

