<style>
.tdL { text-align:right; color:#666; padding-right:8px;}
.pubLeft { text-align:right;padding-right:8px;vertical-align:top;}
</style>

<script>
    _run(function () {
        $('.tabList li a').click(function () {
            var tabId = wojilu.str.trimStart($(this).parent().attr('id'), 'tab');
            $('.tabMain').hide();
            $('#content' + tabId).show();
        });
    });
</script>

<div style="margin:15px;height:380px;">
<form method="post" action="#{ActionLink}" id="vform">

	<ul class="tabList">
	<li id="tab1" class="firstTab currentTab" style="margin-left:30px; width:130px;"><a href="#">:{general}</a><span></span></li>
	<li id="tab2" style="width:130px;"><a href="#">:{advanced}</a><span></span></li>
    <li id="tab3" style="width:130px;"><a href="#">SEO</a><span></span></li>
	</ul>

    <div style="margin-top:10px;">
	
	<div id="content1" class="tabMain" style="width:98%;">
    <table style="width: 98%;" border="0" cellpadding="0">

	    <tr class="videoPlay">
	        <td style="vertical-align:top;"><strong>快速获取：</strong></td>
	        <td style="vertical-align:top; padding-bottom:15px;">
	            <div class="note">请填写视频<span style="color:Red; font-weight:bold;">播放页网址</span>(支持优酷、土豆、56、酷六、新浪播客、凤凰视频)</div>	        
	            <div><input name="vPlayUrl" id="vPlayUrl" type="text" style="width:350px;" />
	            <input id="btnGet" type="button" value="自动获取视频信息" class="btn btns" /><span id="loadingInfo"></span></div>
	        </td>
	    </tr>


	    <tr><td><strong>或 直接填写：</strong></td><td></td></tr>
	    <tr class="videoDetail">
		    <td class="tdL">_{title}</td>
		    <td><input name="Title" id="vTitle" type="text" style="width: 350px" class="tipInput" tip="_{exTitle}"><span class="valid" mode="border"></span></td>
	    </tr>
	    <tr class="videoDetail">
		    <td class="tdL">flash的网址</td>
		    <td><input name="SourceLink" id="vSourceLink" type="text" style="width: 550px" class="tipInput" tip=":{addVideoUrlValid}"><span class="valid" mode="border"></span></td>
	    </tr>
	    <tr class="videoDetail">
		    <td class="tdL">:{picUrl}</td>
		    <td><input name="ImgLink" id="vImgLink" type="text" size="20" style="width: 550px" class="tipInput" tip=":{addVideoPicValid}"><span class="valid" mode="border"></span></td>
	    </tr>
	    <tr>
		    <td>&nbsp;</td>
		    <td>
		        <input name="Submit1" type="submit" value=":{addVideo}" class="btn">
		        <input type="button" value="_{cancel}" class="btnCancel" />
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
		<tr>
			<td class="pubLeft">_{summary}</td>
			<td>
			<textarea name="Summary" style="width: 550px; height: 50px"></textarea></td>
		</tr>
	    <tr>
            <td class="pubLeft">_{closeComment}</td><td>        
                <label class="checkbox"><input type="checkbox" name="IsCloseComment" value="1"> 关闭评论</label>   	    
	        </td>
        </tr>
        <tr>
            <td class="pubLeft">查看次数</td>
            <td><input name="Hits" type="text" style="width:30px" value="0" /></td>
        </tr>
		<tr>
		    <td class="pubLeft">发布时间</td>
		    <td>
		        <input name="Created" type="text" class="right20" value="#{created}" />
		    </td>
		</tr>
		<tr>
		    <td class="pubLeft">视频大小</td>
		    <td>:{width}<input id="width" name="Width" type="text" value="#{width}" style="width:40px;" />px 
            <span class="left20">:{height}</span><input id="height" name="Height" type="text" value="#{height}" style="width:40px;" />px</td>
		</tr>

        <tr><td></td><td>&nbsp;</td></tr>

		<tr>
		    <td class="pubLeft">跳转网址</td>
		    <td style="padding-bottom:5px;"><input name="RedirectUrl" type="text" style="width:480px;" value="" /><br />
            <span class="note">(如果设置，则文章被点击之后，不再显示内容，而是直接跳转到此网址)</span></td>
		</tr>

        <tr>
            <td class="pubLeft">首页排序</td>
            <td><input name="OrderId" type="text" size="20" style="width: 30px" value="">
            <span class="note">(可以调整文章在首页的顺序。数字越大越靠前)</span>
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
		<td style="width:100px">SEO Keywords</td>
		<td><input name="MetaKeywords" type="text" style="width: 550px" /></td>
	</tr>
	<tr>
		<td style="width:100px;vertical-align:top">SEO Description</td>
		<td style=""><textarea name="MetaDescription" style="width: 550px; height:30px;"></textarea></td>
	</tr>
    </table>
    </div>

    </div>
</form>
</div>

<script type="text/javascript">
<!--



_run( function() {

	
	$('#btnGet').click( function() {
	
	    var playUrl = $('#vPlayUrl').val();
	    if( playUrl == '' ) {
	        alert( '请填写播放页的网址(不是flash自身的网址)' );
	        $('#vPlayUrl').focus();
	        return;
	    }
	
        $('#loadingInfo').html( '<img src="~img/ajax/loading.gif"/>' );
        $.post( ('#{videoServiceUrl}?playUrl='+playUrl).toAjax(), function(data) {        
            
            if( data.Title ) $('#vTitle').val( data.Title ).click();
            if( data.FlashUrl ) $('#vSourceLink').val( data.FlashUrl ).click();
            if( data.PicUrl ) $('#vImgLink').val( data.PicUrl ).click();            
            
            $('#loadingInfo').html( '' );            
        });

	});

	
});
//-->
</script>