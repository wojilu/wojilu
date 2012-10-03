define(['lib/colorpicker/js/colorpicker'], function () {

    function loadMyPicList() {
        wojilu.ui.frmUpdate();
        $('#frmLoader').click();
    };

    function reloadMyPicList() {
        $('#frmLoader').click().click();
    };

    function setColorPicker() {

        $('.colorSelector').each(function () {
            var colorSelector = $(this);
            var thisColor;

            var eleSelector = colorSelector.attr('eleSelector');
            if (!eleSelector) eleSelector = eleTag;

            colorSelector.ColorPicker({
                color: '#0000ff',
                onShow: function (colpkr) {
                    $(colpkr).fadeIn(200);
                    return false;
                },
                onHide: function (colpkr) {
                    $(colpkr).fadeOut(200);
                    return false;
                },
                onChange: function (hsb, hex, rgb) {
                    thisColor = '#' + hex;
                    var cssKey = colorSelector.attr('cssKey');
                    $(eleSelector, window.parent.document).css(cssKey, thisColor);
                },
                onClick: function (hex) {
                    var bg = '#' + hex;
                    var cssKey = colorSelector.attr('cssKey');
                    $.post(saveUrl.toAjax(), { kv: cssKey + ':' + bg + ';', ele: eleSelector }, function (data) {
                        if (data != 'ok') alert('保存失败，请稍后再试');
                    });
                }
            });
        });
    };

    //------------------------------------------------------------------------

    function setBgPic(picThumb, picUrl) {
        var bgVal = 'url(' + picUrl + ')';
        if (!picUrl) bgVal = '';
        $(eleTag, window.parent.document).css('background', 'url(' + picUrl + ')');
        $.post(saveUrl.toAjax(), { kv: 'background:' + bgVal + ';', ele: eleTag }, function (data) {
            if (data != 'ok') alert('保存失败，请稍后再试');
        });
    };


    function setHeight() {
        var saveHeight = function (newH, eleSelector) {
            $.post(saveUrl.toAjax(), { kv: 'height:' + newH + 'px;', ele: eleSelector }, function (data) {
                if (data != 'ok') alert('保存失败，请稍后再试');
            });
        };

        $('#cmdUp').click(function () {
            var eleSelector = $(this).attr('eleSelector');
            var ele = $(eleSelector, window.parent.parent.document);
            var hValue = ele.height();
            var newH = hValue + 1;
            ele.height(newH);
            $('#lblHeight').text(newH + 'px');
            saveHeight(newH, eleSelector);
        });

        $('#cmdDown').click(function () {
            var eleSelector = $(this).attr('eleSelector');
            var ele = $(eleSelector, window.parent.parent.document);
            var hValue = ele.height();
            var newH = hValue - 1;
            ele.height(newH);
            $('#lblHeight').text(newH + 'px');
            saveHeight(newH, eleSelector);
        });

    };

    function setBgStyle() {

        $('#background-repeat').change(function () {

            var val = $(this).val();
            if (val == 'no-repeat')
                $('#background-position').attr('disabled', false);
            else
                $('#background-position').attr('disabled', 'disabled');

            $(eleTag, window.parent.document).css('background-repeat', val);
            $.post(saveUrl.toAjax(), { kv: 'background-repeat:' + val, ele: eleTag }, function (data) {
                if (data != 'ok') alert('保存失败，请稍后再试');
            });

        });

        $('#background-position').change(function () {

            var val = $(this).val();
            $(eleTag, window.parent.document).css('background-position', val);
            $.post(saveUrl.toAjax(), { kv: 'background-position:' + val, ele: eleTag }, function (data) {
                if (data != 'ok') alert('保存失败，请稍后再试');
            });

        });

        $('#chkDeleteBgPic').click(function () {
            setBgPic('', '');
        });
    };

    //------------------------------------------------------------------------


    loadMyPicList();
    setColorPicker();
    setHeight();
    setBgStyle();

    return {setBgPic: setBgPic, reloadMyPicList:reloadMyPicList }

});
