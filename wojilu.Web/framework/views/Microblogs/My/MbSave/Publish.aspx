<form method="post" action="#{ActionLink}" id="myform">

<style>
#btnPublish { background:url("~img/publish.gif");width:58px; height:45px; border:0px; color:#fff; font-size:16px; font-weight:bold;cursor:pointer;}
</style>

<script>

_run( function() {
	$('#btnPublish').click( function() {
	    var btn = $(this);
		$('#loading').html( '<img src="~img/ajax/loading.gif"/>' );
		var mycontent = $( '#txtContent' ).val();
		var picUrl = $( '#picUrl' ).val();
		if( wojilu.str.isNull( mycontent ) ) {			
			$('#loading').html( '' );
			alert( '_{exContent}' );
			return;
		}
		
		btn.attr( 'disabled', 'disabled' );
		var url = $('#myform').attr( 'action' );
		$.post( url.toAjax(), { 'Content':mycontent, 'PicUrl':picUrl }, function( data ) {
			btn.attr( 'disabled', '' );
			
			if( data=='ok' ) {
			    wojilu.tool.reloadPage();return;
				//$( '#txtContent' ).val( '' );
				//$('#loading').html( '' );
				//$('#microblogNew').html( mycontent + ' <span class="note">_{updatedJustNow}</span>' );
			}
			else
				alert( 'sorry,_{exop}=>'+data );
		});
	});
});

function setBgPic( thumbUrl, picUrl ) {
    $('#uploadPicThumb').html( '<img src="'+thumbUrl+'" />' );
    $('#picUrl').val( picUrl );
    $('#mcmdImg').click();
}

</script>

<style>
#mcmdImg { background:url("~img/img.gif"); background-repeat:no-repeat; background-position:0px 1px; padding-left:20px; display:inline-block;}
#mcmdLink { background:url("~img/link.gif"); background-repeat:no-repeat; background-position:0px 2px; padding-left:20px;display:inline-block;}
#mcmdImg, #mcmdLink { cursor:pointer;margin-right:10px;}
</style>


	<table style="width: 100%; margin:0px 0px 25px 0px; background:#;" cellpadding="0" cellspacing="0" border=0>
		<tr>
			<td rowspan="2" style="padding-right:10px; vertical-align:top; padding-top:30px;"><a href="#{my.Link}" title="_{updateFaceByHit}"><img src="#{my.Face}" /></a>
			</td>
			<td>	<div style="font-size:18px; font-weight:bold; margin-left:0px; padding:5px;" >What's happening?</div></td>
		</tr>
		<tr>
		    <td>
								
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td style="width:380px;">
						<div><textarea id="txtContent" name="Content" style=" width:380px;height:70px; border:1px #ccc inset;"></textarea></div>
						
						</td>
						<td style=" width:90px; vertical-align:top;padding-left:5px; padding-top:10px">
						<input type="button" id="btnPublish" value="_{publish}" />
						</td>
					</tr>
					<tr><td colspan="2">
						<div style="margin-top:5px;">插入：<span id="mcmdImg" class="frmUpdate" href="#{uploadLink}" loadTo="uploadPanel">图片</span> </div>
						<div id="uploadPanel" class="hide"></div>
						<div id="uploadPicThumb"></div>
						<div style="margin-top:10px; background:#eee;padding:5px;"><span id="microblogNew">#{currentBlog}</span><span id="loading" class="note left10"></span></div>
                        <input id="picUrl" type="hidden" />
					
					</td></tr>
				</table>									
								
			</td>
		</tr>
	</table>



</form>
