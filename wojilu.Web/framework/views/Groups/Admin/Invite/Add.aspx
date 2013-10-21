<div style="">
<div class="warning" style="margin:20px 0px 20px 40px; padding:5px 10px; ">注意：邀请一旦发送，不可取消。</div>
<form method="post" action="#{ActionLink}" style="margin:20px;" class="ajaxPostForm">
<table style="width:90%;">
    <tr><td style="width:60px; vertical-align:top;text-align:right;">受邀者
    <span class="valid" mode="border" to="receiver" ></span> 
    </td><td>
        <div><textarea id="receiver" name="receiver" style="width:99%; height:50px;"></textarea></div>
        <div style="margin:5px 0px 5px 0px;"><span href="#{lnkSelectFriends}" class="cmd frmBox" title="选择好友"><img src="~img/add.gif" /> 选择好友</span></div>

    </td></tr>
    <tr><td style="vertical-align:top; text-align:right;">邀请信息</td><td>
    
<script type="text/plain" id="msg" name="msg"></script>
<script>
    _run(function () {
        wojilu.editor.bind('msg').height(150).line(1).show();
    });
</script>
<span class="valid border" to="msg"></span>
    
    
    </td></tr>
    <tr><td></td><td><input id="Submit1" class="btn" type="submit" value="发送邀请" />
    
    
    </td></tr>
</table>


</form>
</div>

<script>



function fillUsers( users ) {	    
    
    var txtValue = $('#receiver').val();
    
    if( txtValue=='' ) {
        $('#receiver').val( users );
    }
    else {
        $('#receiver').val( txtValue + ','+users );
    }	    

}
</script>