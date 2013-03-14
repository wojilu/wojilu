<div class="note" style="margin-bottom:5px;">请先在编辑器中<span style="color:Red; font-weight:bold;">点击定位</span>，然后开始上传。(你也可以 <a href="#" id="nlink">使用传统方式上传</a>)</div>
<div><input id="file_upload" name="file_upload" type="file" /> <span id="status"></span></div>
<div id="uploader"></div>


<script>

_run( function(){

    $('#nlink').click( function() {
        wojilu.tool.forward( '#{normalLink}'.toAjaxFrame(), 0 );
    });

    var cfg = wojilu.upload.getSettings();

    cfg.uploader = '#{uploadLink}'.toAjax(); // 接受上传的网址
    cfg.postData = #{authJson}; // 客户端验证信息(cookie等)
    cfg.fileTypeExts = '*.jpg;*.gif;*.png;*.jpeg;*.bmp;'; // 允许上传的文件类型
    cfg.fileTypeDesc = 'Image Files'; // 选择框中文件类型描述
    cfg.queueID = 'uploader'; // 显示进度条的容器

    cfg.onUploadSuccess = function (file, data, response) {            
        var obj = eval( '(' + data + ')' );
        if( !obj.IsValid ) {
            alert( obj.Msg );
            return;
        }

        window.parent.addEditorPicAndLink( '#{editorName}', obj.PicUrl, obj.OpicUrl );            
    };

    cfg.onQueueComplete = function (data) {
        wojilu.tool.reloadPage();// 解决上传完毕，无法输入中文问题
    };


    wojilu.upload.initFlash( '#file_upload', cfg );
    
});

</script>