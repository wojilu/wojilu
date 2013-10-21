<form method="post" action="#{ActionLink}">
	<table cellspacing="1" cellpadding="4" width="98%">
		<tr>
			<td><strong>_{title}</strong>
			</td>
			<td>
			<input name="Title" type="text" style="width: 500px" value="#{data.Title}"></td>
		</tr>
		<tr>
			<td><strong>_{category} </strong></td>
			<td><strong>#{CategoryId}&nbsp;&nbsp; _{tag}</strong> <span class="red">
			<input name="TagList" type="text" style="width: 150px" value="#{data.TagList}">(_{blankSeparator})</span></td>
		</tr>
		<tr>
			<td>
			<strong>_{content}</strong></td>
			<td>
<script type="text/plain" id="Content" name="Content">#{Content}</script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(350).line(2).show();
    });
</script>
<span class="valid border" to="Content"></span>
			</td>
		</tr>
		<tr>
			<td></td>
			<td align="left">
			<input name="btnEdit" type="submit" value=":{blog_pub_button}" id="btnPubBlog" class="btn">  
			<input name="btnSaveDraft" type="button" value=":{blog_draft_button}" id="btnSaveDraftBlog" class="btn left10 btnCancel" style="width:120px;margin-left:10px;">			
			<span id="saveInfo" class="left10 note">:{autoSaveTip}</span>
			<input name="Button1" type="button" value="_{return}" class="btnReturn left20" />
			<input type="hidden" id="draftId" value="#{data.Id}" /><input type="hidden" id="draftActionUrl" value="#{draftActionUrl}"/></td>
		</tr>
	</table>
</form>

<style>
#btnSaveDraftBlog { background:url("~img/s/save.png") no-repeat 10px 3px;}
</style>

<script type="text/javascript">
<!--
_run( function() {

    var btnSaveBlog;    
    
    var callback = function(data) {
        var result = data;
        if( !result.IsValid ) {
            $( '#saveInfo' ).html( '<span>:{exSave}:'+result.Msg+'</span>' );
        }
        else {			
            var resultId = parseInt( result.Msg );				
            $( "#draftId" ).val( resultId );
            $( '#saveInfo' ).html( ':{saveDraftOk}:' + wojilu.tool.getTime() );
        }
        btnSaveBlog.disabled=false;
        $( btnSaveBlog ).val( ':{saveDraft}' );
    };
    
    var autoSave = function() {
    
    
        var draftId = $( "#draftId" ).val();
        var postData = {
            "Title":$("input[name=Title]").val(),
            "Content": $("textarea[name=Content]").val(), 
            "TagList":$("input[name=TagList]").val(), 
            "CategoryId":$("select[name=CategoryId] option:selected").val(),
			"AccessStatus":$("input[name=AccessStatus]:checked").val(), 
            "IsCloseComment":$("input[name=IsCloseComment]:checked").val(), 
            "draftId":draftId
        };
        
 		if( !postData.IsCloseComment ) postData.IsCloseComment=0;
		if( wojilu.str.isNull( postData.Content ) ) return;
        
 		btnSaveBlog = this;
		btnSaveBlog.disabled=true;
		$( btnSaveBlog ).val( ':{saving}' );
        
        var draftActionUrl = $("#draftActionUrl").val().toAjax();
        $.post( draftActionUrl, postData, callback );
      
    };    

	setInterval( autoSave, 1000*60*3 );
    
    $("#btnSaveDraftBlog").click( autoSave );
    
 
    
});
//-->
</script>
