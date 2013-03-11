<div>
<style>
.new {color:red;font-weight:bold;}
.hasReply {color:#aaa; font-weight1:bold;}
</style>

<div style="padding:0px 10px 5px 0px; text-align:right;">
<form action="#{ActionLink}" method="get">
_{senderName}(<a href="#{friendLink}" class="frmBox" title="_{addReceiverByFriendsTitle}">_{addReceiverByFriendsTip}</a>) 
<input name="q" type="text" id="txtReceiver" value="#{searchTerm}" /> <input type="submit" value="_{searchMail}" class="btn btns" />
</form>
</div>

<table style="width: 100%; " cellpadding="4" cellspacing="0" id="dataAdminList" data-action="#{adminAction}">
	<tr class="adminBar">
		<td colspan="4">
		<span id="btnDelete" class="btnCmd" cmd="delete"><img src="~img/trash.gif" /> _{toTrash}</span>
		</td>
		<td colspan="2" style="text-align:right">
		<span id="btnDeleteTrue" class="btnCmd" cmd="deletetrue"><img src="~img/delete.gif" /> _{deleteTrue}</span>		
		</td>
	</tr>
	<tr class="tableHeader">
		<td style="width:5%" align="center"><input id="selectAll" type="checkbox" title="_{checkAll}"/></td>
		<td style="width:6%">_{status}</td>
		<td style="width:12%">_{sender}</td>
		<td style="width:4%"></td>
		<td style="width:55%">_{mailTitle}</td>
		<td style="width:18%">_{receiveTime}</td>
	</tr>
	<!-- BEGIN list -->
	<tr class="tableItems">
		<td align="center"><input name="selectThis" id="checkbox#{m.Id}" type="checkbox" class="selectSingle"> </td>
		<td>#{m.StatusString}</td>
		<td>#{m.SenderName}</td>
		<td style="text-align:right">#{m.Attachments}</td>
		<td><a href="#{m.ReadUrl}" style="color:#005eac" class="msgLink" data-IsNew="#{m.IsNew}">#{m.Title}</a> </td>
		<td>#{m.ReceiveTime}</td>
	</tr>
	<!-- END list -->
	<tr>
		<td colspan="6" align="left" class="adminPage">#{page}</td>
	</tr>
</table>
</div>

<script>
function fillUsers( users ) {	    
    
    var txtValue = $('#txtReceiver').val();
    
    if( txtValue=='' ) {
        $('#txtReceiver').val( users );
    }
    else {
        $('#txtReceiver').val( txtValue + ','+users );
    }	    

}

_run( function() {

    $('.msgLink').click( function() {
    
        var isNew = $(this).attr( 'data-IsNew' );      
        if( isNew =='false' ) return;
        
        var eleMsgCount = $('#newMsgCount', wojilu.tool.getRootParent().document );
        
        var msgText = $.trim( eleMsgCount.text() );
        var mm = wojilu.str.trimStart(msgText,'(');
        mm = $.trim( wojilu.str.trimEnd(mm,')') );
        
        var msgCount = parseInt( mm );
        msgCount = msgCount - 1;
        if( msgCount <0 ) msgCount ==0;
        
        if( msgCount == 0 ) {
            $('#viewerNewMsgCount', wojilu.tool.getRootParent().document ).text( '' );
        }
        else {        
            eleMsgCount.text( '('+msgCount+')' );        
        }
        
    });
});

</script>