
<style>
.tableItems td { vertical-align:top;}
</style>

<div style="padding:0px 10px 5px 0px; text-align:right;">
<form action="#{ActionLink}" method="get">
_{receiverName}(<a href="#{friendLink}" class="frmBox" title="_{addReceiverByFriendsTitle}">_{addReceiverByFriendsTip}</a>) 
<input name="q" type="text" id="txtReceiver" value="#{searchTerm}" /> <input type="submit" value="_{searchMail}" class="btn btns" />
</form>
</div>

<table style="width: 100%; " cellpadding="4" cellspacing="0" id="dataAdminList" data-action="#{adminAction}">
	<tr class="adminBar"><td colspan="5">
	_{op}: 
	<span id="btnDeleteSendedMsg" class="btnCmd" cmd="deletesendedmsg">_{deleteTrue}</span>
	<tr class="tableHeader">
		<td style="width:5%" align="center"><input id="selectAll" type="checkbox" title="_{checkAll}" /></td>
		<td style="width:15%">_{receiver}</td>
        <td style=" text-align:right;">&nbsp;</td>
		<td style="width:55%">_{mailTitle}</td>
		<td style="width:20%">_{sendTime}</td>
	</tr>
	<!-- BEGIN list -->
	<tr class="tableItems">
		<td align="center"><input name="selectThis" id="checkbox#{m.Id}" type="checkbox"></td>
		<td>#{m.ToName}</td>
        <td style=" text-align:right;">#{m.Attachments}</td>
		<td><a href="#{m.ReadUrl}">#{m.Title}</a> </td>
		<td>#{m.SendTime}</td>
	</tr>
	<!-- END list -->
	<tr>
		<td colspan="5" align="left" class="adminPage">#{page}</td>
	</tr>
</table>


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
</script>
