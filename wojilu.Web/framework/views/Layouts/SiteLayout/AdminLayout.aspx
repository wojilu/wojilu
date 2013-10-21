<!DOCTYPE html>
<html lang="zh-CN">
<head>
<title>#{pageTitle}</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="Pragma" content="no-cache" />
<meta http-equiv="Expires" content="-1">
<link href="~css/wojilu._common.css?v=#{cssVersion}" rel="stylesheet" type="text/css" />
<link href="~css/wojilu._admin.css?v=#{cssVersion}" rel="stylesheet" type="text/css" />

<script>var __funcList = []; var _run = function (aFunc) { __funcList.push(aFunc); }; var require = { urlArgs: 'v=#{jsVersion}' };</script>

<script type="text/javascript">
    _run( function() {
        $('.adminContainer').height( $(window).height()-$('#headerWrap').height() -2  );
	    setInterval( function() { $.post( '#{lostPage}', function() {});}, 1000*60*10 );	
	    
	    $('#appAdminItem').click( function() {
	        $('#appAdmin').mouseover();
	    });
	    
	    $('#userDataAdminItem').click( function() {
	        $('#userDataAdmin').mouseover();
	    });
	    
    });
</script>

<style>

#sidebarApps li {margin:0px 0px 0px 0px; padding-left:20px; height:38px;}
#sidebarApps li a { border:0px;}
#sidebarApps li a:hover { background:#fff; }
#sidebarApps li:hover { background:#fff; }

* html #sidebarApps {margin-left:20px;}
* html #sidebarApps li {padding-left:0px;}

#adminMenuList .menuItems { background:#fff;}
#adminMenuList .menuItems li a{ color:#005eac;}
#adminMenuList .menuItems li { float:none; text-align:left; padding-left:20px; font-size:14px; font-weight:bold; margin:5px 0px 5px 0px;}

</style>

</head>

<body>

<!--logo-->
<table style='width:100%; height:38px; background:url("~img/admin/menu-bg-big.gif"); '>
    <tr>
       <td style="padding-left:20px;"><img src="~img/admin/logo.jpg" /></td>
    </tr>
</table>

<!--navMenu-->
<div id="adminHeaderWrap">

    <div id="adminMenuWrap">

	    <div id="adminMenu">
	    
	        <!--menus-->
		    <div style="float:left">
		    <ul id="adminMenuList">#{menus}</ul>			
		    </div>
		    
		    <!--login/logout-->
		    <div style="float:right; padding-top:12px;text-align:center;">
		        <div style="margin-bottom:3px;">_{hello}, #{viewer.Name}</div>
		        <div>
		            <a href="~/" class="right10"><img src="~img/home.gif" />_{siteHome}</a>
		            <a href="#{site.LogoutLink}"><img src="~img/s/logout.png" />_{logoutAdmin}</a>
		        </div>		
		    </div>
		    <div class="clear"></div>
	    </div>

    </div>
</div>

<div id="adminMain" style="margin:0px;padding:0px;">

<table border="1" cellpadding="0" cellspacing="0" class="adminContainer">
	<tr>
		<td class="adminSidebar" style="width:180px;">
			<div class="adminSidebarTitle"><div class="adminSidebarTitleInternal">_{adminApp}</div></div>

            <ul>
                <li style="#{appHide}">
                    <a href="#{lnkAppList}"><img src="~img/s/settings.png"> 程序列表</a> 
                    <a href="#{lnkAppAdd}" class=""><img src="~img/add.gif"> 添加</a>
                </li>
                <li style="#{menuHide}">
                    <a href="#{lnkMenuList}"><img src="~img/s/menus.png"> 菜单列表</a> 
                    <a href="#{lnkMenuAdd}" class=""><img src="~img/add.gif"> 添加</a>
                </li>

                <li style="#{appConfigHide}"><a href="#{lnkAppConfig}"><img src="~img/s/cache.png"> App和组件配置</a></li>
            </ul>

			<div style="border-bottom:1px #aaa dotted; margin:10px 0px 5px 0px;"></div>

			<ul id="sidebarApps"><!-- BEGIN apps --><li>#{app.NameAndUrl}</li><!-- END apps -->
			</ul>
			
			<div style="border-bottom:1px #aaa dotted; margin:10px 0px 5px 0px;"></div>

			<ul>
                <!--------------------jquery chrome bug : last node----------------------->
			    <!-- BEGIN siteDataAdmin -->
			    <li><a href="#{m.Link}" class="frmLink2" loadto="adminMainBody" nolayout=1><img src="~img/s/#{m.Logo}.png"/> #{m.Name}</a></li><!-- END siteDataAdmin -->

			</ul>
		</td>
		<td class="adminMain" id="adminMainBody">#{layout_content}</td>
	</tr>
</table>



</div>

<script data-main="~js/main" src="~js/lib/require-jquery-wojilu.js?v=#{jsVersion}"></script>
<script>require(["wojilu._admin"])</script>

</body>
</html>
