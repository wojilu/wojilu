<script type="text/javascript">
<!--
    _run(function () {

        var btnSaveBlog;

        var callback = function (data) {
            var result = data;
            if (!result.IsValid) {
                $('#saveInfo').html('<span>:{exSave}:' + result.Msg + '</span>');
            }
            else {
                var resultId = parseInt(result.Msg);
                $("#draftId").val(resultId);
                $('#saveInfo').html(':{saveDraftOk}:' + wojilu.tool.getTime());
            }
            btnSaveBlog.disabled = false;
            $(btnSaveBlog).val(':{saveDraft}');
        };

        var autoSave = function () {

            var draftId = $("#draftId").val();
            var postData = {
                "Title": $("input[name=Title]").val(),
                "Content": $("textarea[name=Content]").val(),
                "TagList": $("input[name=TagList]").val(),
                "CategoryId": $("select[name=CategoryId] option:selected").val(),
                "AccessStatus": $("input[name=AccessStatus]:checked").val(),
                "IsCloseComment": $("input[name=IsCloseComment]:checked").val(),
                "draftId": draftId
            };

            if (!postData.IsCloseComment) postData.IsCloseComment = 0;

            if (wojilu.str.isNull(postData.Content)) return;

            btnSaveBlog = this;
            btnSaveBlog.disabled = true;
            $(btnSaveBlog).val(':{saving}');

            var draftActionUrl = $("#draftActionUrl").val().toAjax();
            $.post(draftActionUrl, postData, callback);

        };

        setInterval(autoSave, 1000 * 60 * 3);

        $("#btnSaveDraftBlog").click(autoSave);



    });
//-->
</script>

<form method="post" action="#{ActionLink}" class="ajaxPostForm" style="width:98%;margin:auto;">

	<table cellspacing="1" cellpadding="4" width="100%" >
		<tr>
			<td><strong>_{title}</strong> </td>
			<td>
			<input name="Title" type="text" style="width: 500px"> <img src="~img/help.gif" title=":{emptyTitleTip}"/></td>
		</tr>
		<tr>
			<td><strong>_{category}</strong></td>
			<td>&nbsp;<span id="categoryList">#{CategoryId}</span><a id="categoryBox" href="#{categoryAddUrl}" class="frmBox link" xwidth="500" title=":{addCategory}"><img src="~img/add.gif"/> _{add}</a>
			<strong class="left10">_{tag}</strong> 
			<input name="TagList" id="tagList" type="text" style="width:150px"> <img src="~img/help.gif" title=":{tagTip}"/></td>
		</tr>

	    <!--附件部分-->
	    <tr>
	        <td>
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
	            <div id="attachmentList" style="padding-left:5px;margin-bottom:5px;"></div>
                <div><input type="hidden" name="attachmentIds" id="attachmentIds" /></div>
	        </td>
	    </tr><!--附件部分-->
        

		<tr>
			<td style="vertical-align:top;"><strong>_{content}</strong>
			<span to="Content" class="valid" mode="border"></span>
			</td>
			<td>
<script type="text/plain" id="Content" name="Content"></script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(400).line(2).show();
    });
</script>
<span class="valid border" to="Content"></span>
            </td>
		</tr>
		<tr><td></td><td><label><input name="saveContentPic" type="checkbox" />保存远程图片</label></td></tr>

		<tr style="display:none;">
			<td>			
			<strong>_{privacy}</strong></td>
			<td><table style="width: 100%">
				<tr>
					<td>
					<input name="AccessStatus" type="radio" value="0" checked>_{statusPublic} 
					<input name="AccessStatus" type="radio" value="1">_{statusFriend} 
					<input name="AccessStatus" type="radio" value="2">_{statusPrivate}&nbsp;&nbsp;&nbsp; 
					<input name="IsCloseComment" value="1" type="checkbox">_{closeComment}</td>
					<td class="style3"><input id="optionMore" type="checkbox">_{moreOption}</td>
				</tr>
			</table></td>
		</tr>
		<tr id="abstractRow" style="display:none">
			<td></td>
			<td>
			
<table style="width: 100%">
				<tr>
					<td>_{summary}</td>
					<td> 
			<textarea name="Abstract" style="width: 498px; height: 95px" rows="1"></textarea></td>
				</tr>
				<tr>
					<td>:{trackbackUrl}<br/>
					<span class="note">Trackback</span></td>
					<td>
					<textarea name="Trackback" style="width: 498px; height: 50px"></textarea>
					<br/><span class="note">:{trackbackTip}</span>
					</td>
				</tr>
			</table></td>
		</tr>
		<tr>
			<td align="left">
			</td>
			<td align="left">
			<input name="Submit1" type="submit" value=":{publog}" id="btnPubBlog" class="btn">&nbsp;&nbsp;&nbsp;&nbsp;
			<input name="Submit2" type="button" value=":{saveDraft}" id="btnSaveDraftBlog" style="width:120px;margin-left:10px;" class="btnCancel">
			 <span id="saveInfo" class="left10 note">:{autoSaveTip}</span>
			<input type="hidden" id="draftActionUrl" value="#{DraftActionUrl}"/>
			<input type="hidden" id="draftId" value="" /></td>
		</tr>
	</table>
</form>

<style>
#btnSaveDraftBlog { background:url("~img/s/save.png") no-repeat 10px 3px;}
</style>

<script>
function refreshCategories( chtml, categoryId ) {
    $('#categoryList').html( chtml );
    $('#CategoryId').val( categoryId );
}

_run( function() {
    if( #{showCategoryBox} ) $('#categoryBox').click();
});



// 以下是上传附件的js
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
        
        $.post( deleteLink, function(data) {
            if( 'ok'!=data ) {
                alert( data );
                return;
            }
            ele.remove();
            
            // 从hidden字段中移除id
            removeId( 'attachmentIds', deleteId );
            
        });
        
    });

    // 总共两个步骤——

    // 1) 上传的配置信息
    var cfg = wojilu.upload.getSettings();

    cfg.uploader = '#{uploadLink}'.toAjax(); // 接受上传的网址
    cfg.postData = #{authJson}; // 客户端验证信息(cookie等)
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
	        
	    // 将附件的Id加入hidden字段
	    addId( 'attachmentIds', obj.Id );
    };

    cfg.onQueueComplete = function (data) {
        $("#status").text(data.successful_uploads + " 个文件上传成功, " + data.upload_errors + " 个错误.");
    };

    // 2) 将普通上传控件转化为 flash 上传控件
    wojilu.upload.initFlash( '#file_upload', cfg );

});

</script>
