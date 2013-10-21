<style>
.radio input {width: 40px;}
.profileHeader {
	border-bottom: 1px #aaa dotted;
	font-family: Microsoft YaHei, Calibri;
	font-size: 18px;
	font-weight: bold;
	padding: 10px 5px 6px 0px;
}
td.profile_panel {	padding-top:10px; padding-left:10px;}
.profile_panel table {border: 0px #fff solid; }
.profile_panel table td {border-bottom: 1px #fff dotted;}
.interesting {width: 500px;	height: 50px;}
td.tdLeft {width:100px;text-align: left;}


.tabList li{ width:80px;}
</style>

<div style="margin:20px 20px 0px 20px; width:95%;">

<div style="font-weight:bold; font-size:14px; margin:20px 10px 15px 10px;"><img src="~img/user.gif" /> _{profile}</div>      

<ul class="tabList clearfix">
<li class="firstTab"><a href="#{viewer.ProfileUrl}" class="frmLink" loadTo="tabMain" nolayout=4>_{profile}</a><span></span></li>
<li><a href="#{viewer.AccountBind}" class="frmLink" loadTo="tabMain" nolayout=4>帐号绑定</a><span></span></li>
<li style="width:50px;"><a href="#{viewer.FaceUrl}" class="frmLink" loadTo="tabMain" nolayout=4>_{face}</a><span></span></li>
<li><a href="#{viewer.ContactLink}" class="frmLink" loadTo="tabMain" nolayout=4>_{contact}</a><span></span></li>
<li><a href="#{viewer.InterestUrl}" class="frmLink" loadTo="tabMain" nolayout=4>_{interest}</a><span></span></li>
<li><a href="#{viewer.TagUrl}" class="frmLink" loadTo="tabMain" nolayout=4>标签tag</a><span></span></li>

<li style="width:100px;"><a href="#{viewer.PwdUrl}" class="frmLink" loadTo="tabMain" nolayout=4>_{changePwd}</a><span></span></li>
<li style="#{privacyLinkStyle}"><a href="#{viewer.PrivacyLink}"class="frmLink" loadTo="tabMain" nolayout=4>_{privacySetting}</a><span></span></li>
</ul>
</div>

<div class="tabMain" id="tabMain" style="width:96%">#{layout_content}</div>

<script>
_run( function() {
    wojilu.tool.makeTab( '.tabList', 'currentTab', '' );
});
</script>
