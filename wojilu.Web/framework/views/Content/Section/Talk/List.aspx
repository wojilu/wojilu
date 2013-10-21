<h4>#{section.Title}</h4>
<ul class="unstyled">
	<!-- BEGIN list -->
	<li style="padding:10px 5px 10px 10px">
		<div style="margin-left:10px;">
            <div style=" float:left;"><span style="font-weight:bold; font:14px;">#{post.Author}</span></div>
            <div style=" float:right;"><span class="note">(#{post.Created})</span></div>
            <div style=" clear:both;"></div>
        </div>	

		<div class="quote"><span class="qSpan" style="font-size:14px; line-height:180%;">#{post.Content}</span></div>
        <div style="text-align:right; margin-bottom:10px; margin-right:15px;"><a href="#{post.Url}" target="_blank" style="color:;">&raquo;详细</a>	</div>
                	
	</li>
	<!-- END list -->
</ul>
<div>#{page}</div>
