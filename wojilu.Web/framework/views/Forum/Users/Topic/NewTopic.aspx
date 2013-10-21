#{location}



<div class="row">
<div class="span12">
<div class="forum-form-container forum-form-normal-container">


    <div class="clearfix" style="margin-top:10px;">

	<form action="#{ActionLink}" method="post" class="form-horizontal ajaxPostForm forum-form-normal">

        <div class="control-group">
            <label class="control-label" for="txtTitle">_{title}</label>
            <div class="controls">#{Category}<input name="Title" id="txtTitle" type="text" value="#{Title}" />
            </div>
            <div class="hide"><span class="valid" to="Title" mode="border"></span> </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="tagList">关键词</label>
            <div class="controls">
                <input name="TagList" id="tagList" type="text" value="#{TagList}" />
                <span class="help-inline">_{tagTip}</span>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="tagList">_{content}</label>
            <div class="controls">
                <div style="width:90%;">
<script type="text/plain" id="Content" name="Content">
    #{Content}
</script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(350).line(2).show();
    });
</script>

                </div>
                <div class="row" style="margin-top:5px;">
                    <div class="span4">                        
                        <label class="checkbox pull-left" style="width:90px;"><input name="saveContentPic" type="checkbox" /> 保存远程图片</label>
                        <label class="checkbox pull-left left20" style="width:130px;">
                            <input id="chkNeedLogin" name="IsAttachmentLogin" class="left10" type="checkbox" />
                            :{downloadCondition}
                        </label>
                    </div>
                    <div class="span5" style=" text-align:right;">
				            :{readPermission}
				            <input name="ReadPermission" size="20" type="text" style="width: 32px; padding:2px;height:13px;" /> 
                            :{price}
				            <input name="Price" size="20" type="text" style="width: 32px; padding:2px; height:13px;" />
                    </div>
                </div>
            </div>
            <div class="hide"><span class="valid" to="Content" mode="border"></span></div>
        </div>

        <div class="control-group">
            <label class="control-label">&nbsp;</label>
            <div class="controls">
                <div class="row">

                    <div class="span2" style="margin-left:10px;">

                        <button type="submit" class="btn btn-primary btn-large" name="btnSubmit" >
                          <i class="icon-pencil icon-white"></i> :{submitPost}
                        </button>

                    </div>

                    <div class="span5" style="margin-top:10px;">
                        <div class="pull-left">
                            <img src="~img/s/upload.png" class="" /> 上传附件 
                            <input id="uploadFileIds" name="uploadFileIds" type="hidden" />
                        </div>

                        <div class="pull-left left5">
                            <input id="file_upload" name="file_upload" type="file" />
                        </div>
                        
                        <div class="pull-left left20">
                            <a href="#{uploadLink}" class="frmBox" xwidth="700" xheight="380" title=":{uploadAttachment}">传统上传</a>
                        </div>
                    </div>

                </div>

                <div id="status"></div>
                <div id="uploader"></div>                
                <div id="uploadFiles"></div>

            </div>
        </div>


	</form>
    <div style="clear:both;"></div>
</div>
</div>
</div>
</div>


<script>

function addFile( objFile, deleteLink ) {
    require( ['wojilu.app.forum.upload'], function( attach ) {
        attach.addFile( objFile, deleteLink );
    });
}

_run( function() {

    wojilu.ui.valid();
    wojilu.ui.ajaxPostForm();

    require( ['wojilu.app.forum.upload'], function( attach ) {

        var cfg = wojilu.upload.getSettings();
        cfg.uploader = '#{batchUploadLink}'.toAjax(); // 接受上传的网址
        cfg.postData = #{authJson}; // 客户端验证信息(cookie等)
        cfg.fileTypeExts = '*.jpg;*.gif;*.png;*.jpeg;*.bmp;*.zip;*.7z;*.rar;'; // 允许上传的文件类型
        cfg.fileTypeDesc = 'All Files'; // 选择框中罗列的类型
        cfg.queueID = 'uploader'; // 显示进度条的容器

        // 可以自定义上传处理结果
        cfg.errors= '';
        cfg.validCount = 0;
        cfg.errorCount = 0;

        cfg.onUploadSuccess = function (file, data, response) {
            var obj = eval( '(' + data + ')' );
            if( obj.Msg ) {
                //alert( obj.Msg );
                cfg.errors += obj.Msg + '<br/>';
                cfg.errorCount += 1;
            }
            else {
                attach.addFile( obj.photo, obj.deleteLink );
                cfg.validCount += 1;
            }
        };

        cfg.onQueueComplete = function (data) {
            //var msg = data.successful_uploads + ' 个文件上传成功. ';
            //if( data.upload_errors>0 ) msg += data.upload_errors + ' 个错误.';
            //$('#status').text( msg );

            var msg = cfg.validCount + ' 个文件上传成功. ';
            if( cfg.errors.length>0 ) msg += '<br/>' + cfg.errorCount + ' 个错误：<br/>'+cfg.errors;
            $('#status').html( msg );

            // 重置
            cfg.errors = '';
            cfg.validCount = 0;
            cfg.errorCount = 0;
        };

        wojilu.upload.initFlash( '#file_upload', cfg );
    });
    
});
</script>
