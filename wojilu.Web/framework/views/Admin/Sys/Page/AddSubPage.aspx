
<div class="formPanel">
	<form action="#{ActionLink}" method="post" class="ajaxPostForm">
		<table style="width: 100%">
			<tr>
				<td style="width:60px;">_{pageTitle}</td>
				<td>
				<input name="Title" style="width: 447px;" type="text" value="" /><span class="valid border"></span></td>
			</tr>
			<tr>
				<td>_{parentPage}</td>
				<td>#{pageName} 
				<span class="left20">_{pageLogo} <input name="Logo" style="width: 150px;" type="text" value="" /></span></td>
			</tr>
			<tr>
				<td style=" vertical-align:top;">_{content}</td>
				<td style="width:800px;">

<script type="text/plain" id="Content" name="Content"></script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(350).line(2).show();
    });
</script>
<span class="valid border" to="Content"></span>
                </td>
			</tr>
			<tr>
				<td>编辑原因</td>
				<td>
                    <input name="editReason" type="text" style="width:500px;" value="创建页面" />
                    <span class="valid border"></span>
                </td>
			</tr>
					
			<tr>
				<td></td>
				<td><input type="submit" value="_{addPage}" class="btn" />
				<input class="btnReturn" type="button" value="_{return}" />
				<span class="left20"><input id="IsAllowReply" name="IsAllowReply" type="checkbox" checked="checked" /><label for="IsAllowReply">_{allowComment}</label></span>
				<input name="ParentId" type="hidden" value="#{parentId}" />
				</td>
			</tr>
		</table>
	</form>
</div>

