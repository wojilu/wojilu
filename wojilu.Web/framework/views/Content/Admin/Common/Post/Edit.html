<div style="width:780px; ">

<style>
.postTitleTd {width:480px;}
.postTitle {width: 460px}
#tagList {width:200px}
#srcLink {width:250px;}

.tabItem { cursor:pointer;}
.tabMain {padding:10px 0px;}

td.pubLeft { text-align:right; width:80px; background:#eceff5; font-weight:bold; vertical-align:top;}

#postSection label {margin-right:10px;}
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

	
	$('#postSection label input').click( function() {
	    $(this).parent().toggleClass( 'clickLabel' );
	});
	
	var sectionIds = '#{sectionIds}'; // 23,88,11
	var arrIds = sectionIds.split( ',' );
	
	function isChecked( id ) {
	    for(var i=0;i<arrIds.length;i++) {	    
	        if( id ==arrIds[i] ) return true;
	    }
	    return false;
	}
	
	$('#postSection label input').each( function() {
	    if( isChecked( $(this).val() ) ) {
	        $(this).click();
	    }

	});
	
	function setImgInfo( obj ) {
        $('#uploadImgPanel').html( '<div><div><img src="'+obj.PicThumbUrl+'" style="width:120px;height:120px;"/></div><div class="link delPic" style="text-decoration:underline;padding-left:50px;" deleteLink="'+obj.DeleteLink+'">删除</div></div>' ).show();
        $('#ImgLink').val( obj.PicUrl );
	}
	
	var objImg = { PicUrl:'#{post.ImgLink}', PicThumbUrl: '#{post.ImgThumbLink}', DeleteLink:'#{post.ImgDeleteLink}'.toAjax() };
	if( objImg.PicUrl ) {
	    setImgInfo( objImg );
	}
	
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
	    setImgInfo( obj );     
    };

    wojilu.upload.initFlash( '#imgUpload', cfgPic );
    
	
});
//-->
</script>




<form method="post" action="#{ActionLink}" style="padding:10px;">
	
	<ul class="tabList clearfix">
	<li id="tab1" class="firstTab currentTab" style="margin-left:30px; width:130px;"><a href="#">:{general}</a><span></span></li>
	<li id="tab2" style="width:130px;"><a href="#">:{advanced}</a><span></span></li>
    <li id="tab3" style="width:130px;"><a href="#">SEO</a><span></span></li>
	<li id="tab4" style="width:130px;"><a href="#{attachmentLink}?nolayout=4" target="_blank">附件管理 <img src="~img/s/external-link.png" /></a><span></span></li>
	</ul>
	
	<div id="content1" class="tabMain" style="width:98%;">
	<table style="width: 99%;margin:auto;">

		<tr>
			<td class="pubLeft"">_{title}</td>
			<td class="postTitleTd"><input name="Title" id="title" type="text" value="#{post.Title}" class="tipInput postTitle" tip="_{exTitle}"><span class="valid border"></span>
			</td>
			<td rowspan="5" id="uploadImgPanel"></td>
		</tr>				

		<tr>
		    <td style="vertical-align:top;" class="pubLeft">所属区块</td>
		    <td id="postSection">#{postSection}<span class="valid border" to="postSection"></span></td>
		</tr>

		<tr>
			<td class="pubLeft">_{author}</td>
			<td><input name="Author" type="text" value="#{post.Author}">
			<span style="margin-left: 10px">:{srcUrl}</span> <input name="SourceLink" type="text" size="20" id="srcLink" value="#{post.SourceLink}">
			</td>
		</tr>
		<tr>
			<td class="pubLeft">:{picUrl}</td>
			<td>
		        <div style="position:relative;">

                    <table>
                        <tr>
                        <td>
                            <input id="ImgLink" style="width:320px;" name="ImgLink" type="text" value="#{post.ImgLink}" />
                            <span class="note">或上传</span>
                        </td>
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
		        <input name="Created" type="text" class="right20" value="#{post.Created}" />
		        
		        <span class="left20 note">文章属性</span>
		        #{PickStatus}
		    </td>
		</tr>

		<tr>
			<td class="pubLeft">_{content}<br /><span to="Content" class="valid border"></span></td>
			<td colspan="2">
