
#{location}

<div style="margin-left:10px;">

    <table style="margin:20px 20px 20px 20px; width:95%;">
        <tr>
            <td style="font-size:20px; font-weight:bold;">:{attachmentAdmin}</td>
            <td style="text-align:right;">
            
            <table align="right">
                <tr>
                    <td><img src="~img/s/upload.png" /> :{addAttachment}</td>
                    <td><input id="file_upload" name="file_upload" type="file" /></td>
                    <td style="padding-right:20px;"><a  href="#{addLink}" class="frmBox left10">(传统上传)</a></td>
                    <td><span class="font12">#{cmd}</span></td>
                </tr>
                <tr><td colspan="4" id="uploader" style="text-align:left;"></td></tr>
            </table>
            
            </td>
        </tr>
        
    </table>
    

    <table id="dataAdminList" style="width:95%; margin:0px 20px 50px 20px; border-collapse:collapse;" border="1" cellpadding="3" cellspacing="0" data-sortAction="#{sortAction}">
        <tr style="font-size:14px; font-weight:bold; background:#eee;">
            <td style="text-align:center;">_{order}</td>
            <td>:{thumbPic}</td>
            <td>:{attachmentNameOrDescription}</td>
            <td>:{size}</td>
            <td>:{downloads}</td>
            <td style="text-align:center;">_{editName}</td>
            <td style="text-align:center;">:{uploadAgain}</td>
            <td style="text-align:center;">_{delete}</td>
        </tr>
        
        <!-- BEGIN list -->
        <tr class="rowLine">
	        <td class="sort" style="text-align:center;">
		        <img src="~img/up.gif" class="cmdUp right10" data-id="#{a.Id}"/>
		        <img src="~img/dn.gif" class="cmdDown" data-id="#{a.Id}"/>
	        </td>
            <td style="color:#666;">#{a.Info}</td>
            <td><a href="#{a.DownloadLink}" target="_blank">#{a.Name}</a></td>
            <td>#{a.Size} KB</td>
            <td>#{a.Downloads}</td>
            <td style="text-align:center;"><img src="~img/edit.gif" /> <a href="#{a.RenameLink}" class="frmBox">_{editName}</a></td>
            <td style="text-align:center;">
            <div><img src="~img/up.gif" /> <a href="#{a.UploadLink}" class="frmBox">:{uploadAgain}</a></div>
            </td>       
            <td style="text-align:center;"><img src="~img/delete.gif" /> <a href="#{a.DeleteLink}" class="deleteCmd">_{delete}</a></td>
        </tr><!-- END list -->
        
        
    </table>

    <style>
    #dataAdminList, #dataAdminList td { border:1px #ccc solid;}
    .rowLine td { padding-bottom:10px; vertical-align:top;}
    </style>
    
</div>

<script>
_run( function() {

    require(['wojilu._admin']);

    var cfg = wojilu.upload.getSettings();

    cfg.uploader = '#{uploadLink}'.toAjax(); // 接受上传的网址
    cfg.postData = #{authJson}; // 客户端验证信息(cookie等)
    cfg.fileTypeExts = '*.jpg;*.gif;*.png;*.jpeg;*.bmp;*.zip;*.7z;*.rar;'; // 允许上传的文件类型
    cfg.fileTypeDesc = 'All Files'; // 选择框中罗列的类型
    cfg.queueID = 'uploader'; // 显示进度条的容器

    cfg.onUploadSuccess = function (file, data, response) {
        var obj = eval( '(' + data + ')' );
        if( obj.Msg && obj.IsValid==false ) {
            alert( obj.Msg );
        }
    };

    cfg.onQueueComplete = function (data) {     
        wojilu.tool.reloadPage();        
    };

    wojilu.upload.initFlash( '#file_upload', cfg );    
    
});
</script>
