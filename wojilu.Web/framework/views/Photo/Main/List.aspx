
<div style="font-size:14px; margin:30px 0px 20px 0px;">当前路径：<a href="#{appLink}">图片频道</a> &raquo; <a href="#{category.LinkShow}">分类浏览</a></div>

<div class="row">
    <div class="span12">
        <div class="img-section">
            <div class="section-title">
                <div class="section-title-line"></div>
                <div class="section-title-text"><a href="#{category.LinkShow}">#{category.Name}</a></div>
            </div>
            <div class="clear"></div>
	        <ul class="unstyled">
	        <!-- BEGIN photos -->
	        <li>
                <div class="img-item"><a href="#{p.LinkShow}"><img src="#{p.ImgThumbUrl}" /></a></div>
		        <div class="img-note">_{from}:<a href="#{p.CreatorLink}">#{p.CreatorName}</a></div>
	        </li>
	        <!-- END photos -->
	        </ul>
	        <div class="clear"></div>
        </div>


        <div>#{page}</div>

    </div>
</div>