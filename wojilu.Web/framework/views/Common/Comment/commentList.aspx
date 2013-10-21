<style>
.postCommentOne .qSpan { color:#999;}
</style>

<!-- BEGIN comments -->
<div style="width: 98%; border-bottom:1px #aaa dotted;margin-bottom:15px;"></div>
<table style="width: 98%; margin:0px 0px 15px 0px;table-layout:fixed;" id="postCommentOne#{c.Id}" class="postCommentOne" cellpadding="0" cellspacing="0" border="0">
	<tr>
		<td rowspan="2" style="width:80px; text-align:center;vertical-align:top;padding-top:5px;">#{c.UserFace}</td>
		<td style="font-size:14px;  height:20px; vertical-align:top;"> #{c.UserName}  <span class="note">#{c.Created}</span></td>
		<td style="text-align:right;">
		    <span href="#{c.ReplyLink}" class="lnkReply link right10" to="content#{c.Id}"><img src="~img/s/reply.png" /> _{reply}</span>
		    <!-- BEGIN admin -->
		    <a href="#{c.DeleteLink}" class="ajaxDeleteCmd" removeId="postCommentOne#{c.Id}"><img src="~img/delete.gif" /> _{delete}</a>
		    <!-- END admin -->
		</td>
	</tr>
	<tr>
		<td id="content#{c.Id}" colspan="2" style="vertical-align:top;overflow:hidden">
		    <div style="width:95%;">#{c.Content}</div>
		</td>
	</tr>
</table>
<!-- END comments -->

<script>
_run( function() {

    $('.lnkReply').click( function() {
        var cellId = $(this).attr( 'to' );
        var contentCell = $('#'+ cellId );
        contentCell.append( $('#commentFormPanel') );        
        var actionUrl = $(this).attr( 'href' );        
        $( '#commentFormPanel form' ).attr( 'action', actionUrl );
    });

});
</script>
