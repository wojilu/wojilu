
<div style="margin:30px 50px;">

    <div style="font-size:22px; border-bottom:1px solid #ccc; padding:10px; margin-bottom:15px;">审核用户头像</div>

    <style>
    .tdL { text-align:right; vertical-align:top;}
    </style>

	<form action="#{ActionLink}" method="post" class="ajaxPostForm" style="padding-left:15px;">

        <table>

        <tr>
        <td class="tdL">用户名：</td><td><strong>#{userList}</strong></td>
        </tr>

        <tr>
        <td class="tdL">审核信息：</td>
        <td>
        <textarea name="Msg" style="width: 380px; height:60px;"></textarea>
        <span class="valid border"></span>
        </td></tr>

        <tr>
        <td class="tdL">状态：</td><td>
            <label><input type="radio" name="IsPass" id="chkPass" value="1" /> 通过</label>
            <label class="left10"><input type="radio" name="IsPass" id="chkUnPass" value="0" checked="checked" /> 不通过</label>
        </td></tr>

        <tr><td colspan="2" style="height:5px;"></td></tr>

        <tr class="nextPanel">
        <td class="tdL">下次审核：</td><td><label><input type="checkbox" name="IsNextAutoPass" />
        用户下次上传，自动通过，不需审核</label></td></tr>      

        <tr class="nextPanel">
        <td class="tdL">删除头像：</td><td><label><input type="checkbox" name="IsDelete" />
        删除当前不符合要求的头像，使用系统默认游客头像</label></td></tr>

        <tr><td colspan="2" style="height:20px;"></td></tr>

        <tr><td></td>
        <td>
		    <input name="mybtn" type="submit" value="提交" class="btn">&nbsp;
		    <input name="Button1" type="button" value="_{return}" class="btnReturn" />
		    <input name="UserIds" type="hidden" value="#{userIds}" />
        </td></tr>

        </table>

	</form>	


</div>

<script>
    _run(function () {
        $('#chkPass').click(function () {
            $('textarea[name=Msg]').val('头像审核通过');
            $('.nextPanel').toggle();
        });

        $('#chkUnPass').click(function () {
            $('textarea[name=Msg]').val('头像不够美观、大方');
            $('.nextPanel').toggle();
        });
    });
</script>