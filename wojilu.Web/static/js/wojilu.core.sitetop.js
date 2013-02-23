define([], function () {

    var _initTopnav = function (navUrl) {

        navUrl = (navUrl + '?url=' + window.location.href).toAjax();
        $('#topNavReturnUrl').val(window.location.href);

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

        $('.closeNt').click(function () {
            $(this).parent().hide();
        });

        $.get(navUrl, function (data) {

            ctx = data;
            var nav = ctx.navInfo;
            if (nav.topNavDisplay == 1) { $('#topNav').hide(); return; }
            if (ctx.viewer.IsLogin == false) {
                $('#loginSection').show();
                $('#topLoginForm').show();
                if (ctx.owner.LoginValidImg == false) {
                    $('#loginValidBox').remove();
                }

                if (ctx.connects.length > 0) {
                    var showConnects = function (cid) {
                        $('#' + cid).show();
                        var xmenu = $('#' + cid + 'Menu');
                        for (var i = 0; i < ctx.connects.length; i++) {
                            xmenu.append('<li><a href="' + ctx.connects[i].link + '"><img src="' + ctx.connects[i].logos + '" /> ' + ctx.connects[i].name + '</a></li>');
                        }
                    }
                    var showTopConnects = function () {
                        $('#topConnects').show();
                        var xmenu = $('#topConnectsMenu');
                        var xmenuWrap = $('#topConnectsMenuWrap');
                        var moreCount = 0;
                        for (var i = 0; i < ctx.connects.length; i++) {
                            if (ctx.connects[i].pick == 1) {
                                xmenuWrap.before('<li style="margin-top:3px;"><img src="' + ctx.connects[i].logos + '" /> <a href="' + ctx.connects[i].link + '">' + ctx.connects[i].lname + '</a></li>');
                            }
                            else {
                                xmenu.append('<li><a href="' + ctx.connects[i].link + '"><img src="' + ctx.connects[i].logos + '" /> ' + ctx.connects[i].lname + '</a></li>');
                                moreCount++;
                            }
                        }
                        if (moreCount == 0) xmenuWrap.hide();
                    }
                    showTopConnects();
                    showConnects('loginBoxConnects');
                }
                return;
            }

            $('#userAccountTable').show();
            $('#topNavMenu').show();

            var urls = ['shareLink', 'myGroupsLink', 'appAdminUrl', 'menuAdminUrl', 'myUrlList', 'viewerSpace', 'viewerMicroblogHome', 'viewerTemplateUrl', 'viewerFriends', 'viewerInviteLink', 'siteOnlineUrl', 'viewerProfileUrl', 'viewerContactLink', 'viewerInterestUrl', 'viewerTagUrl', 'viewerPwdUrl', 'viewerSettings', 'viewerCurrency', 'logoutLink'];
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



        });


    };

    return { init: _initTopnav };

});
