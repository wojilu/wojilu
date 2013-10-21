<h4>#{section.Name}</h4>

<ul class="sectionVideos unstyled">
	<!-- BEGIN list -->
	<li style="width:148px; height:125px; border:1px solid #eee; border-radius:3px; margin:10px 20px 10px 10px; background:#fff; padding:10px 5px; box-shadow:3px 3px 7px rgba(0,0,0,0.1);">
		<div><a href="#{img.Url}"><img src="#{img.Thumb}"  class="thumbItem"/></a></div>
		<div style="width:148px;  overflow:hidden;text-overflow:clip;  white-space:nowrap; margin-top:10px;">
        <a href="#{img.Url}" style=" color:#333; font-size:14px;">#{img.Title}</a>
        </div>	

	</li><!-- END list -->
	
</ul>

<div class="clear"></div>

<div style="margin-top:15px;">#{page}</div>
<script>
    _run(function () {

        var twidth = 128;
        var theight = 96;

        $('.thumbItem').each(function () {
            if ($(this).width() > twidth) $(this).width(twidth);
            if ($(this).height() > theight) $(this).height(theight);
        });


    });
</script>