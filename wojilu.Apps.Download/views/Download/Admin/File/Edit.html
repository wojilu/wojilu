<div>
	<div class="formPanel" style="margin:10px 15px;">
		
<form method="post" action="#{ActionLink}" class="ajaxPostForm">

    <table style="width:100%;">
        <tr>
            <td style="width:100px;">
                软件名称</td>
            <td>
                <input name="fileItem.Title" type="text" style="width:500px;" value="#{fileItem.Title}" /><span class="valid" mode="border"></span></td>
        </tr>
        <tr>
            <td>
                软件分类</td>
            <td>
                #{categoryId} <select id="dropSubCategories" name="fileItem.CategoryId"></select><span class="valid" mode="border"></span> 
                <span class="left10">软件版权</span>#{fileItem.LicenseTypeId}
                <span class="left10">语言</span>#{fileItem.Lang}
                </td>
        </tr>
        <tr>
            <td style="vertical-align:top;">
                软件平台</td>
            <td><div style="">#{fileItem.PlatformIds}<span class="valid" mode="border" to="fileItem.PlatformIds"></span></div>
                </td>
        </tr>

        <tr>
            <td>
                版本</td>
            <td>
                <input name="fileItem.Version" type="text" value="#{fileItem.Version}" style="width:80px" /><span class="valid" mode="border"></span>&nbsp; 
                大小 <input name="fileItem.SizeMB" type="text" value="#{fileItem.SizeMB}" class="tipInput" tip="请填写整数或小数" style="width:100px;" /><span class="valid" mode="border"></span>MB&nbsp;&nbsp;&nbsp;
                评分<input name="fileItem.Rank" type="text" value="#{fileItem.Rank}" style="width:50px;" /><span class="note">(整数类型1-5)</span></td>
        </tr>
        <tr>
            <td>
                软件商名称</td>
            <td>
                <input name="fileItem.Provider" type="text" value="#{fileItem.Provider}" style="width:300px;" /><span class="valid" mode="border"></span></td>
        </tr>
        <tr>
            <td>
                软件商网址</td>
            <td>
                <input name="fileItem.ProviderUrl" type="text" value="#{fileItem.ProviderUrl}" style="width:200px;" />&nbsp;&nbsp; 
                email
                <input name="fileItem.Email" type="text" value="#{fileItem.Email}" /></td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>演示(demo)网址</td>
            <td>
                <input name="fileItem.DemoUrl" type="text" value="#{fileItem.DemoUrl}" style="width:500px;" /><span class="valid" mode="border"></span></td>
        </tr>

        <tr>
            <td>
                下载网址1</td>
            <td>
                
                <table>
                    <tr>
                    <td><input id="fileUrl1" name="fileItem.Url" type="text" style="width:500px;" value="#{fileItem.Url}" /><span class="valid" mode="border"></span></td>
                    <td><input id="file_upload" name="file_upload" type="file" /></td>
                    </tr>
                </table>
                <div id="myqueue"></div>

            </td>
        </tr>
        <tr>
            <td>
                下载网址2</td>
            <td>
                <input name="fileItem.Url2" type="text" value="#{fileItem.Url2}" style="width:500px;" /></td>
        </tr>
        <tr>
            <td>
                下载网址3</td>
            <td>
                <input name="fileItem.Url3" type="text" value="#{fileItem.Url3}" style="width:500px;" /></td>
        </tr>
        <tr>
            <td>
                简介</td>
            <td>
            
<script type="text/plain" id="fileItem.Description" name="fileItem.Description">#{fileItem.Description}</script>
<script>
    _run(function () {
        wojilu.editor.bind('fileItem.Description').height(250).line(1).show();
    });
</script>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <input name="Submit1" type="submit" value="_{submit}" class="btn" /></td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>

    

</form>

	</div>
</div>


<script>
_run( function() {

    var subcats = #{subCategoriesJson}; // 子类的数据(json格式)
    
    var fillSubCats = function( selectedVal ) {
        
        var dropsub = $('#dropSubCategories').empty(); // 子类的下拉框
        
        for( var i=0;i< subcats.length;i++ ) {
            var s = subcats[i];
            if( s.ParentId == selectedVal ) {
                dropsub[0].options.add( new Option(s.Name, s.Id) );
            }
        }
    };
    
    $("#categoryId").change( function() {
        fillSubCats($(this).val());
    });
        
    $("#categoryId").each( function() {
        fillSubCats($(this).val());
    });

    $('#dropSubCategories').val( #{fileItem.CategoryId} );
    
    wojilu.ui.valid();

    // 总共两个步骤——

    // 1) 上传的配置信息
    var cfg = wojilu.upload.getSettings();

    cfg.uploader = '#{uploadLink}'.toAjax(); // 接受上传的网址
    cfg.postData = #{authInfo}; // 客户端验证信息(cookie等)
    cfg.fileTypeExts = '*.jpg;*.gif;*.png;*.zip;*.rar;*.7z;'; // 允许上传的文件类型
    cfg.fileTypeDesc = 'All Files (.jpg, .png, .gif, .zip, .rar, .7z)'; // 选择框中文件类型描述
    cfg.queueID = 'myqueue'; // 显示进度条的容器

    // 每个文件上传成功之后在这里处理。data是服务端返回的数据。
    cfg.onUploadSuccess = function (file, data, response) { 

        var obj = eval( '('+data+')');

	    if( !obj.IsValid ) {
	        alert( obj.Msg );
	        return;
	    }

        $('#fileUrl1').val( obj.FileUrl );

    };

    // 2) 将普通上传控件转化为 flash 上传控件
    wojilu.upload.initFlash( '#file_upload', cfg );
    
});
</script>