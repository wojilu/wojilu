<style>
a.lnkPreview { text-decoration:underline; margin-left:5px;}
</style>
<form method="post" action="#{ActionLink}" class="ajaxPostForm">
	<table cellspacing="1" cellpadding="4" width="98%">
		<tr>
			<td style="width:60px;"><strong>_{title}</strong>
			</td>
			<td>
			<input name="Title" type="text" style="width: 500px" value="#{data.Title}"></td>
		</tr>
		<tr>
			<td><strong>_{category} </strong></td>
			<td><strong>#{CategoryId}&nbsp;&nbsp; _{tag}</strong> <span class="red">
			<input name="TagList" type="text" style="width: 115px" value="#{data.TagList}"> <img src="~img/help.gif" title=":{tagTip}"/></span></td>
		</tr>

        <tr>
            <td style="vertical-align: top; padding-top: 8px;">
                <strong>附件</strong>
            </td>
            <td>
                <table>
                    <tr>
                        <td><img src="~img/attachment.gif" />上传附件</td>
                        <td><input id="file_upload" name="file_upload" type="file" /></td>
                        <td><span id="status"></span></td>
                    </tr>
                </table>
                <div id="myqueue"></div>
                <div id="attachmentList" style="padding-left:5px;margin-bottom:5px;">
                    <!-- BEGIN attachments -->
                    <div style="background:#f2f2f2;padding:0px 5px;margin:2px 0px;">
                        <img src="~img/arrowRight.gif"/>#{obj.FileName}
                        <span class="link left5 delAttachment" style="text-decoration:underline;" deleteLink="#{obj.DeleteLink}" deleteId="#{obj.Id}">删除</span>
                        #{obj.PicLink}
                    </div>
                    <!-- END attachments -->	
                </div>
                <div><input type="hidden" name="attachmentIds" id="attachmentIds" /></div>
            </td>
        </tr>

		<tr>
			<td style="vertical-align:top">
			<strong>_{content}</strong></td>
			<td>
<script type="text/plain" id="Content" name="Content">#{Content}</script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(400).line(1).show();
    });
</script>
<span class="valid border" to="Content"></span>
			</td>
		</tr>

		<tr><td></td><td><label><input name="saveContentPic" type="checkbox" />保存远程图片</label></td></tr>

		<tr style="display:none;">
			<td class="style1"><strong>_{privacy}</strong></td>
			<td class="style1"><table style="width: 100%">
				<tr>
					<td>#{data.AccessStatus}&nbsp;&nbsp;&nbsp; #{data.IsCloseComment}
					
					</td>
					<td class="style3"><span>
					<input name="option" id="optionMore" type="checkbox">_{moreOption}</span></td>
				</tr>
			</table>
			&nbsp;</td>
		</tr>
		<tr id="abstractRow" style="display:none">
			<td>
			
			
</td>
			<td><table style="width: 100%">
				<tr>
					<td>_{summary}</td>
					<td> 
			<textarea name="Abstract" style="width: 498px; height: 95px" rows="1">#{data.Abstract}</textarea></td>
				</tr>
				<tr>
					<td>trackback</td>					
					<td><input name="Text2" type="text" style="width: 494px">&nbsp;</td>
				</tr>
			</table>
			
			&nbsp;</td>
		</tr>
		<tr>
			<td></td>
			<td align="left">
			<input name="btnEdit" type="submit" class="btn" value=":{blog_edit_button}" id="btnPubBlog">
			<input name="Button1" type="button" value="_{return}" class="btnReturn" />
			</td>
		</tr>
	</table>
</form>

<script>

// 以下是上传附件的js
_run(function() {

    $('.delAttachment').live( 'click', function() {
        
        var deleteLink = $(this).attr( 'deleteLink' );
        var deleteId = $(this).attr( 'deleteId' );
        var ele = $(this).parent();
        
        $.post( deleteLink, function(data) {
            
            if( 'ok'!=data ) {
                alert(data);
                return;
            }
            
            ele.remove();
        });
        
    });

    var cookieValue = #{authJson};
    var uploadLink = '#{uploadLink}';
    
    cookieValue.dataId = #{dataId};
    cookieValue.dataType = '#{dataType}';
    
    cookieValue.viewerId = #{viewerId};
    cookieValue.viewerUrl = '#{viewerUrl}';
    
    cookieValue.ownerId = #{ownerId};
    cookieValue.ownerType = '#{ownerType}';
    cookieValue.ownerUrl = '#{ownerUrl}';
    

    // 总共两个步骤——

    // 1) 上传的配置信息
    var cfg = wojilu.upload.getSettings();

    cfg.uploader = uploadLink.toAjax(); // 接受上传的网址
    cfg.postData = cookieValue; // 客户端验证信息(cookie等)
    cfg.fileTypeExts = '*.jpg;*.gif;*.png;*.zip;*.rar;*.7z;'; // 允许上传的文件类型
    cfg.fileTypeDesc = 'All Files (.jpg, .png, .gif, .zip, .rar, .7z)'; // 选择框中文件类型描述
    cfg.queueID = 'myqueue'; // 显示进度条的容器

    // 每个文件上传成功之后在这里处理。data是服务端返回的数据。
    cfg.onUploadSuccess = function (file, data, response) { 
	        var obj = eval( '('+data+')' );
	        if( obj.FileName=='' ) {
	            alert( obj.Msg );
	            return;
	        }
	        
	        // 生成附件一行
	        $('#attachmentList').append( '<div style="background:#f2f2f2;padding:0px 5px;margin:2px 0px;"><img src="~img/arrowRight.gif"/> '+obj.FileName+' <span class="link left5 delAttachment" style="text-decoration:underline;" deleteLink="'+obj.DeleteLink+'" deleteId="'+obj.Id+'">删除</span></div>' );

    };

    cfg.onQueueComplete = function (data) {
        $("#status").text(data.successful_uploads + " 个文件上传成功, " + data.upload_errors + " 个错误.");
    };

    // 2) 将普通上传控件转化为 flash 上传控件
    wojilu.upload.initFlash( '#file_upload', cfg );


});

</script>
