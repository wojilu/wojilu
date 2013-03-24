define([], function () {

    var _initTopnav = function (navUrl, loginAction, regLink, resetPwdLink) {

		var userAppMenus = '<ul class="nav-top" id="topNavMenu">'
		+ '	<li class="dropdown">'
		+ '		<a href="#" class="dropdown-toggle viewerFeeds2" data-toggle="dropdown" data-hover="dropdown">快速入口<span class="caret" style="border-top-color:#fff;"></span></a>'
		+ '		<ul class="dropdown-menu" style="min-width:120px;">'
		+ '			<li id="feedItem"><a class="viewerFeeds" href="#"><img src="'+wojilu.path.img+'/app/s/feeds.png"/> 好友消息</a></li>'
		+ '			<li><a href="#" id="myGroupsLink"><img src="'+wojilu.path.img+'/app/s/group.png"/> 群组</a></li>'
		+ '			<li class="divider"></li>'
		+ '			<li><a href="#" id="appAdminUrl"><img src="'+wojilu.path.img+'/app/s/settings.png"/> 程序管理</a></li>'
		+ '			<li><a href="#" id="menuAdminUrl"><img src="'+wojilu.path.img+'/app/s/menus.png"/> 菜单管理</a></li>'
		+ '			<li><a href="#" id="myUrlList"><img src="'+wojilu.path.img+'/app/s/links.png"/> 常用网址</a></li>'
		+ '		</ul>'
		+ '	</li>'
		+ '	'
		+ '	<li id="SpaceAdmin"><a href="#" id="viewerSpace">我的空间</a></li>'
		+ '	<li><a href="#" id="viewerMicroblogHome">微博</a></li>'
		+ '	<li><a href="#" id="viewerTemplateUrl" class="frmLink" loadTo="uaMain" nolayout=1>模板</a></li>'
		+ '	<li><a href="#" id="viewerFriends" class="frmLink" loadTo="uaMain" nolayout=1>好友</a></li>'
		+ '	<li id="siteAdminCmd"></li>'
		+ ''
		+ '</ul>';	
		
		
		var userAccountList = '<ul id="userAccountTable" class="nav-top" style="display: none;">'
		+'	<li id="siteNotification"></li>'
		+'	<li><a href="" class="frmLink" id="msgLink" loadTo="uaMain" nolayout=1>私信<span id="viewerNewMsgCount"></span></a></li>'
		+'	<li style="position:relative"><a href="#" id="nfMsg"></a>&nbsp;'
		+'	<div style="position:relative;">'
		+'		<div id="ntBox" style="position:absolute; text-align:left; z-index:999;left:-70px;top:12px;width:180px; ">'
		+'			<div id="confirmEmailPanel" class="hide">'
		+'				<img src="'+wojilu.path.img+'/mail/mail.gif" /> <a href="#" id="confirmEmailLink" style="color:#e56f04;">激活邮件</a> <span class="closeNt">×</span>'
		+'			</div>'
		+'			<div id="viewerNewNotificationCount"></div>'
		+'			<div id="viewerNewMicroblogAtCount"></div>'
		+'		</div>'
		+'	</div>'
		+'	</li>'
		+''
		+'	<li><a href="#" id="viewerInviteLink" class="frmLink" loadTo="uaMain" nolayout=1>邀请</a>	</li>'
		+'	<li><a href="#" id="siteOnlineUrl">在线<span style="font-size:11px;">(<span id="siteOnlineCount"></span>)</span></a></li>'
		+'	<li><a href="#" class="viewerFaceUrl frmLink" title="上传头像" id="userTopNavPic" loadTo="uaMain" nolayout=1></a></li>'
		+'	<li><a href="#" id="viewerProfileName" title="个人资料" class="frmLink" loadTo="uaMain" nolayout=1></a></li>'
		+''
		+'	<li id="topAccountInfo" class="dropdown">'
		+'		<a href="#" class="dropdown-toggle" data-hover="dropdown" data-toggle="dropdown" id="AccountAdmin">帐号<span class="caret" style="border-top-color:#fff;"></span></a>'
		+'	<ul class="dropdown-menu" style="min-width:120px;">'
		+'		<li><a href="#" id="viewerProfileUrl" class="frmLink" loadTo="uaMain" nolayout=1><img src="'+wojilu.path.img+'/s/base.png"/> 个人资料</a></li>'
		+'		<li><a href="#" id="viewerBindUrl" class="frmLink" loadTo="uaMain" nolayout=1><img src="'+wojilu.path.img+'/s/avatar.png"/> 帐号绑定</a></li>'
		+'		<li><a href="#" class="viewerFaceUrl frmLink" loadTo="uaMain" nolayout=1><img src="'+wojilu.path.img+'/img.gif"/> 上传头像</a></li>'
		+'		<li><a href="#" id="viewerPwdUrl" class="frmLink" loadTo="uaMain" nolayout=1><img src="'+wojilu.path.img+'/s/pwd.png"/> 修改密码</a></li>'
		+'		<li><a href="#" id="viewerContactLink" class="frmLink" loadTo="uaMain" nolayout=1><img src="'+wojilu.path.img+'/s/address.png"/> 联系方式</a></li>'
		+'		<li><a href="#" id="viewerInterestUrl" class="frmLink" loadTo="uaMain" nolayout=1><img src="'+wojilu.path.img+'/s/interest.png"/> 兴趣爱好</a></li>'
		+'		<li><a href="#" id="viewerTagUrl" class="frmLink" loadTo="uaMain" nolayout=1><img src="'+wojilu.path.img+'/s/tag.png"/> 标签tag</a></li>'
		+'		<li><a href="#" id="viewerSettings" class="frmLink" loadTo="uaMain" nolayout=1><img src="'+wojilu.path.img+'/s/privacy.png"/> 隐私设置</a></li>'
		+'		<li><a href="#" id="viewerCurrency" class="frmLink" loadTo="uaMain" nolayout=1><img src="'+wojilu.path.img+'/s/credit.png"/> 我的积分</a></li>'
		+'		<li class="divider"></li>'
		+'		<li><a href="#" id="logoutLink" class="postCmd link"><img src="'+wojilu.path.img+'/s/logout.png" /> 注销</a></li>'
		+'	</ul>'
		+'	</li>'
		+''
		+'</ul>';
		
		var loginForm = '  '
		+ '<div style="display:none;">'
		+ '<form id="loginForm" class="form-inline" action="'+loginAction+'" method="post">'
		+ '	<div style="padding:20px 0px;">'
		+ '	<table style="width:480px; " cellpadding="5">'
		+ '		<tr>'
		+ '			<td style="width:100px; font-size:14px; text-align:right;">用户名</td>'
		+ '			<td><input type="text" name="txtUid" placeholder="用户名" style="height:24px;margin:0px;font-size:14px;"><a href="'+regLink+'" class="left10 help-inline">用户注册</a></td>'
		+ '		</tr>'
		+ '		<tr>'
		+ '			<td style="width:100px; font-size:14px; text-align:right;">密　码</td>'
		+ '			<td><input type="password" name="txtPwd" placeholder="密码" style="height:24px;margin:0px; font-size:14px;"><a href="'+resetPwdLink+'" class="left10 help-inline">忘记密码</a></td>'
		+ '		</tr>'
		+ '		<tr>'
		+ '			<td></td>'
		+ '			<td><div id="loginValidBox" class="ebox" style="width:380px;padding:2px 5px; ">验证码 #{ValidationCode}</div></td>'
		+ '		</tr>'
		+ '		<tr>'
		+ '			<td><input type="hidden" name="returnUrl" id="Hidden1" value="" /></td>'
		+ '			<td>'
		+ '				<div class="controls">'
		+ '					<button type="submit" class="btn btn-primary" style="font-size:14px;">'
		+ '						<i class="icon-user icon-white"></i> 登录'
		+ '					</button>'
		+ '					<span class="loadingInfo"></span>'
		+ ''
		+ '					<label id="top-nav-login-form-remember" class="checkbox inline" style="width:120px;margin-left:10px;">'
		+ '						<input name="RememberMe" type="checkbox" id="chkRemember" checked="checked" /> 记住我'
		+ '					</label>'
		+ '				</div>'
		+ '			</td>'
		+ '		</tr>'
		+ '	  </table>'
		+ ''
		+ '	  <div id="loginBoxConnects" style="margin:15px 10px; border-top:1px dotted #aaa; padding-top:20px; display:none;">'
		+ '		<div style="margin-left:20px; font-size:14px; float:left;">使用其他帐号登录</div>'
		+ '		<ul id="loginBoxConnectsMenu" style="margin-left:10px; font-size:14px;" class="unstyled pull-left">'
		+ '		</ul>'
		+ '	  </div>'
		+ '	  </div>'
		+ '</form>'      
		+ '</div>';
	
		var loginSection = '<div id="loginSection" style=" vertical-align:middle; display:block; padding-top:5px; display:none; margin-right:5px;">'
		+ ''
		+ '	<div style="float:left;">'
		+ '	<span class="help-inline top-login-btn">'
		+ '		<a href="" id="loginLink" class="btn btn-primary btn-mini" style="font-size:12px;line-height:14px;padding:3px 6px 3px 6px;margin:0px;"><i class="icon-user icon-white"></i> 登录'
		+ '		</a>'
		+ '	</span>'
		+ ''
		+ '	<span class="help-inline top-reg-btn"><a href="'+regLink+'" class="btn btn-primary btn-mini right10" style="font-size:12px;line-height:14px;padding:3px 6px 3px 6px;margin:0px;"><i class="icon-play-circle icon-white"></i> 注册</a></span>'
		+ '	<span class="help-inline left5 top-forget-pwd"><a href="'+resetPwdLink+'">忘记密码</a></span>'
		+ '	</div>'
		+ '	<ul id="topConnects" class="nav-top" style="margin-left:5px; margin-right:0px; display:none;">'
		+ ''
		+ '		<li id="topConnectsMenuWrap" class="dropdown" style="margin-top:3px;">'
		+ '			<a href="#" class="dropdown-toggle" data-hover="dropdown" data-toggle="dropdown">'
		+ '				<span style="color:#333;">更多</span>'
		+ '				<span class="caret" style="border-top-color:#fff;"></span>'
		+ '			</a>'
		+ '			<ul id="topConnectsMenu" class="dropdown-menu"></ul>'              
		+ '		</li>'
		+ '	</ul>'
		+ ''
		+ '	<div style="clear:both;"></div>'
		+ '</div>';
	
        navUrl = (navUrl + '?url=' + window.location.href).toAjax();
        $('#topNavReturnUrl').val(window.location.href);	
		

		var bindLoginForm = function() {
		
			$('#loginLink').click(function () {
				wojilu.box.title('用户登录').width(500).height(280).id('loginForm').show();
				return false;
			});
			
			$('#loginForm .txtUid').click(function () {
				$(this).parent().append($('#loginValidBox'));
				var ps = $(this).position();
				$('#loginValidBox').css('top', ps.top + 20).css('left', ps.left).slideDown('fast');
			});


			$('#loginForm').submit(function () {
				var tform = this;

				if ($('#loginValidBox').length > 0 && $('#loginValidBox').css('display') == 'none') {
					$(this).append($('#loginValidBox'));
					var ps = $(tform).position();
					$('#loginValidBox').css('top', ps.top + 20).css('left', (ps.left + 50)).slideDown('fast');
					return false;
				}

				var btn = $(':submit', tform);
				btn.attr('disabled', 'disabled');
				var loading = $('.loadingInfo', tform);
				loading.html(' <img src="' + wojilu.path.img + '/ajax/loading.gif"/>');

				$.post($(tform).attr('action').toAjax(), $(tform).serializeArray(), function (data) {
					var result = data;

					if (result.IsValid) {
						wojilu.tool.forward(result.ForwardUrl, 0);
					} else {
						loading.html('');
						alert(result.Msg);
						btn.attr('disabled', false);
					};
				});

				return false;
			});
		
		};

        $('.closeNt').click(function () {
            $(this).parent().hide();
        });
		
		var showFormConnects = function () {
			var cid = 'loginBoxConnects';
			$('#' + cid).show();
			var xmenu = $('#' + cid + 'Menu');
			for (var i = 0; i < ctx.connects.length; i++) {
				var _cn = ctx.connects[i];
				xmenu.append('<li><a href="' + _cn.link + '"><img src="' + _cn.logos + '" /> ' + _cn.name + '</a></li>');
			}
		};
		
		var showTopConnects = function () {
			$('#topConnects').show();
			var xmenu = $('#topConnectsMenu');
			var xmenuWrap = $('#topConnectsMenuWrap');
			var moreCount = 0;
			for (var i = 0; i < ctx.connects.length; i++) {
				var _cn = ctx.connects[i];
				if (_cn.pick == 1) {
					xmenuWrap.before('<li style="margin-top:3px;"><img src="' + _cn.logos + '" /> <a href="' + _cn.link + '">' + _cn.lname + '</a></li>');
				}
				else {
					xmenu.append('<li><a href="' + _cn.link + '"><img src="' + _cn.logos + '" /> ' + _cn.lname + '</a></li>');
					moreCount++;
				}
			}
			if (moreCount == 0) xmenuWrap.hide();
		};

        $.get(navUrl, function (data) {

            ctx = data;
            var nav = ctx.navInfo;
			
			// 是否显示顶部导航栏
            if (nav.topNavDisplay == 1) { $('#topNav').hide(); return; }
			
			// 未登录状态
            if (ctx.viewer.IsLogin == false) {			
			
				// 显示登录链接
				$('#top-nav-right').html(loginSection);
                $('#loginSection').show();
				
				// 登录表单
				$('body').append(loginForm);
				bindLoginForm();
				
				// 登录验证码
                if (ctx.owner.LoginValidImg == false) {
                    $('#loginValidBox').remove();
                }

				// 第三方登录
                if (ctx.connects.length > 0) {
                    showTopConnects();
                    showFormConnects();
                }
				
				wojilu.ui.httpMethod('.navbar-inner');
				
                return;
            }
			
			// 显示：左侧app和用户的各种链接
			$('#user-app-menu').html( userAppMenus );
            $('#topNavMenu').show();
			
			// 显示：右侧帐号菜单
			$('#top-nav-right').html( userAccountList );
			$('#userAccountTable').show();

            var urls = ['shareLink', 'myGroupsLink', 'appAdminUrl', 'menuAdminUrl', 'myUrlList', 'viewerSpace', 'viewerMicroblogHome', 'viewerTemplateUrl', 'viewerFriends', 'viewerInviteLink', 'siteOnlineUrl', 'viewerProfileUrl', 'viewerBindUrl', 'viewerContactLink', 'viewerInterestUrl', 'viewerTagUrl', 'viewerPwdUrl', 'viewerSettings', 'viewerCurrency', 'logoutLink'];
            //var urls = ['shareLink', 'myGroupsLink', 'appAdminUrl','menuAdminUrl','myUrlList', 'viewerSpace', 'viewerMicroblogHome', 'viewerTemplateUrl', 'viewerFriends', 'viewerInviteLink', 'siteOnlineUrl', 'viewerProfileUrl', 'viewerContactLink', 'viewerInterestUrl', 'viewerTagUrl', 'viewerPwdUrl', 'viewerSettings', 'viewerCurrency'];
            for (var i = 0; i < urls.length; i++) { $('#' + urls[i]).attr('href', nav[urls[i]]); }

            if (nav.isUserHomeClose) {
                $('#topUserHome').hide();
                $('#topAppList').hide();
            }

            if (nav.isEnableUserSpace == false) { $('#viewerSpace').parent().hide(); }
            if (nav.isEnableGroup == false) { $('#myGroupsLink').parent().hide(); }
            if (nav.isMicroblogClose) { $('#viewerMicroblogHome').parent().hide(); }
            if (nav.isFriendClose) { $('#viewerFriends').hide(); }
            if (nav.isSkinClose) { $('#viewerTemplateUrl').hide(); }
            if (nav.isMessageClose) { $('#msgLink').hide(); }
            if (nav.isUserInviteClose) { $('#viewerInviteLink').hide(); }
            if (nav.isUserAppAdminClose) { $('#appAdminUrl').attr('href', '#').parent().hide(); }
            if (nav.isUserMenuAdminClose) { $('#menuAdminUrl').attr('href', '#').parent().hide(); }
            if (nav.isUserLinksClose) { $('#myUrlList').attr('href', '#').parent().hide(); }
            if (nav.isUserPrivacyClose) { $('#viewerSettings').attr('href', '#').parent().hide(); }

            $('.viewerFeeds').attr('href', nav.viewerFeeds);
            if (nav.isFeedClose) {
                $('.viewerFeeds').attr('href', '#');
                $('#feedItem').hide();
            }

            $('.viewerFaceUrl').attr('href', nav.viewerFaceUrl);
            $('#msgLink').attr('href', nav.viewerMsg);
            $('#viewerProfileName').attr('href', nav.viewerProfileUrl).text(nav.viewerName);
            $('#siteNotification').html(nav.viewerSiteNotification);
            $('#siteAdminCmd').html(nav.siteAdminCmd);
            $('#viewerNewMsgCount').html(nav.viewerNewMsgCount);
            $('#userTopNavPic').html('<img id="userAvatar" src="' + nav.viewerPicSmall + '" />');

            $('#siteOnlineCount').text(nav.siteOnlineCount);

            $('#viewerNewNotificationCount').html(nav.viewerNewNotificationCount);
            $('#viewerNewMicroblogAtCount').html(nav.viewerNewMicroblogAtCount);


            var appHtml = '';
            for (var i = 0; i < nav.userAppList.length; i++) { appHtml += '<li>' + nav.userAppList[i] + '</li>'; }
            $('#feedItem').after(appHtml);

            if (ctx.viewer.HasPic == false && ctx.viewer.IsAlertUserPic) {
                var cUrl = wojilu.tool.getRootParent(window).location.href.toLowerCase();
                if (cUrl.indexOf('needuserpic') < 0 && cUrl.indexOf('done') < 0) {
                    wojilu.tool.forwardPage(nav.uploadAvatarLink, 0);
                    return;
                }
            }

            if (ctx.viewer.EmailConfirm == false && ctx.viewer.IsAlertActivation) {
                $('#confirmEmailPanel').show();
                $('#confirmEmailLink').attr('href', nav.confirmEmailLink);
            }

            if ($.browser.msie && $.browser.version == "6.0") {
                $('#ntBox').css('left', '-60px').css('top', '9px');
            }

			wojilu.ui.httpMethod('#navbar-inner');
            if($.browser.msie && $.browser.version=="6.0") wojilu.ui.resetDropMenu('#navbar-inner');

        });


    };

    return { init: _initTopnav };

});
