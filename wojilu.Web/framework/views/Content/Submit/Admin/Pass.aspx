<div style="padding: 20px;">
    <form method="post" action="#{ActionLink}" class="ajaxPostForm">
    <div>
        <span style="font-weight:bold; color:blue;">通过</span>：<strong>#{post.Title}</strong></div>
    <div>
        给投递人的消息<span class="note">(可选)</span>：</div>
    <div>
        <textarea name="msg" style="width: 420px; height: 80px;"></textarea></div>
    <div>
        <input id="Submit1" type="submit" class="btn" value="允许本投递项" /></div>
    </form>
</div>
