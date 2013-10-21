<script type="text/javascript">
<!--
_run( function() {

	var href = window.location.href;

	if( href.indexOf( 'Friends')>-1 || href.indexOf( 'Photo/Index')>-1 )
		$('#tabFriends').removeClass('otherTab').addClass( 'currentTab' );
	else if( href.indexOf( 'Album')>-1 )
		$('#tabCategory').removeClass('otherTab').addClass( 'currentTab' );
	else if( href.indexOf( 'Blogroll')>-1 )
		$('#tabBlogroll').removeClass('otherTab').addClass( 'currentTab' );
	else if( href.indexOf( 'Admin/Post/' )>-1 || href.indexOf( 'Admin/Post/' )>-1 )
		$('#tabAdd').removeClass('otherTab').addClass( 'currentTab' );
	else
		$('#tabMy').removeClass('otherTab').addClass( 'currentTab' );
    
});
//-->
</script>

<div style="padding:10px 10px 10px 10px;">

    <style>
    .tabList {border-bottom: 1px solid #B8D5FF;}
    .tabList li {width:120px;}
    </style>
    
    <div style="margin:5px 0px 15px 10px; font-weight:bold; font-size:14px;"><img src="~img/app/s/wojilu.Apps.Photo.Domain.PhotoApp.png" /> _{photo}</div>


<ul class="tabList" style="margin-left:10px; width:95%;">
    <li id="tabFriends" class="firstTab"><a href="#{friendsPhotoLink}" class="frmLink" loadTo="photoMain" nolayout=3>:{friendsPhoto}</a><span></span></li>
    <li id="tabMy"><a href="#{myLink}" class="frmLink" loadTo="photoMain" nolayout=3>:{myphoto}</a><span></span></li>
    <li id="tabAdd"><a href="#{uploadLink}" class="frmLink" loadTo="photoMain" nolayout=3><img src="~img/s/upload.png" /> _{upload}</a><span></span></li>
    <li id="tabCategory"><a href="#{categoryAdmin}" class="frmLink" loadTo="photoMain" nolayout=3>:{albumAdmin}</a><span></span></li>
    <div class="clear"></div>
</ul>

    <div style="width:100%;padding:0px;margin:0px;" class="tabMain">
	    <table style="width: 100%">
		    <tr>
			    <td style="padding:10px;" id="photoMain">#{layout_content}</td>
		    </tr>
	    </table>
    </div>

</div>
