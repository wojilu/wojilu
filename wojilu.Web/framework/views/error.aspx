<!DOCTYPE html>
<html lang="zh-CN">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>#{exMsg}_#{pageTitle}</title>
<script>var __funcList = []; var _run = function (aFunc) { __funcList.push(aFunc); }; var require = { urlArgs: 'v=#{jsVersion}' };</script>
<script>
_run( function() { 

    var parentIframeId = wojilu.tool.getCurrentFrmId();
    
    var hideLoading = function() {
        if( parentIframeId==null ) return;
        window.parent.wojilu.ui.__removeFrameLinkPrev( parentIframeId );
        $('#'+parentIframeId + 'Loading', window.parent.document ).hide();
        $('#'+parentIframeId, window.parent.document ).show();
    };
    
    hideLoading();
});
</script>

<style>
body {font-size:14px;line-height:150%;font-family:verdana; }
#navLink a{ color:#666; text-decoration:none;}
#navLink a:hover { text-decoration:underline;}
</style>
</head>

<body style="padding:0px;margin:0px;background:#ccc;">

    <div style="width:90%; margin:30px auto;background:#fff;border-radius:5px;box-shadow:5px 5px 15px #666;">

        <div style="margin:0px 30px;padding-top:10px;padding-bottom:30px;">
            <h2 style="font-size:22px;">#{exTitle}</h2>
            <div style="font-size:14px;" id="navLink">#{navLink}</div>
            <div style="width:95%;border:1px #aaa solid;padding:15px;font-size:12px;color:#333;background:#f2f2f2;border-radius:5px;">#{exDetail}</div>
        </div>

    </div>
</body>

<script data-main="~js/main" src="~js/lib/require-jquery-wojilu.js?v=#{jsVersion}"></script>
</html>
