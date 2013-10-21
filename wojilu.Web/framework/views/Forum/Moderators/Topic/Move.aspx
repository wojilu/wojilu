

<div style="width: 600px;margin:20px;">

<script>
_run(function () {

	$('#chkReason').click( function() {
		$('#reasonText').toggle();
		$('#reasonSelect').toggle();
	});
	
	function isFormValid() {
		if( isReason()==false ) {
			alert( ':{exOpReason}!' );
			$('#myform').attr( 'isSubmit', 'false' );
			return false;
		}
		else {
			$('#myform').attr( 'isSubmit', 'true' );
		}
	}
	
	function isReason() {		
		if( $('#chkReason').attr( 'checked' )==false && $('#reasonSelect').val() != '' ) return true;
		if( $('#chkReason').attr( 'checked' ) && $('#reasonText').val() != '' ) return true;
		return false;
	}
	
	$('#myform').submit( isFormValid );
	
});
</script>

	<form id="myform" method="post" action="#{ActionLink}">
		<h4 style="font-size:14px;">#{ActionName}</h4>
		<table class="" style="width: 90%;">
			<tr><td>:{targetBoard}</td><td>#{dropForums}</td></tr>
		<tr>
			<td style="width:80px; vertical-align:top">:{opReason}</td>
			<td>
				<select name="reasonSelect" id="reasonSelect">
				<option value="">_{plsSelect}...</option>
				<optgroup label=":{punish}">
					<option value=":{ad}">:{ad}</option>
					<option value=":{spam}">:{spam}</option>
					<option value=":{illegalContent}">:{illegalContent}</option>
					<option value=":{irrelevantContent}">:{irrelevantContent}</option>
					<option value=":{repeatPost}">:{repeatPost}</option>
				</optgroup>
				<optgroup label=":{encourage}">
					<option value=":{agree}">:{agree}</option>
					<option value=":{pickedPost}">:{pickedPost}</option>
					<option value=":{originalWork}">:{originalWork}</option>
				</optgroup>
			</select>
			<input name="chkReason" id="chkReason" type="checkbox" /><label for="chkReason">:{orCustomReason}</label><br/>
			<textarea name="reasonText" id="reasonText" style="width: 350px; height: 50px; display:none"></textarea>
		</td>
		</tr>
		<tr><td></td>
		<td><div style="margin:20px 10px 40px 0px;">
		<input name="Submit1" id="mysubmit" class="btn" type="submit" value="_{submit}" />&nbsp;&nbsp;
			<input name="IsSendMsg" type="checkbox" id="IsSendMsg" checked="checked" /> <label for="IsSendMsg">:{sendMsgToAuthor}</label></div>
		</td>
		</tr>
		
	</table>		
	</form>
</div>
