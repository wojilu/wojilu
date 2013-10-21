
    <style type="text/css">

.inviteTip {background:#; padding:5px 20px; margin-top:10px; }

.invitePanel {margin:20px 10px 40px 20px; background2:#f2f2f2; padding:0px; border:0px #aaa solid;}
    .inviteTitle {font-size:14px; font-weight:bold; margin:0px 0px 5px 0px; padding:5px 10px 5px 15px;    border-radius:4px;    }
    .inviteContent { padding:5px; padding-left:15px;}
    .inviteContent img {width:100px; height:100px;}
    
#myWordsPreview {color:Blue; font-weight:bold; margin:5px 0px;}
    </style>

<div style="margin:15px 10px 0px 25px; font-size:14px; font-weight:bold;"><img src="~img/m/invite.png" /> 邀请好友</div>

<div class="invitePanel">
    <div class="inviteTitle cbg">方法1：直接发送邀请链接</div>
    <div class="inviteContent">
        <div>您可以按 ctrl+c 复制下面的链接，通过QQ、MSN等聊天工具，直接发送给您的好友。</div>
        <div style="margin-bottom:px;">
            <textarea id="txtInviteLink" style="width:90%; border:1px #ccc inset;">#{inviteLink}</textarea><a href="#{inviteLink}"></a></div>
    </div>
</div>

<div class="invitePanel">
    <div class="inviteTitle cbg">方法2：也可以给好友发送 Email 邀请</div>   
    <div class="inviteContent">
        <form action="#{ActionLink}" method="post" class="ajaxPostForm">
        <div style="margin-top:0px;">请填写好友的Email<span class="note left10">(多个Email之间用逗号,分开)</span></div>
        <div><textarea  name="EmailList" style="width:90%; height:80px;" cols="20" rows="1"></textarea></div>
        <div style="margin-top:10px;">说些什么吧</div>
        <div><input name="myWords" id="txtMywords" type="text" style="width:90%;" /></div>
        <div style="margin-top:10px;"><input id="Submit1" class="btn" type="submit" value="发送邀请" /></div>
        </form>
    </div>
    
    <div class="" style="width; margin:10px 0px 20px 10px;">
        <div style="padding-left:10px;"></div>
        <div style="width:90%;margin:5px;">
            <div class="inviteContent cbg" style="border-radius:4px;">
                <div style="color:Red; text-align:center; ">邀请信预览</div>
                #{previewHtml}
            </div>
        </div>
    </div>

</div>


<script>
_run( function() {

    $('#txtMywords').focus( function() {        
        setInterval( function() {
            $('#myWordsPreview').html( $('#txtMywords').val() );
        }, 100 );        
    });
    
    $('#txtInviteLink')[0].select();
    $('#txtInviteLink').click( function() {
        this.select();
    });
});
</script>



