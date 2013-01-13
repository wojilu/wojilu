define([], function() {

    function initSkinCustom() {
        var q = wojilu.tool.getQuery('skin');
        if (q == 'custom') {
            $('#customSkin').click();
            wojilu.ui.box.hideBg();
        }
        return this;
    }

    function initBackTop() {

        $('body').append('<div id="backTop"><span>回顶部</span></div>');

        $(window).scroll(function () {
            if ($(window).scrollTop() >= 300) {
                $("#backTop").fadeIn(800);
            } else {
                $("#backTop").fadeOut(800);
            }
        });

        $('#backTop').click(function () {
            $('html,body').animate({ scrollTop: 0 }, 800);
            return false;
        });

        return this;
    }

    function setCurrentNav(eleMenu, clsCurrent) {
        var linkEqual = function (moduleUrl, menuUrl) { return moduleUrl.indexOf(menuUrl) > -1; }
        $(eleMenu + ' li').each(function () {
            var lnk = $('a', this);
            var menuRawUrl = lnk.attr('data-raw-url');
            for (var i = 0; i < _moduleList.length; i++) {
                if (linkEqual(_moduleList[i], menuRawUrl)) {
                    $(this).addClass(clsCurrent);
                    break;
                }
            }
        });

        return this;
    }

    return { customSkin:initSkinCustom, backTop: initBackTop, setCurrent : setCurrentNav };

});
