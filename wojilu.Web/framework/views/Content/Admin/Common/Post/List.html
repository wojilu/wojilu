<div>

<div style="margin:5px 10px; background:#f2f2f2; padding:5px 10px; border-radius:4px;">

<div style="float:left; margin-right:10px;"><strong>分类浏览：</strong></div>

<ul style="float:left;">
<!-- BEGIN cats -->
<li style="float:left; margin-right:10px;"><a href="#{x.data.SectionLink}">#{x.Title}</a></li>
<!-- END cats -->
</ul>

<div style=" clear:both;"></div>

</div>

<script>
    _run(function () {
        $('#btnTrans').click(function () {

            var choiceList = wojilu.ui.select();

            if (choiceList.length == 0) {
                alert(lang.exSelect);
                return false;
            }

            var oUrl = $('#lnkTrans').attr('href');
            if (oUrl.indexOf('?') <= 0) {
                var newUrl = oUrl + "?ids=" + choiceList;
                $('#lnkTrans').attr('href', newUrl).click();
            }
            else {
                $('#lnkTrans').click();
            }

        });
    });
</script>

<table cellspacing="0" cellpadding="4" border="0" style="width:98%;margin:10px auto;" id="dataAdminList" data-action="#{OperationUrl}">

    <tr class="adminBar">
    <td colspan="7">

        <div style="float:left;">
        
                <a href="#{addUrl}" class="frmBox"><img src="~img/add.gif" /> 添加文章</a>
            
                <span class="menuMore left20" list="batchAdmin">批量操作<img src="~img/downWhite.gif" />
                <ul id="batchAdmin" class="menuItems" style="width:130px">
                    <li class="btnCmd" style="" cmd="status_pick"><img src="~img/star.gif" /> 设为要闻</li>
                    <li class="btnCmd" style="" cmd="status_normal"><img src="~img/doc.gif" /> 设为普通</li>
                    <li class="btnCmd" style="" cmd="status_focus"><img src="~img/sticky2.gif" /> 设为头条</li>
                    <li class="btnCmd" style="" cmd="delete"><img src="~img/s/delete.png" /> 批量删除</li>
                </ul></span>

                <span class="left20" id="btnTrans" style=" cursor:pointer;"><img src="~img/combine.gif" /> 转移</span>
                <a href="#{lnkTrans}" class="frmBox" id="lnkTrans" data-scrolling="auto" style=" display:none;" xwidth=580 xheight=280 title="迁移数据">&nbsp;</a>
        
            </div>
            <div style="float:right; padding-right:10px;">
                <form method="get" action="#{ActionLink}">:{searchTitle}: <input name="q" type="text" value="#{searchKey}" /><input id="Submit1" type="submit" value="_{submit}" class="btn btns" /></form>
            </div>          



        </td>
    </tr>

	<tr class="tableHeader">
	    <td align="center" style="width:4%;"><input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" /></td>
		<td style="width:5%;">投递人</td>
		<td style="width:3%;"></td>
		<td style="width:5%;"></td>
		<td style="">_{title}</td>
		<td style="width:13%;">_{created}</td>
		<td style="width:12%;">_{admin}</td>
	</tr>
	<!-- BEGIN list -->
	<tr class="tableItems">
	    <td align="center"><input name="selectThis" id="checkbox#{post.Id}" type="checkbox" class="selectSingle"> </td>
		<td>
        <div style="width:60px; overflow:hidden; text-overflow:ellipsis; white-space:nowrap; ">
        #{post.Submitter}
        </div>
        </td>
		<td>#{post.PickIcon}</td>
		<td style="text-align:right">#{post.AttachmentIcon}#{post.ImgIcon}</td>
		<td>
        <div style="width:500px; overflow:hidden; text-overflow:ellipsis; white-space:nowrap; ">
            <a href="#{post.Link}" style="#{post.TitleCss}" target="_blank">#{post.Title}</a>
		     <span id="tag#{post.Id}" postId="#{post.Id}" style="font-weight:bold;margin-left:10px; color:Blue">#{post.Tag.TextString}</span> <span class="cmdTag link"><img src="~img/s/tag_edit.png" /></span>
         </div>
		</td>
		<td>#{post.PubDate}</td>
		<td>
		<a href="#{post.EditUrl}" class="frmBox"><img src="~img/edit.gif" /> _{edit}</a>
		<span class="menuMore left5" list="adminPostMenus#{post.Id}">更多<img src="~img/down.gif" />
		<ul class="menuItems" id="adminPostMenus#{post.Id}" style="width:120px;">
		    <li></li>
		    <li><a href="#{post.AttachmentLink}" class="frmBox link" title="[附件]_#{post.Title}" xwidth="750"><img src="~img/attachment.gif" /> 附件(<a href="#{post.Link}">#{post.Attachments})</a></a></li>
		    <li><a href="#{post.DeleteUrl}" class="deleteCmd"><img src="~img/s/delete.png" /> _{delete}</a></li>
		    <li><a href="#{post.EditTitleStyleUrl}" class="frmBox" xwidth="700" title="[标题样式]_#{post.Title}"><img src="~img/edit.gif" /> 标题样式</a></li>
		    <li><a href="#{post.HtmlUrl}" target="_blank"><img src="~img/s/external-link.png" /> 静态网址</a></li>
		</ul></span>
		
		</td>
	</tr>
	<!-- END list -->
	<tr>
	    <td colspan="7">
        <table style="width:100%;">
            <tr>
                <td>#{page}</td>
	            <td style="text-align:right;padding-right:10px;">图例说明：	        
	                <img src="~img/star.gif" class="left10" /> 要闻
	                <img src="~img/sticky2.gif" class="left10" /> 头条
	                <img src="~img/img.gif" class="left10" /> 图片
	                <img src="~img/attachment.gif" class="left10" /> 附件
	                <img src="~img/s/tag_edit.png" class="left10" /> 修改tag
	            </td>
            </tr>
        </table>
        

	</tr>
</table>

<style>
#tagBox div { margin:5px 0px;}
</style>

<div id="tagBox" class="ebox" style="width:280px; padding:10px; ">
    <div>请输入tag(多个tag之间用逗号分隔)</div>
    <div><input id="tagValue" type="text" style="width:250px;" /></div>
    <div><input id="btnSaveTag" type="button" value="_{submit}" class="btn" />
        <input id="tagCancel" type="button" value="_{cancel}" class="btnCancel" />
        <input id="tagId" type="hidden" />
    </div>
</div>

</div>



<script>

_run( function() {

    $('.cmdTag').click( function() {
	    var ps = $(this).position(); // 获取当前被点击的元素的位置
	    $('#tagBox').css( 'top', ps.top+20 ).css( 'left', ps.left ).toggle('fast'); // 通过绝对定位来显示图层
	    var tagValue = $(this).prev().text();
	    $('#tagValue').val( tagValue );
	    $('#tagId').val( $(this).prev().attr( 'id' ) );
    });
    
    $('#tagCancel').click( function() {
        $("#tagBox").toggle('fast');
    });
    
    $('#btnSaveTag').click( function() {
        var tagValue = $('#tagValue').val();
        var tagSpan = $( '#'+$('#tagId').val() );
        var postId = tagSpan.attr( 'postId' );
        $.post( '#{tagAction}', {'postId':postId ,'tagValue':tagValue}, function(data) {
            if( 'ok'==data ) {
                tagSpan.text( tagValue );
                $("#tagBox").toggle('fast');
            }
            else
                alert( data );
        });
    });
    
    //
    $('.turnpage a').click( function() {
        $(this).attr( 'href', $(this).attr('href')+'#');
    });

});

</script>