<script type="text/plain" id="Content" name="Content">#{Content}</script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(250).line(1).show();
    });
</script>
            
            </td>
		</tr>		
		<tr>
			<td>&nbsp;</td>
			<td colspan="2">
			    <input name="Submit1" type="submit" value=":{editData}" class="btn">
			    <input type="button" value="_{cancel}" class="btnCancel right10" />
			    <label><input name="saveContentPic" type="checkbox" />保存远程图片</label>
			</td>
		</tr>
	</table>
	</div>
	
	<div id="content2" class="tabMain" style="display:none;width:98%;">
	<table style="width: 99%;margin:auto;">

	    <tr><td class="pubLeft">_{tag}</td><td><input name="TagList" type="text" id="tagList" style="width:300px;" value="#{post.TagList}"> <span class="note left5" title="">_{tagTip}</span></td></tr>
		<tr>
			<td class="pubLeft">_{summary}</td>
			<td><textarea name="Summary" style="width: 600px; height: 80px">#{post.Summary}</textarea></td>
		</tr>
        <tr><td></td><td>&nbsp;</td></tr>
	    <tr><td class="pubLeft">_{closeComment}</td><td>#{post.IsCloseComment}	    	    
	    </td></tr>
        <tr>
            <td class="pubLeft">查看次数</td>
            <td><input name="Hits" type="text" style="width:30px" value="#{post.Hits}" /></td>
        </tr>

		<tr>
		    <td class="pubLeft">图片大小</td>
		    <td>:{width}<input id="width" name="Width" type="text" value="#{post.Width}" style="width:40px;" />px 
            <span class="left20">:{height}</span><input id="height" name="Height" type="text" value="#{post.Height}" style="width:40px;" />px</td>
		</tr>


        <tr><td></td><td>&nbsp;</td></tr>
		<tr>
		    <td class="pubLeft">跳转网址</td>
		    <td style="padding-bottom:5px;"><input name="RedirectUrl" type="text" style="width:480px;" value="#{post.RedirectUrl}" /><br />
            <span class="note">(如果设置，则文章被点击之后，不再显示内容，而是直接跳转到此网址)</span></td>
		</tr>
	    <tr>
		    <td class="pubLeft">_{titleHome}</td>
		    <td style="padding-bottom:5px;"> <input name="TitleHome" type="text" style="width:480px;" type="text" value="#{post.TitleHome}" ><br />
            <span class="note">(为了美化首页排版，你可以稍作编辑，比如精简字数、修饰标题等)</span>
            </td>
	    </tr>
        <tr>
            <td class="pubLeft">首页排序</td>
            <td><input name="OrderId" type="text" size="20" style="width: 30px" value="#{post.OrderId}">
            <span class="note">(可以调整文章在首页的顺序。数字越大越靠前)</span>
            </td>
        </tr>
		<tr>
			<td class="pubLeft">:{titleStyle}</td>
			<td><textarea name="Style" style="width: 600px; height: 50px">#{post.Style}</textarea><br />
			<span class="note">(:{cssRequired})</span>
			</td>
		</tr>
	
	</table>
	</div>

	<div id="content3" class="tabMain" style="display:none;width:98%;">
	<table style="width: 99%;margin:auto;">
	    <tr>
	        <td class="pubLeft">SEO<br />keywords</td>
	        <td><input name="MetaKeywords" type="text" style="width:600px;" value="#{post.MetaKeywords}"><br />
	        <span class="note">如果不填写，则使用tag</span>
	        </td>
	    </tr>
		<tr >
			<td class="pubLeft">SEO<br />description</td>
			<td>
			<textarea name="MetaDescription" style="width: 600px; height: 80px">#{post.MetaDescription}</textarea><br />
			<span class="note">如果不填写，则使用摘要</span>
			</td>
		</tr>
    </table>
    </div>

</form>
</div>
