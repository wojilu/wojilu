
<div class="row" id="pick-row">

    <div class="span8">
        <div class="photo-pick-left">
        <script> _run(function() {wojilu.ui.slider();}); </script>
        <div id="slider" class="slideWrap" style="width:610px;height:280px;margin-bottom:20px;">
            <!-- BEGIN picked -->
               <a href="#{p.LinkShow}"><img alt="第一张图片" src="#{p.ImgUrl}"  /></a>
            <!-- END picked -->
        </div> 
        </div>
        <div style="clear:both;"></div>
    </div>


    <div class="span4">
		<div class="user-rank">
	
			<div class="user-rank-title"><i class="icon-user"></i> :{photoCountRank}</div>
			<ul class="unstyled">
			<!-- BEGIN users -->
			<li>
                <div><a href="#{user.Link}"><img src="#{user.Face}"/></a></div>
                <div><a href="#{user.Link}">#{user.Name}</a></div>
            </li>
			<!-- END users -->
			</ul>
			<div class="clear"></div>
		</div>
		
    </div>

</div>

<div class="row">
    <div class="span12">
        <!-- BEGIN categories -->
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
        <!-- END categories -->
    </div>
</div>
