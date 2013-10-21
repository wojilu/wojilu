
<style type="text/css">
#menuHorizontal {margin:0px;padding:0px; clear:both; }
#menuHorizontal li{float:left; display:inline; margin:3px 5px 1px 5px; list-style:none;max-width:150px;}
#menuHorizontal li:hover { background:#f2f2f2; }

#menuHorizontal li div { height:20px;
    max-width:150px; 
     overflow: hidden;     
     white-space:nowrap;
     text-overflow:clip;
}

</style>

<ul class="menuHorizontal" id="menuHorizontal">
	<!-- BEGIN categories -->
	<li><div><a href="#{c.LinkPostAdmin}" class="frmLink" nolayout=4 loadTo="photoListPanel"><img src="~img/dot.gif" />#{c.Name}</a></div></li>
	<!-- END categories -->
	<li><a href="#{lnkDefaultAlbum}" class="frmLink" nolayout=4 loadTo="photoListPanel"><img src="~img/dot.gif" />:{defaultAlbum}</a></li>
</ul>
<div class="clear"></div>


<div id="photoListPanel">
#{layout_content}

</div>

