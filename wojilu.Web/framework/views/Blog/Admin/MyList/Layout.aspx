
<style type="text/css">
#menuHorizontal {margin:0px;padding:0px; clear:both;margin-bottom:10px;}
#menuHorizontal li{float:left;margin:3px 5px 10px 5px; list-style:none;height:20px;}

#menuHorizontal li div { height:20px;
    max-width:150px; 
     overflow: hidden;     
     white-space:nowrap;
     text-overflow:clip;
}
</style>

<div style="margin:10px;">
    <ul style="margin:0px;padding:0px;" id="menuHorizontal">
	    <li>_{viewByCategory}:</li>	
	    <!-- BEGIN categories -->
	    <li><div><a href="#{c.LinkPostAdmin}" class="frmLink" loadTo="blogListPanel" nolayout=4><img src="~img/dot.gif" />#{c.Name}</a></div></li>
	    <!-- END categories -->

    </ul>

    <div id="blogListPanel">
    #{layout_content}

    </div>

</div>