<div style="width:520px; height:300px; padding:20px;">
    <div style="color:#666;font-size:12px;">

    请每行输入一个用户的“<span style="color:Red; font-weight:bold;">用户名/密码/email</span>”(用户名和密码之间用斜线分割)，<br />比如“<span style="color:blue; font-weight:bold;">张三/123456/zhsang@126.com</span>”，一行只能一个用户。
    </div>

    <form method="post" action="#{ActionLink}">
        <div style="margin:10px 0px">
            <textarea name="txtUsers" style="width:490px;height:220px;"></textarea>
        </div>
        
        <div>
            <input id="Submit1" type="submit" value="添加用户" class="btn" />
            <input id="Button1" type="button" value="_{cancel}" class="btnCancel" />
        </div>

    </form>

</div>