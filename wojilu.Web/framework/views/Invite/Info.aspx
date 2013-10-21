

<div style="width:90%; margin:auto;">
<div style="padding:40px 50px;">

    <div style="border-bottom:1px #ccc solid; width:100%; padding:5px 10px; font-size:14px;">好友邀请</div>

    <table style="width:98%; margin:20px auto;">
        <tr>
            <td style="width:100px; vertical-align:top;">
                <div style="text-align:center; margin-bottom:5px; font-weight:bold;">#{userName}</div>
                <div><img src="#{userPic}" /></div>
            </td>
            <td style="vertical-align:top;">
                <div style="border:0px #ccc solid; padding:2px 10px 10px 20px; ">
                    <div style="font-weight:bold; font-size:14px; color:Red;">欢迎光临！#{userName}邀请您加为好友。</div>
                    <div style="background2:#f2f2f2; padding:0px; margin:5px 0px 20px 0px;" class="note">
                    加为好友之后，您可以了解好友的最新动态消息，接收到好友分享的文章、博客、照片、视频，还可以和好友互动，结交更多的朋友。
                    </div>
                    <div><form action="#{ActionLink}" method="post">
                        <input id="Button1" type="submit" class="btn" style="" value="接受邀请，注册成为#{userName}的好友" />
                        <input id="Hidden1" name="inviteCode" type="hidden" value="#{inviteCode}" />
                        <a href="#{loginLink}" class="left10">已有帐号，我要登录</a>
                        </form>
                    </div>
                    
                    
                </div>            
            </td>
        </tr>
    </table>
</div>
</div>

