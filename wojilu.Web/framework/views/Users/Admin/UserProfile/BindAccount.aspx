<div style="padding:30px;">

    <div style="margin:0px 10px 0px 10px;">
        <div><span style="font-size:14px;">绑定帐号可以把您的信息同步给<strong>其他网站</strong>的更多好友。</span><a href="#" id="tipLink">绑定帐号有什么用？安全吗？</a></div>
        <div style="display:none; " id="tipInfo">
            <div style="padding:10px 5px; border:1px solid #ccc; margin-top:10px; background:#f2f2f2;">

                <div style="margin:10px;">
                    <div style="margin:5px; font-weight:bold;font-size:14px;">绑定帐号有什么用？</div>
                    <div style="margin-left:5px;">1. 可以直接使用这些帐号登录本站。</div>
                    <div style="margin-left:5px; margin:5px;">2. 您在本站发布的微博等信息，<strong style="color:Blue;">在获得您的授权后，可以自动同步到其他网站</strong>(比如新浪微博、腾讯微博等大型SNS网站)。<strong style="color:Blue;">再也不用您手动拷贝粘贴</strong>，方便及时分享给其他网站的好友。</div>
                    
                </div>

                <div style="margin:20px 10px 10px 10px;">
                    <div style="margin:5px; font-weight:bold;font-size:14px;">安全吗？</div>
                    <div style="margin-left:5px;">放心，在绑定过程中，只有明确经过您的授权，帐号绑定才可以成功。绑定成功之后（比如新浪微博）如果您自己没有设定，也没有主动发布，本站不会向您的帐号推送任何广告或不良信息。</div>
                </div>

            </div>
        </div>
    </div>




    <style>
    .bindWrap {width:90%; margin:10px 10px 30px 0px;}
    .bindItem td { padding-top:20px; padding-bottom:20px; border-bottom:1px dotted #ccc;}
    .bindItem:hover { background:#f2f2f2;}
    .bindLogo { vertical-align:top; width:70px; text-align:right; padding-right:5px;}
    .bindInfo { vertical-align:top; padding-top:12px; padding-left:15px;}
    .bindInfo div{ margin-bottom:5px;}
    .bindInfo * { vertical-align:middle;}
    </style>

<table class="bindWrap" cellpadding="0" cellspacing="0">

    <!-- BEGIN list -->
    <tr class="bindItem">
        <td class="bindLogo"><img src="#{connect.Logo}" /></td>
        <td class="bindInfo">
            <div style="font-size:14px; font-weight:bold;">#{connect.Name}</div>
            <!-- BEGIN bind -->
            <div>
                <span>已绑定帐号</span> <strong>#{connect.Name}</strong><span class="hide">(#{connect.Uid})</span> 
                <span><a href="#{connect.UnBindLink}" class="deleteCmd cmd btns">解除绑定</a></span>
            </div>
            <div><label><input type="checkbox" class="checkSync" data-link="#{connect.SyncLink}" #{connect.CheckSync} /> 同步微博内容</label></div>
            <!-- END bind -->

            <!-- BEGIN unbind -->
            <div><a href="#{connect.BindLink}" target="_blank" style="font-size:14px; ">绑定帐号 <img src="~img/s/external-link.png" /></a></div>
            <!-- END unbind -->
        </td>
    </tr>
    <!-- END list -->
    
</table>


</div>


 <script>
     _run(function () {

         $('.checkSync').click(function () {
             var url = $(this).attr('data-link');
             var isSync = $(this).attr('checked') == 'checked' ? 1 : 0;
             var _this = $(this);
             _this.attr("disabled", "disabled");
             $.post(url.toAjax(), { 'isSync': isSync }, function (data) {
                 if (data != 'ok') {
                     alert('对不起，操作错误，请稍后再试。');
                 }
                 else {
                     _this.removeAttr('disabled');
                 }
             });
         });

         $('#tipLink').click(function () {
             $('#tipInfo').slideToggle('fast');
         });

     });
</script>