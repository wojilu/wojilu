<div style="margin:10px 0px 20px 0px;">
	<form id="feedbackForm" method="post" action="#{ActionLink}" class="ajaxUpdateForm" insertType="prepend">

		<textarea id="feedbackContent" name="Content" style="width:99%;height:60px;"></textarea><br/>
		<input id="btnSubmit" class="btn" name="Submit1" type="submit" value="_{feedback}" /><input id="chkPrivate" name="chkPrivate" type="checkbox" class="left5" /><label for="chkPrivate">_{privateWords}</label> <span class="note">#{pwTip}</span>
	
	</form>
</div>

<div>
    <div id="formResult"></div>
	#{feedbackList}
</div>

<div>#{page}</div>

<script type="text/javascript">
<!--
_run( function() {
	wojilu.ui.ajaxUpdateForm();
});
//-->
</script>
