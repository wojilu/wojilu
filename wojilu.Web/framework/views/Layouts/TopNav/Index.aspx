<div id="topNavWrap" style="">

<style>#topNav .ValidationText {width:80px;border:1px #ccc inset;}</style>
<table id="topNav" cellpadding="0" cellspacing="0" >
	<tr>
		<td id="topSiteName"><a href="#{site.Url}">#{site.Name}</a></td>
		<td id="topNavMenu" style="display:none; padding-left:10px;">
			<span id="topUserHome" class="menuMore" list="topAppList"><a href="#" class="viewerFeeds frmLink" loadTo="uaMain" nolayout=1>_{userStart}</a><img src="~img/down.gif"/>
            <ul id="topAppList" class="menuItems" style="width:120px; font-size:12px;">
                <li id="feedItem" style="padding-top:5px;"><a class="viewerFeeds" href="#"><img src="~img/app/s/feeds.png"/> 好友消息</a></li>
			    <li style="border-bottom:1px #ccc dotted;"><a href="#" id="myGroupsLink"><img src="~img/app/s/group.png"/> _{group}</a></li>

			    <li><a href="#" id="appAdminUrl"><img src="~img/app/s/settings.png"/> _{appAdmin}</a></li>
			    <li><a href="#" id="menuAdminUrl"><img src="~img/app/s/menus.png"/> _{menuAdmin}</a></li>
			    <li><a href="#" id="myUrlList"><img src="~img/app/s/links.png"/> _{adminSiteUrl}</a></li>
            </ul>
            </span>

			<span id="SpaceAdmin" class="left10"><a href="#" id="viewerSpace">_{mySpace}</a></span>
			<span class="left10"><a href="#" id="viewerMicroblogHome">微博</a></span>
			<a href="#" id="viewerTemplateUrl" class="frmLink left10" loadTo="uaMain" nolayout=1>_{skin}</a>			
			<a href="#" id="viewerFriends" class="frmLink left10" loadTo="uaMain" nolayout=1>_{friend}</a>			
            
            <span id="siteAdminCmd"></span>
		</td>
		
		<td>	
            <div id="topLoginForm" style="display:none; text-align:right;">
                <a href="#{loginLink}" class="left5 right5 strong">_{login}</a>
		        <a href="#{RegLink}" class="left5 right5 strong">_{register}</a>
		        <a href="#{resetPwdLink}">_{forgetPwd}</a>
            </div>

			

		<table id="userAccountTable" cellpadding="0" cellspacing="0" border="0" style="display:none;">
		    <tr>
		        <td style="width:180px; text-align:right;">
		            <div>
		            <span id="siteNotification"></span>
			        <a href="#" class="frmLink left10" id="msgLink" loadTo="uaMain" nolayout=1>_{msg}<span id="viewerNewMsgCount"></span></a>
			        <span id="nfMsg">&nbsp;</span>
			        </div>
			        
			        <div style="position:relative">
                    <div id="ntBox" style="position:absolute; text-align:left; z-index:999;left:80px;top:8px;">                        
                        <div id="myUploadDlg" class="hide">
                            <img src="~img/s/upload1.png" /> <a href="#" id="uploadAvatar" style="color:#e56f04;">上传头像</a> #{uploadCredit}
                             <span class="closeNt">×</span>
                        </div>
                        <div id="confirmEmailPanel" class="hide">
                            <img src="~img/mail/mail.gif" /> <a href="#" id="confirmEmailLink" style="color:#e56f04 !important;">激活邮件</a> #{emailCredit} <span class="closeNt">×</span>
                        </div>
                        <div id="viewerNewNotificationCount"></div>
                        <div id="viewerNewMicroblogAtCount"></div>
                    </div>
                    </div>
		        </td>
		        <td style="width:35px; text-align:right; padding-right:5px;">
		            <a href="#" id="viewerInviteLink" style="display:inline-block;margin-bottom:-2px; " class="frmLink" loadTo="uaMain" nolayout=1>_{invite}</a>		            
		        </td>
		        <td style="">
	                <a href="#" id="siteOnlineUrl">_{online}<span style="font-size:11px;">(<span id="siteOnlineCount"></span>)</span></a>
                </td>
		        <td style="width:30px;padding-right:6px; text-align:right;">
		        <a href="#" class="viewerFaceUrl frmLink" title=" _{uploadFace}" id="userTopNavPic" loadTo="uaMain" nolayout=1></a>		        
		        </td>
		        <td style="padding-right:5px; font-weight:bold;"><a href="#" id="viewerProfileName" title="_{profile}" class="frmLink" loadTo="uaMain" nolayout=1></a></td>

		        <td style="width:100px;">
			            <span id="AccountAdmin" class="menuMore left5" list="accountItems">_{account}<img src="~img/down.gif"/>
			                <ul id="accountItems" class="menuItems" style="width:120px;">
				                <li><a href="#" id="viewerProfileUrl" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/base.png"/> _{profile}</a></li>
                                <li><a href="#" id="viewerBindUrl" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/avatar.png"/> 帐号绑定</a></li>
                                <li><a href="#" class="viewerFaceUrl frmLink" loadTo="uaMain" nolayout=1><img src="~img/img.gif"/> _{uploadFace}</a></li>
				                <li><a href="#" id="viewerPwdUrl" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/pwd.png"/> _{changePwd}</a></li>
				                <li><a href="#" id="viewerContactLink" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/address.png"/> _{contact}</a></li>
				                <li><a href="#" id="viewerInterestUrl" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/interest.png"/> _{interest}</a></li>
				                <li><a href="#" id="viewerTagUrl" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/tag.png"/> 标签tag</a></li>
				                
               		            <li><a href="#" id="viewerSettings" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/privacy.png"/> _{privacySetting}</a></li>
				                <li><a href="#" id="viewerCurrency" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/credit.png"/> _{myIncome}</a></li>
			                </ul>
			            </span>            			
			            <span class="left10"><span href="#" id="logoutLink" class="postCmd link">_{logout}</span>			            
			            </span>
		            </ul>		        
		        </td>
		    </tr>
		</table>		

		</td>		
	</tr>
</table>

</div>

<script>
_run( function() {
    require( ["wojilu.core.sitetop"], function( topnav ) {
        topnav.init( '#{navUrl}');
    });
});
</script>
