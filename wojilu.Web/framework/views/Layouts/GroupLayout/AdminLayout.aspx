<!DOCTYPE html>
<html lang="zh-CN">
<head>
<title>#{pageTitle}</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" /> 

<meta name="Keywords" content="#{pageKeywords}" />
<meta name="Description" content="#{pageDescription}" />

<link href="~css/wojilu._common.css?v=#{cssVersion}" rel="stylesheet" type="text/css" />
<link href="~css/wojilu._admin.css?v=#{cssVersion}" rel="stylesheet" type="text/css" />

#{siteSkinContent}
<style>#topNavWrap {background:url("~skin/site/topNavBg.png");}</style>

#{skinContent}
<style>
#groupPage {width:940px;margin:auto;}

 /*页脚*/
#group-footer-wrap {margin: auto; }
#group-footer { text-align:center;padding:20px ; }

#gAdminWrap{background:#eee;  line-height:150%; border-left:0px #aaa solid; border-right:0px #aaa solid; }
#gAdminWrap a {}

#gAdminHeader { padding:10px;border-bottom:1px #ccc solid;}
#gAdminLogo {width:100px; padding:10px 10px 10px 10px}
#gAdminTitle {font-size:24px;font-weight:bold;}
#gAdminGroupInfo {margin:0px 10px 10px 0px; padding:10px}

#gAdminMain { background:#fff;}

#gAdminTab a { color:#333;}
#gAdminTabMain {color:#333}
#gAdminTabMain a{color:#333}

#sidebar ul {margin-left:10px; line-height:200%; font-size:14px;}
#gAppList li {padding-left:18px;background:url("~img/arrowRight.gif") no-repeat left center; display:block;}

#gAdminMenuLabel { padding:15px 5px 5px 15px; font-weight:bold;font-size:14px;}


div.tabMain {border-left:0px #cdcdcd solid;border-right:0px #cdcdcd solid;border-bottom:0px #cdcdcd solid;}td.otherTab {border-bottom:1px #cdcdcd solid;}td.currentTab {border-left:1px #cdcdcd solid;border-right:1px #cdcdcd solid;border-top:1px #cdcdcd solid;}
</style>
#{pageRssLink}

<script>var __funcList = []; var _run = function (aFunc) { __funcList.push(aFunc); }; var require = { urlArgs: 'v=#{jsVersion}' };</script>
</head>

<body>

#{topNav}

    
<div id="groupPage">
    <div id="gAdminWrap">

	    <div id="gAdminHeader">
	        <table style="width:100%;">
	            <tr>
	                <td id="gAdminLogo"><a href="#{groupLink}"><img src="#{group.Logo}" style="width:80px;"/></a></td>
	                <td id="gAdminGroupInfo">
			            <div id="gAdminTitle">_{adminGroup}: <a href="#{groupLink}">#{groupName}</a>
			            <a href="#{groupLink}" style="font-size:14px; font-weight:normal; margin-left:15px;">&raquo;
 进入小组首页</a>
			            </div>
			            <div class="note" style="margin-top:10px;">
				            <div>_{administrator}: #{g.OfficerList}</div>
				            <div>#{groupStats}</div>
			            </div>
	                </td>
	            </tr>
	        </table>	    
	    </div>

	    <table style="width: 100%;background:#fff;" cellpadding="0" cellspacing="0">
		    <tr>
			    <td style="width:120px;background:#f2f2f2; border-right:1px #ccc solid; padding:10px 5px 20px 5px; vertical-align:top" id="sidebar">
			    <ul style=" ">
			        <li style="margin-bottom:10px;"><a href="#{groupLink}"><img src="~img/home.gif"/> 小组首页</a></li>
				    <li><a href="#{lnkBaseInfo}"><img src="~img/app/s/settings.png"/> _{baseInfo}</a></li>
				    <li><a href="#{lnkLogo}"><img src="~img/s/color.png"/> Logo</a></li>
				    <li><a href="#{lnkMember}"><img src="~img/s/users.png"/> _{memberAdmin}</a></li>
				    <li><a href="#{lnkInvite}"><img src="~img/addUser.gif"/> 邀请加入</a></li>
				    <li><a href="#{lnkSkin}"><img src="~img/s/interest.png"/> _{skin}</a></li>
				    <li><a href="#{lnkFriends}"><img src="~img/s/external-link.png"/> _{friendGroup}</a></li>				
			    </ul>
    			
			    <div id="gAdminMenuLabel">_{adminApp}</div>
			    <ul style="margin-left:10px" id="gAppList">
				    <!-- BEGIN apps --><li>#{app.NameAndUrl}</li><!-- END apps -->
			    </ul>
			    
			    <ul style="margin-top:10px;">
				    <li><a href="#{lnkAppAdmin}"><img src="~img/app/s/settings.png"/> _{appAdmin}</a></li>
				    <li><a href="#{lnkMenuAdmin}"><img src="~img/app/s/menus.png"/> _{menuAdmin}</a></li>
				    <li><a href="#{urlList}"><img src="~img/app/s/links.png"/> _{adminSiteUrl}</a></li>
    			
			    </ul>
			    </td>
			    <td style="vertical-align:top">#{layout_content}</td>
		    </tr>
	    </table>
    	
	    <div id="group-footer-wrap">
		    <div id="group-footer">
			    <div>copyright &copy <a href="#{groupLink}">#{groupName}</a> 20011-2012</div>
			    <div>Powered by <a href="#{sitelink}">#{sitename}</a></div>
		    </div>
	    </div>
    	
    </div>
</div>

<script data-main="~js/main" src="~js/lib/require-jquery-wojilu.js?v=#{jsVersion}"></script>
<script>require(["wojilu._admin"])</script>
#{statsJs}

</body>
</html>
