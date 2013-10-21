<style>
.currentNav { background:#fff;}
.mbStats {width:90%;margin:5px auto;}
.mbStats td {padding:0px 3px; text-align:center;}
.mbStatsNo {font-size:12px; font-weight:bold;width:33%; }
</style>

<table id="uaContainer" cellpadding="0" cellspacing="0" border="0" >
	<tr>
		<td id="sidebar">
		
		    <table style="width:100%; margin:10px auto 20px auto;" border="0" cellpadding="0" cellspacing="0">
                <tr><td colspan="2" style=" text-align:center; padding-bottom:5px;">
		            <div><a href="#{user.HomeLink}" title="_{profile}" style="font-weight:bold;font-size:16px;" class="frmLink" loadTo="uaMain" nolayout=1>#{owner.Name}</a></div>
		            <div class="hide"><a href="#{owner.EditProfile}" title="_{profile}" style="color:#666;" class="frmLink" loadTo="uaMain" nolayout=1>编辑资料</a></div>
                </td></tr>
		        <tr><td style="text-align:center;">
		            <a href="#{user.HomeLink}" title="" class="frmLink" loadTo="uaMain" nolayout=1><img src="#{owner.Pic}" style="border:1px #ccc solid; padding:3px; border-radius:3px;width:120px;height:120px;" /></a>
		        </td>
                </tr>
                <tr><td colspan="2">
		        <table style="" cellpadding="0" cellspacing="0" class="mbStats">
                    <tr>
                        <td class="mbStatsNo" style="border-right:1px #aaa solid; "><a href="#{user.FollowingLink}" target="_blank">#{user.FollowingCount}</a></td>
                        <td class="mbStatsNo" style="border-right:1px #aaa solid;"><a href="#{user.FollowerLink}" target="_blank">#{user.FollowersCount}</a></td>
                        <td class="mbStatsNo" style=""><a href="#{user.HomeLink}">#{user.MicroblogCount}</a></td>
                    </tr>
                    <tr>
                        <td style="border-right:1px #aaa solid;"><a href="#{user.FollowingLink}" target="_blank">_{follow}</a></td>
                        <td style="border-right:1px #aaa solid;"><a href="#{user.FollowerLink}" target="_blank">_{fans}</a></td>
                        <td style=""><a href="#{user.HomeLink}">_{microblog}</a></td>
                    </tr>
                </table>
                </td></tr>
		        <tr><td colspan="2" style="text-align:center; padding-top:5px;" class="userToolbar">		        
		            <a href="#{owner.EditPic}" title="_{uploadFace}" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/avatar.png"/></a>
                    <a href="#{owner.EditProfile}" title="_{profile}" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/base.png"/></a>                    
		            <a href="#{owner.EditContact}" title="_{contact}" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/address.png"/></a>		            
		            <a href="#{owner.EditPwd}" title="_{changePwd}" class="frmLink" loadTo="uaMain" nolayout=1><img src="~img/s/pwd.png"/></a>
		        </td></tr>
		    </table>
		    <div style="border-top: 1px solid white;border-bottom: 1px solid #EBF1FA;"></div>
		    
			<ul style="margin-top:0px;" class="sidebar-list">			
			    <!-- BEGIN apps --><li style="border-top: 1px solid white;border-bottom: 1px solid #EBF1FA;">#{app.NameAndUrl}</li><!-- END apps -->    			
			    <li style="#{groupLinkStyle}"><a href="#{groupLink}" class="frmLink" loadTo="uaMain" nolayout=3><img src="~img/app/m/group.png"/> _{group}</a></li>
			</ul>
		
			<ul id="appSetting" style="margin-left:2px;" class="sidebar-list">

			<li style="#{appAdminStyle}"><a href="#{appAdminUrl}" class="frmLink" loadTo="uaMain" nolayout=3><img src="~img/app/s/settings.png"/> _{appAdmin}</a></li>
			<li style="#{menuAdminStyle}"><a href="#{menuAdminUrl}" class="frmLink" loadTo="uaMain" nolayout=3><img src="~img/app/s/menus.png"/> _{menuAdmin}</a></li>
			<li style="#{myUrlStyle}"><a href="#{myUrlList}" class="frmLink" loadTo="uaMain" nolayout=3><img src="~img/app/s/links.png"/> _{adminSiteUrl}</a></li>
				
			</ul>
		
		</td>
		<td id="uaMain">#{layout_content}</td>
	</tr>
</table>

<script>
_run(function () {
    $('.sidebar-list a').click(function () {
        $('.sidebar-list li').removeClass('currentNav');
        $(this).parent().addClass( 'currentNav' );
    });
});
</script>