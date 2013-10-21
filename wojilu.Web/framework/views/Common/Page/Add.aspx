<style type="text/css">
#postContainer {background:#eee;}
.tdL {width:80px; text-align:right; font-weight:bold; padding-right:10px; vertical-align:top; padding-top:5px;}
</style>

<div style="clear:both"></div>
<div id="postContainer">

    <div style="padding:30px;">

			<form action="#{ActionLink}" method="post" class="ajaxPostForm">
				<table style="width: 100%">
					<tr>
						<td class="tdL">_{pageTitle}</td>
						<td>
						<input name="Title" style="width: 500px;" type="text" value="" placeholder="请填写标题" />
                        <span class="valid border"></span></td>
					</tr>
					<tr>
						<td class="tdL">页面内容</td>
						<td>
                        
<script type="text/plain" id="Content" name="Content"></script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(500).line(1).show();
    });
</script>
<span class="valid border" to="Content"></span>
                        </td>
					</tr>
					<tr>
					    <td class="tdL">编辑原因</td>
					    <td>
                            <input name="editReason" type="text" style="width:500px;" value="创建页面" />
                            <span class="valid border"></span></td>
					</tr>
					<tr>
						<td></td>
						<td>
                        <button class="btn btn-primary" type="submit">
                            <i class="icon-pencil icon-white"></i>
                            创建页面
                        </button>

						<input class="btnReturn btn left10" type="button" value="_{return}" />
						</td>
					</tr>
				</table>
			</form>
			
		</div>
</div>

<script>
_run( function() {
    wojilu.ui.pageReturn();
    wojilu.ui.tip();
    wojilu.ui.valid();
    wojilu.ui.ajaxPostForm();

});
</script>