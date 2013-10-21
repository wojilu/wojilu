<div style="width:98%; height:300px; padding:20px;">
    <div style="font-size:14px;">

    请每行输入一个用户的
    “<span style="color:blue; font-weight:bold;">用户名/密码/email</span>”
    或者
    “<span style="color:blue; font-weight:bold;">用户名/密码/email/<span style="color:#333;">友好网址</span></span>”
    
    (用户名和密码之间用斜杠/分割)，<br />比如
    “<span style="color:blue; font-weight:bold;">张三/111111/xx@126.com</span>”
    或者
    “<span style="color:blue; font-weight:bold;">张三/111111/xx@126.com/<span style="color:#333;">zhangsan</span></span>”
    ，一行只能一个用户。<br />
    <span style="color:#666;font-size:12px;">[说明]“友好网址”是指给用户分配的地址，比如 #{userUrlPrefix}<span style="color:#333; font-weight:bold;">zhangsan</span>#{urlExt}，如果不设置，则自动指定</span>
    </div>

    <form method="post" action="#{ActionLink}">
        <div style="margin:10px 0px 0px 0px;">
            <textarea name="txtUsers" style="width:95%;height:180px;"></textarea>
        </div>

        <div style="margin-bottom:10px;">
            <label><input name="chkInstallApp" type="checkbox" checked="checked" /> 给用户空间安装App</label>        
        </div>
        
        <div>
            <input id="Submit1" type="submit" value="添加用户" class="btn" />
            <input id="Button1" type="button" value="_{cancel}" class="btnCancel" />
        </div>

    </form>

</div>