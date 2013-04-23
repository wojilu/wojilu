<div style="width:780px; ">


<style>
#titleTd {width:480px;}
#title {width: 460px}
#tagList {width:200px}
#srcLink {width:250px;}

.tabItem { cursor:pointer;}
.tabMain {padding:10px 0px;}

td.pubLeft { text-align:right; width:80px; background:#eceff5; font-weight:bold; vertical-align:top;}

.postSection label {margin-right:10px;}
label.clickLabel { background:#ffe45c; }

.tabList {border-bottom: 1px solid #B8D5FF;}
</style>

<script type="text/javascript">
<!--
_run( function() {

    $('.tabList li a').click( function() {
	    var tabId = wojilu.str.trimStart( $(this).parent().attr( 'id' ), 'tab' );	    
	    $('.tabMain').hide();
	    $('#content'+tabId).show();	
    });
	
	$('.postSection label input').click( function() {
	    $(this).parent().toggleClass( 'clickLabel' );
	});
	
    //-------------------通过js选择section--------------------------------------------------
	
	var sectionId = #{sectionId};
	if( sectionId<=0 ) {
	    $('#sectionPanel').show();
	}
	else {
	    $('input[value='+sectionId+']').click();
	    $('#uploadImgPanel').attr( 'rowspan', 4 );
	}

	
	//-----------------------------------------------------------------------

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
    
    // 附件上传
    $('.delAttachment').live( 'click', function() {

        var deleteLink = $(this).attr( 'deleteLink' );
        var deleteId = $(this).attr( 'deleteId' );
        var ele = $(this).parent();

        $.post( $(this).attr( 'deleteLink' ), function(data) {
            if( 'ok'!=data ) alert( data );
            ele.remove();

            removeId( 'attachmentIds', deleteId );

        });

    });
    
    // 图片上传
    $('.delPic').live( 'click', function() {
    
        var deleteLink = $(this).attr( 'deleteLink' );
        var ele = $(this).parent();
        
        $.post( $(this).attr( 'deleteLink' ), function(data) {
            if( 'ok'!=data ) {
                alert( data );
                return;
            }
            
            ele.remove();
            $('#ImgLink').val('');
        });
    
    });
    

	//---------------文件上传--------------------------------------------------------   

    var cfg = wojilu.upload.getSettings();

    cfg.uploader = '#{uploadLink}'.toAjax(); // 接受上传的网址
    cfg.postData = #{authJson}; // 客户端验证信息(cookie等)
    cfg.fileTypeExts = '*.zip;*.rar;*.7z;'; // 允许上传的文件类型
    cfg.fileTypeDesc = 'Zip Files'; // 选择框中罗列的类型
    cfg.queueID = 'myqueue'; // 显示进度条的容器
    cfg.queueSizeLimit = 1;

    cfg.onUploadSuccess = function (file, data, response) {            
	    var obj = eval( '('+data+')' );
	    if( obj.FileName=='' ) {
	        alert( obj.Msg );
	        return;
	    }
	        
	    // 生成附件一行
	    $('#attachmentList').append( '<div style="background:#f2f2f2;padding:0px 5px;margin:2px 0px;"><img src="~img/arrowRight.gif"/> '+obj.FileName+' <span class="link left5 delAttachment" style="text-decoration:underline;" deleteLink="'+obj.DeleteLink+'" deleteId="'+obj.Id+'">删除</span></div>' );
	        
	    // 将附件在服务器上的url加入文本字段
	    addId( 'attachmentIds', obj.Id );
    };

    wojilu.upload.initFlash( '#file_upload', cfg );

    //-------------图片上传-----------------------------------------------------------

    var cfgPic = wojilu.upload.getSettings();
    cfgPic.uploader = "#{imgUploadLink}".toAjax();
    cfgPic.postData = #{authJson}; // 客户端验证信息(cookie等)
    cfgPic.fileTypeExts = '*.jpg;*.gif;*.png;'; // 允许上传的文件类型
    cfgPic.fileTypeDesc = 'Image Files'; // 选择框中罗列的类型
    cfgPic.queueID = 'imgUploadQueue'; // 显示进度条的容器
    cfgPic.queueSizeLimit = 1;

    cfgPic.onUploadSuccess = function (file, data, response) {      
        var obj = eval( '('+data+')' );
        if( obj.PicUrl=='' ) {
	        alert( obj.Msg );
	        return;
        }

        // 生成附件一行
        $('#uploadImgPanel').html( '<div><div><img src="'+obj.PicThumbUrl+'" style="width:120px;height:120px;"/></div><div class="link delPic" style="text-decoration:underline;padding-left:50px;" deleteLink="'+obj.DeleteLink+'">删除</div></div>' ).show();
	        
        // 将附件在服务器上的url加入文本字段
        $('#ImgLink').val( obj.PicUrl );
    };

    wojilu.upload.initFlash( '#imgUpload', cfgPic );

});
//-->
</script>

	
<form method="post" action="#{ActionLink}" style="padding:10px;">

	<ul class="tabList">
	<li id="tab1" class="firstTab currentTab" style="margin-left:30px; width:130px;"><a href="#">:{general}</a><span></span></li>
	<li id="tab2" style="width:130px;"><a href="#">:{advanced}</a><span></span></li>
    <li id="tab3" style="width:130px;"><a href="#">SEO</a><span></span></li>
    <li id="tab4" style="width:130px;"><a href="#">:{attachment}</a><span></span></li>
	</ul>
	
	
	<div id="content1" class="tabMain" style="width:98%;">
	<table style="width: 99%;margin:auto;" border="0">
		<tr>
			<td class="pubLeft">_{title}</td>
			<td id="titleTd"><input name="Title" id="title" type="text" class="tipInput" tip="_{exTitle}" style="height:20px;"><span class="valid border"></span></td>
			<td rowspan="5" style=" border-left2:1px #e2e6f3  solid;" id="uploadImgPanel"></td>
		</tr>
		<tr id="sectionPanel" style="display:none;">
		    <td style="vertical-align:top;" class="pubLeft">所属区块</td>
		    <td class="postSection">#{postSection}
            <span class="valid border" to="postSection"></span>
            </td>
		</tr>

		<tr>
			<td class="pubLeft">_{author}</td>
			<td style="vertical-align:top;"><input name="Author" type="text">
			<span class="left15">:{srcUrl}</span> <input name="SourceLink" id="srcLink" type="text" size="20">
			</td>
		</tr>
		
		<tr>
		    <td class="pubLeft">图片</td>
		    <td>
		        <div style="position:relative;">

                    <table>
                    <tr>
                        <td><input id="ImgLink" name="ImgLink" type="text" style="width:320px;" />
                        <span class="note">或上传</span></td>
                        <td><input id="imgUpload" name="imgUpload" type="file" /></td>
                    </tr>
                    </table>
		            <div><span id="imgUploadStatus" style="color:Blue;"></span></div>
		            <div id="imgUploadQueue" style="position:absolute; top:0px; left:0px;"></div>		  
                </div>
		    </td>
		</tr>


		<tr>
		    <td class="pubLeft">时间</td>
		    <td>
		        <input name="Created" type="text" class="right20" value="#{created}" />
		        
		        <span class="left20 note">文章属性</span>
		        #{PickStatus}
		    </td>
		</tr>
		
		<tr>
			<td class="pubLeft">_{content}</td>
			<td colspan="2">
<script type="text/plain" id="Content" name="Content"></script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(250).line(1).show();
    });
</script>
<span class="valid border" to="Content"></span>
            </td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td colspan="2">
		        <input name="Submit1" type="submit" value=":{submitData}" class="btn"> 
		        <input type="button" value="_{cancel}" class="btnCancel" />
		        <label class="left10">
                    <input name="isDowloadPic" type="checkbox" />抓取内容中的图片
		        </label>
		    </td>
		</tr>
	</table>
	</div>
	
	<div id="content2" class="tabMain" style="display:none;width:98%;">
	<table style="margin:auto;" class="pubTable">
	    <tr>
	        <td class="pubLeft">_{tag}</td>
	        <td><input name="TagList" id="tagList" type="text" style="width:300px;"><span class="note left5" title="">_{tagTip}</span></td>
	    </tr>

		<tr >
			<td class="pubLeft">_{summary}</td>
			<td>
			<textarea name="Summary" style="width: 600px; height: 90px"></textarea></td>
		</tr>
	    <tr>
            <td class="pubLeft">_{closeComment}</td>
            <td>        
                <label class="checkbox"><input type="checkbox" name="IsCloseComment" value="1"> 关闭评论</label>   	    
	        </td>
        </tr>
        <tr>
            <td class="pubLeft">查看次数</td>
            <td><input name="Hits" type="text" style="width:30px" value="0" /></td>
        </tr>
		<tr>
		    <td class="pubLeft">图片大小</td>
		    <td>:{width}<input id="width" name="Width" type="text" value="#{width}" style="width:40px;" />px 
            <span class="left20">:{height}</span><input id="height" name="Height" type="text" value="#{height}" style="width:40px;" />px</td>
		</tr>


        <tr><td></td><td>&nbsp;</td></tr>

		<tr>
		    <td class="pubLeft">跳转网址</td>
		    <td><input name="RedirectUrl" type="text" style="width:480px;" /><br />
            <span class="note">(如果设置，则文章被点击之后，不再显示内容，而是直接跳转到此网址)</span>
            </td>
		</tr>
	    <tr>
		    <td class="pubLeft">_{titleHome}</td>
		    <td style="padding-bottom:5px;"> <input name="TitleHome" type="text" style="width:480px;" type="text" value="" ><br />
            <span class="note">(为了美化首页排版，你可以稍作编辑，比如精简字数、修饰标题等)</span>
            </td>
	    </tr>
	    <tr>
		    <td class="pubLeft">:{titleStyle}</td>
		    <td><textarea name="Style" style="width: 550px; height: 50px"></textarea><br />
		    <span class="note">(:{cssRequired})</span>
		    </td>
	    </tr>





		
	</table>
	</div>

    <div id="content3" class="tabMain" style="display:none;width:98%;">
	<table style="margin:auto;" class="pubTable">

	    <tr>
	        <td class="pubLeft">SEO<br />keywords</td>
	        <td><input name="MetaKeywords" type="text" style="width:600px;"><br />
	        <span class="note">如果不填写，则使用tag</span>
	        </td>
	    </tr>

		<tr >
			<td class="pubLeft">SEO<br />description</td>
			<td>
			<textarea name="MetaDescription" style="width: 600px; height: 90px"></textarea><br />
			<span class="note">如果不填写，则使用摘要</span>
			</td>
		</tr>
        </table>
    </div>
	

    <div id="content4" class="tabMain" style="display:none;width:98%;">
	<table style="margin:auto;width:90%;" class="pubTable">

		<tr>
		    <td class="pubLeft">:{attachment}</td>
		    <td>
		        <div><input id="file_upload" name="file_upload" type="file" /><span id="status"></span></div>
                <div id="myqueue"></div>
                <div id="attachmentList" style="padding-left:5px;margin-bottom:5px;"></div>
                <input id="attachmentIds" name="attachmentIds" type="hidden" />
		    </td>
		</tr>

        </table>
    </div>

</form>


</div>

