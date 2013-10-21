
<div>
	<h3 style="margin-bottom:10px; padding-left:5px;"><img src="~img/s/feeds.png" /> _{hotTopic}</h3>
    <div class="hero-unit" style="padding:20px;">
	    <ul class="unstyled">
		    <!-- BEGIN list -->
		    <li><a href="#{post.Link}">#{post.Title}</a>
		    <span class="note">(_{group}:<a href="#{post.OwnerLink}">#{post.OwnerName}</a>, _{reply}:#{post.Replies})</span>
		    </li><!-- END list -->
	    </ul>
    </div>
</div>	

	
<div class="group-list">
	<h3 style="margin-bottom:10px; padding-left:5px;"><img src="~img/s/users242.png" /> _{hotGroup}</h3>
		
	<ul class="thumbnails">
		<!-- BEGIN hots -->
		<li class="span2" style="width:130px;">
            <div class="thumbnail">
		        <a href="#{g.Link}" ><img src="#{g.Logo}" /></a>
		        <div class="caption">
                    <h5><a href="#{g.Link}">#{g.Name}</a><span class="note">(#{g.MemberCount})</span></h5>
		            <div>#{g.Created}</div>
                </div>
            </div>
		</li><!-- END hots -->
	</ul>
	<div class="clear"></div>
</div>
	
<div class="group-list">
    <div class="pull-right"><a href="#{allGroupLink}" style="margin:5px 10px 0px 0px;">&rsaquo;&rsaquo; _{more}</a></div>
	<h3 style="margin-bottom:10px; padding-left:5px;"><img src="~img/s/credit.png" /> _{newGroup}</h3>
	<ul class="thumbnails">
		<!-- BEGIN recent -->
		<li class="span2" style="width:130px;">
            <div class="thumbnail">
                <a href="#{g.Link}" ><img src="#{g.Logo}"  /></a>
		        <div class="caption">                    
		            <h5><a href="#{g.Link}">#{g.Name}</a><span class="note">(#{g.MemberCount})</span></h5>
		            <div>#{g.Created}</div>
                </div>
            </div>
		</li><!-- END recent -->
	</ul>
	<div class="clear"></div>
</div>

