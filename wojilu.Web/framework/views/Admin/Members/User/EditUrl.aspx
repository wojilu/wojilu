<div style="padding:20px; text-align:center; ">

<form method="post" action="#{ActionLink}" class="">

    <div class="warning" style="width:95%;">警告：本修改只对从未发布数据的新注册用户有效。<br />老用户会有历史数据不一致问题，后果自负。</div>

    <div style="margin-top:10px; margin-bottom:10px;">
        个性网址：<input type="text" name="userUrl" value="#{userUrl}" />
    </div>

    <div>
        <button type="submit" class="btn">_{submit}</button>
        <button type="button" class="btnCancel">_{cancel}</button>
    </div>

</form>

</div>