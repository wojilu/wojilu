<style type="text/css">

#postContainer {background:#eee;}
.tdL {width:60px; text-align:right; padding-right:10px; font-weight:bold; padding-top:5px; vertical-align:top;}

</style>

<div style="clear:both"></div>
<div id="postContainer">

<div style="padding:30px;">

	<form action="#{ActionLink}" method="post" class="ajaxPostForm">
		<table style="width: 100%">
			<tr>
				<td class="tdL">_{pageTitle}</td>
				<td>
				<input name="Title" style="width: 500px;" type="text" value="#{title}" /><span class="valid border"></span></td>
			</tr>
			<tr>
				<td class="tdL">页面内容</td>
				<td>
<script type="text/plain" id="content" name="content">#{content}</script>
<script>
    _run(function () {
        wojilu.editor.bind('content').height(500).line(2).show();
    });
</script>
<span class="valid border" to="content"></span>
                </td>
			</tr>
			<tr>
				<td class="tdL">编辑原因</td>
				<td>
                    <input name="editReason" type="text" style="width:500px;" class="tipInput" tip="请输入编辑原因" />
                    <span class="valid border"></span>
                </td>
			</tr>
			<tr>
				<td></td>
				<td><input type="submit" value="_{editPage}" class="btn btn-primary" />
				<span id="btnCancel" class="btn left20">放弃返回</span>
				</td>
			</tr>
		</table>
	</form>
			
</div>
</div>

<script>
    _run(function () {
        wojilu.ui.pageReturn();
        wojilu.ui.tip();
        wojilu.ui.valid();
        wojilu.ui.ajaxPostForm();

        var pingServer = function () {
            $.post('#{pingUrl}'.toAjax(), function (data) {
            });
        };

        pingServer();

        setInterval(pingServer, 1000 * 30); // 每隔30秒刷新一下服务器

        $('#btnCancel').click(function () {
            $.post('#{cancelUrl}'.toAjax(), function (data) {
                wojilu.tool.forward('#{showUrl}', 0);
            });
            return false;
        });

    });
</script>