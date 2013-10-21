
			<div id="postComment">
				<h4>_{comment}<a name="comments"></a></h4>
				#{commentList}				
				<div id="formResult"></div>
				<!-- BEGIN page --><div style="margin:5px 0px 20px 10px;">#{page}</div><!-- END page -->

			</div>			
			
			<div id="postCommentForm" style="padding:10px 0px 10px 10px;">
			<div id="commentFormPanel" style="margin:5px 0px 10px 10px; box-shadow:0px 0px 10px #ccc;">
			<a name="commentFormStart"></a>
			<form class="commentForm ajaxUpdateForm autoSubmitForm" callback="resetImg" method="post" action="#{CommentActionUrl}"><a name="reply"></a>
			
				<!-- BEGIN loginForm -->
				<table class="commentPublishTable">
				<tr><td style="vertical-align:top;padding-top:8px; width:80px; text-align:center;"><img src="#{user.ThumbFace}" style="border-radius:4px;" /><br/>#{user.Name}</td>
				<td style="vertical-align:top; padding-top:8px; padding-bottom:8px;">
				<div><textarea name="Content" style="width: 430px; height: 70px"></textarea></div>
				<div style="margin-top:5px"><input name="Submit1" type="submit" value="_{submitComment}" class="btn btnSubmit">&nbsp;<span id="loading"></span></div>
				</td>
				</tr>
				</table>
				<input name="UserName" type="hidden" value="#{user.Name}" />				
				<!-- END loginForm -->
				
				<!-- BEGIN guestForm -->
				<table class="commentPublishTable">
					<tr>
						<td style="width:60px;">_{userName}<span class="red strong">*</span></td>
						<td>
						<input style="width: 120px" name="UserName" type="text" class="tipInput" tip="_{exUserName}" > <span class="valid" mode="border"></span> 
						_{emailOrWebsite} <input name="UserEmail" size="20" style=" width:210px;" type="text"></td>
							</tr>
							<tr>
								<td style="vertical-align:top">_{commentBody}<span class="red strong">*</span>
								<span class="valid" mode="border" to="Content"></span>
								</td>
						<td>
						<textarea name="Content" id="commentContent" style="width: 420px; height: 70px" class="tipInput" tip="#{contentLength}"></textarea>
                         </td>
					</tr>
					<tr style="display:none;" id="validationRow"><td>_{validationCode}<span class="red">*</span></td><td>#{Captcha}
					<tr>
						<td>&nbsp;</td>
						<td><input name="Submit1" type="submit" class="btn btnSubmit" value="_{submitComment}"></td>
					</tr>
				</table>
				<!-- END guestForm -->
			
			<input name="ParentId" type="hidden" value="#{parentId}" />
			</form>
			</div>
			</div>



<script type="text/javascript">
<!--
function showValidationImg() {
    if( wojilu.ctx.isValidationImgLoad ) return;
    $('#validationRow').show();    
    wojilu.ctx.isValidationImgLoad = true;
}

function resetImg(thisForm,data) {
    $('#validationRow img').click();
    return true;
}

_run( function() {
	wojilu.ui.tip();
    wojilu.ui.valid();
    wojilu.ui.httpMethod();
	wojilu.ui.ajaxUpdateForm();
	$('#commentContent').focus( showValidationImg );

});







//-->
</script>


