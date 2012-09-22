		
<script type="text/javascript">
<!--
_run( function() {


var setUploadImg = function( imgUrl, width, height ) {

    if( !imgUrl ) return;

    $('#width').val( width );
    $('#height').val( height );

    $('#imgLink').val( imgUrl ).hide();
    var imgHtml ='<img src="'+imgUrl+'" style="width:100px; height:100px;" /> '+
        '<img id="cmdDeleteUpload" src="~img/delete.gif" style="cursor:pointer" />';
    $('#uploadImgShow').html( imgHtml );

    $('#cmdDeleteUpload').click( function() {
        $('#imgLink').val('');
        $('#uploadImgShow').html('');
        $.post( '#{deleteUploadLink}', {'imgUrl':imgUrl}, function(data) {
            if( 'ok' != data ) alert( data );
        });
    });    
}

setUploadImg( '#{post.ImgLink}' );


//----------------------------------------------------------------------

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
	
});
//-->
</script>

<style>
.postTitleTd {width:480px;}
.postTitle {width: 460px}
#srcLink {width:220px;}

.tabItem { cursor:pointer;}
.tabMain {padding:10px 0px;}

td.pubLeft { text-align:right; width:80px; background:#eceff5; font-weight:bold; vertical-align:top;}
#postSection label {margin-right:10px;}
label.clickLabel { background:#ffe45c; }

.tabList {border-bottom: 1px solid #B8D5FF;}
</style>

<div style="width:780px;">
<form method="post" action="#{ActionLink}" style="padding:10px;">
	
	<ul class="tabList">
	<li id="tab1" class="firstTab currentTab" style="margin-left:30px; width:130px;"><a href="javascript:void(0);">:{general}</a><span></span></li>
	<li id="tab2" style="width:130px;"><a href="javascript:void(0);">:{advanced}</a><span></span></li>
	<div class="clear"></div>
	</ul>
	
	<div id="content1" class="tabMain" style="width:98%;">
	<table style="width: 99%;margin:auto;">

		<tr>
			<td class="pubLeft"">_{title}</td>
			<td class="postTitleTd"><input name="Title" id="title" type="text" value="#{post.Title}" class="tipInput postTitle" tip="_{exTitle}"><span class="valid" mode="border"></span>
			</td>
			<td rowspan="5" id="uploadImgShow">&nbsp;</td>
		</tr>				
	    <tr>
		    <td class="pubLeft">_{titleHome}</td>
		    <td> <input name="TitleHome" class="postTitle" type="text" value="#{post.TitleHome}" ></td>
	    </tr>
		<tr>
		    <td style="vertical-align:top;" class="pubLeft">所属区块</td>
		    <td id="postSection">#{postSection}</td>
		</tr>

		<tr>
			<td class="pubLeft">_{author}</td>
			<td><input name="Author" type="text" value="#{post.Author}">
			<span style="margin-left: 10px">:{srcUrl}</span> <input name="SourceLink" type="text" size="20" id="srcLink" value="#{post.SourceLink}">
			</td>
		</tr>
		
		<tr>
		    <td class="pubLeft">时间</td>
		    <td>
		        <input name="Created" type="text" class="right20" value="#{post.Created}" />
		        查看<input name="Hits" type="text" style="width:30px" value="#{post.Hits}" />
		        <span class="left20 note">文章属性</span>
		        #{PickStatus}
		    </td>
		</tr>

		<tr>
			<td class="pubLeft">_{content}<br /><span to="Content" class="valid" mode="border"></span></td>
			<td colspan="2">#{Editor}</td>
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
		<tr>
		    <td class="pubLeft">跳转网址</td>
		    <td><input name="RedirectUrl" type="text" style="width:600px;" value="#{post.RedirectUrl}" /></td>
		</tr>
	    <tr><td class="pubLeft">_{closeComment}</td><td>#{post.IsCloseComment}	    
	    <span class="left20 strong">_{orderId}</span>
	    <input name="OrderId" type="text" size="20" style="width: 30px" value="#{post.OrderId}">
	    </td></tr>
	    <tr><td class="pubLeft">_{tag}</td><td><input name="TagList" type="text" id="tagList" style="width:600px;" value="#{post.TagList}"><br /><span class="note" title="">_{tagTip}</span></td></tr>
		<tr>
			<td class="pubLeft">_{summary}</td>
			<td><textarea name="Summary" style="width: 600px; height: 80px">#{post.Summary}</textarea></td>
		</tr>
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
		<tr>
			<td class="pubLeft">:{picUrl}</td>
			<td>
			<input name="ImgLink" type="text" style="width: 600px" value="#{post.ImgLink}">
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

	
</form>
</div>

