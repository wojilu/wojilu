
<div class="formPanel2" style=" height:550px;">

	<form id="dataForm" method="post" action="#{ActionLink}" class="ajaxPostForm">
		<table style="width:98%;">
			<tr>
				<td style="width:50px; text-align:right; vertical-align:top; padding-right:5px;">
				<a href="#{friendLink}" class="frmBox" title="_{addReceiverByFriendsTitle}">_{receiver}</a>
				</td>
				<td>
                    <textarea name="ToName" id="receiver" style="width:380px; height:15px;">#{m.ToName}</textarea>
				    <a href="#{friendLink}" class="frmBox" title="_{addReceiverByFriendsTitle}">_{addReceiverByFriendsTip}</a>
				    <span class="note">_{receiverNote}</span>
                    <span class="valid border" to="ToName"></span>
				</td>
			</tr>
			<tr>
				<td style="text-align:right; vertical-align:top;padding-right:8px;">_{title}</td>
				<td>
				    <div>
                        <input name="Title" type="text" style="width:380px;" value="#{m.Title}">
                        <span class="valid border" to="Title"></span>
                    </div>
				    <table>
				        <tr>
				            <td><img src="~img/attachment.gif" />上传附件</td>
				            <td><input id="file_upload" name="file_upload" type="file" /></td>
				            <td><span id="status"></span></td>
				        </tr>
				    </table>				    
				    <div id="myqueue"></div>
				    <div id="attachmentList" style="padding-left:5px;margin-bottom:5px;"></div>
				</td>
			</tr>
			<tr><td style="text-align:right; vertical-align:top;padding-right:8px;">_{content}</td>
				<td>

<script type="text/plain" id="Content" name="Content">#{Content}</script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(350).line(1).show();
    });
</script>
<span class="valid border" to="Content"></span>

				</td>
			</tr>
			<tr><td></td>
				<td>
				    <input type="hidden" name="replyId" value="#{m.ReplyId}" />
                    <input type="hidden" name="attachmentIds" id="attachmentIds" />
				    <input name="Submit1" type="submit" id="btnSendMsg" value="_{send}" class="btn">&nbsp;				
				    <span id="lblInfo" class="note"></span>
				</td>
			</tr>
		</table>
	</form>
	
</div>
<script>
function fillUsers( users ) {	    
    
    var txtValue = $('#receiver').val();
    
    if( txtValue=='' ) {
        $('#receiver').val( users );
    }
    else {
        $('#receiver').val( txtValue + ','+users );
    }
}



function getRemovedIds( txtValue, deleteId ) {
    var ids = txtValue.split( ',' );
    var result = '';
    for( var i=0;i<ids.length;i++ ) {
        if( ids[i] == deleteId ) continue;
        result += ids[i];
        if( i<ids.length-1 ) result += ',';
    }
    return result;
}

function removeId( eleId, deleteId ) {
    var newIds = getRemovedIds( $('#'+eleId).val(), deleteId );
    $('#'+eleId).val( newIds );
}

function addId( eleId, objId ) {
    var ele = $('#'+eleId);
    var txtValue = ele.val();
    if( txtValue=='' ) {
        ele.val( objId );
    }
    else {
        ele.val( txtValue + ','+objId );
    }
}


_run(function() {

    $('.delAttachment').live( 'click', function() {
        
        var deleteLink = $(this).attr( 'deleteLink' );
        var deleteId = $(this).attr( 'deleteId' );
        var ele = $(this).parent();
        
        $.post( $(this).attr( 'deleteLink' ), function(data) {
            if( 'ok'!=data ) alert( data );
            ele.remove();
            
            // 从hidden字段中移除id
            removeId( 'attachmentIds', deleteId );
            
        });
        
    });

    var cfg = wojilu.upload.getSettings();

    cfg.uploader = '#{uploadLink}'.toAjax(); // 接受上传的网址
    cfg.postData = #{authJson}; // 客户端验证信息(cookie等)
    cfg.fileTypeExts = '*.zip;*.rar;*.7z;'; // 允许上传的文件类型
    cfg.fileTypeDesc = 'Zip Files'; // 选择框中文件类型描述
    cfg.queueID = 'myqueue'; // 显示进度条的容器

    cfg.onUploadSuccess = function (file, data, response) {            
	    // 此处代码：见下面下面第二部分的代码
	    var obj = eval( '('+data+')' );
	    if( obj.FileName=='' ) {
	        alert( obj.Msg );
	        return;
	    }
	        
	    // 生成附件一行
	    $('#attachmentList').append( '<div style="background:#f2f2f2;padding:0px 5px;margin:2px 0px;"><img src="~img/arrowRight.gif"/> '+obj.FileName+' <span class="link left5 delAttachment" style="text-decoration:underline;" deleteLink="'+obj.DeleteLink+'" deleteId="'+obj.Id+'">删除</span></div>' );
	        
	    // 将附件的Id加入hidden字段
	    addId( 'attachmentIds', obj.Id );	  
    };

    cfg.onQueueComplete = function (data) {
        var msg = data.successful_uploads + ' 个文件上传成功. ';
        if( data.upload_errors>0 ) msg += data.upload_errors + ' 个错误.';
        $('#status').text( msg );
    };

    wojilu.upload.initFlash( '#file_upload', cfg );


});
</script>

