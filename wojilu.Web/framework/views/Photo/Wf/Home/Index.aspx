<div style="font-size:14px; margin:5px 10px 15px 25px; display:none;#{showAlbumCss}">
所属分类：<strong>#{albumName}</strong><span class="note left10 font12">(#{albumDataCount} 张照片)</span>
</div>

<div class="wflist">
	<ul id="wfContainer">

        <!-- BEGIN list -->
        <li class="wfpost">
            <div class="wfpost-pic">
                <div class="wfpost-picbody"><a href="#{x.Link}"><img src="#{x.Pic}" width="#{x.WidthSx}" height="#{x.HeightSx}" /></a></div>
                <div class="wfpost-name">#{x.Title}</div>
                <div class="wfpost-pins-likes">#{x.PinsLikes}</div>
            </div>
                
            <div class="wfpost-info">
                <table><tr><td class="wfpost-face"><a href="#{x.CreatorLink}"><img src="#{x.CreatorPic}" /></a></td>
                <td><a href="#{x.CreatorLink}">#{x.CreatorName}</a>  发布到 <a href="#{x.AlbumLink}">#{x.AlbumName}</a>
                </td></tr></table>
            </div>

            <div class="wfpost-tool">
                <a href="#{x.RepinLink}" class="frmBox right10" title="收集"><i class="icon-plus icon-white"></i>收集</a>
                <span class="#{x.LikedCss}" data-like-href="#{x.LikeLink}" data-unlike-href="#{x.UnLikeLink}">#{x.LikeName}</span>
            </div>

        </li>
        <!-- END list -->
    </ul>
    <div id="loadingWrap" style=" text-align:center;padding:10px;"></div>
</div>

<div style=" display:none;">#{page}</div>


