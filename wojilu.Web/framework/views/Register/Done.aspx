
<div style="width:680px; background: #fff; margin: auto; padding: 30px 0px 50px 0px;">
	<div style="font-size: 16px; font-weight: bold; padding: 5px 10px; border-bottom: 1px #ccc solid; margin: 10px">
		免费注册</div>

    <div style="padding-left:20px;">
        <div class="lgTitle orange">您已注册成功，还差一步就可以开通帐号</div>

        <div>您的 <b>#{email}</b> 将收到一封注册确认邮件，请点击其中的 <b class="orange">确认链接 </b>，以开通您的帐号。</div>

        <div style="margin:10px 0px 10px 20px; ">
        <!-- BEGIN mailLink -->
        <a class="btnBig" style="font-size:20px; padding:5px 10px; font-weight:bold;width:250px;" href="#{goUrl}" target="_blank">
        <img src="~img/mail/mail.gif" /> 立即去邮箱激活帐号 &raquo;</a>
        <!-- END mailLink -->
        </div>
        <div style="margin-top:0px; font-size:12px; color:Red">
        如果您没有收到验证邮件，请稍等两三分钟，并检查 <strong>垃圾箱(spam)</strong> 中是否有激活邮件。<br />
        如果是qq等邮件，请在“<strong>设置->反垃圾->设置域名白名单</strong>”中将 #{domainName} 加入白名单。
        </div>
        <div style="margin:15px 0px 10px 30px; font-size:14px;">如果确实没有收到，您也可以尝试 <a href="#{resendLink}" target="_blank" style="font-weight:bold;">点击此处</a> 重发确认邮件。</div>


</div>
</div>

<style>
.lgTitle{padding:15px 10px 10px 0px; font-size:18px;font-weight:bold; }
.btn:hover {color:#fff;}
</style>



