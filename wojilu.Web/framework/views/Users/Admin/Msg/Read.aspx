
<div style="">

    <style>
    #msgHeader td { padding:1px 5px;}
    td.msgLeft { text-align:right; color:#666;}

    .msgToolbar {background:#dfedff; border-bottom:1px #aac5e6 solid; width:100%;}
    .msgToolbar td {padding:3px 10px; }
    .msgToolbar a { margin-right:10px;}

    .senderUrl { color:#666; margin-left:10px;}
    .senderUrl a{ color:#666;}

    </style>


    <table class="msgToolbar">
        <tr>
            <td>
                <span class="link" class="right5" onclick="javascript:history.back();" >&laquo; 返回</span>
                <span style="border-right:1px inset #efefef;margin-right:8px;">&nbsp;</span>
                #{m.ReplyButton}
                <a href="#{m.ForwardUrl}">_{forward}</a>
                <a href="#{m.DeleteUrl}" class="deleteCmd">_{delete}</a>       
            </td>        
            <td style="text-align:right;  color:#aaa;">
                #{m.PrevUrl}
                #{m.NextUrl}
            </td>
        </tr>
    </table>


    <table style="width:100%; background:#e8f0f9; border-bottom:1px #aac5e6 solid;" id="msgHeader">
	    <tr class="head">
		    <td style="width:60px;" class="msgLeft">_{sender}:</td>
		    <td><strong>#{m.Sender} </strong> #{m.SenderUrl}</td>
	    </tr>
	    <tr class="head">
		    <td  class="msgLeft">_{title}: </td>
		    <td><strong>#{m.Title}</strong></td>
	    </tr>
	    <tr class="head">
		    <td class="msgLeft">_{receiveTime}: </td>
		    <td class="note">#{m.CreateTime}</td>
	    </tr>
    	
	    <!-- BEGIN attachmentPanel -->
	    <tr>
	        <td style="vertical-align:top;" class="msgLeft">附件:</td>
	        <td>
	            <!-- BEGIN attachments -->
	            <div style="padding:0px 5px 0px -5px;margin:2px 0px;"><img src="~img/attachment.gif"/>#{obj.FileName}<span class="note left5">(#{obj.FileSizeKB}KB)</span><a href="#{obj.DownloadUrl}" class="left10" style="text-decoration:underline;">下载附件</a></div>
	            <!-- END attachments -->
	        </td>
	    </tr><!-- END attachmentPanel -->
    	
    </table>

    <div id="msgBody" style="padding:10px; font-size:14px;margin-bottom:30px;">
    #{m.Content}
    </div>

    <table class="msgToolbar">
        <tr>
            <td>
                <span class="link" class="right5" onclick="javascript:history.back();" >&laquo; 返回</span>
                <span style="border-right:1px inset #efefef;margin-right:8px;">&nbsp;</span>
                #{m.ReplyButton}
                <a href="#{m.ForwardUrl}">_{forward}</a>
                <a href="#{m.DeleteUrl}" class="deleteCmd">_{delete}</a>       
            </td>        
            <td style="text-align:right;  color:#aaa;">
                #{m.PrevUrl}
                #{m.NextUrl}
            </td>
        </tr>
    </table>


</div>


<script>

_run( function() {
    $('#msgBody a').unbind('click').click( function() {
        $(this).attr( 'target', '_blank' );
    });
});
</script>
