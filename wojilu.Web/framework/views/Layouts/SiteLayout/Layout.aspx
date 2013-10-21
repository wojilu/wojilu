<!DOCTYPE html>
<html lang="zh-CN">
<head>
<title>#{pageTitle}</title>
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" /> 
<meta content="#{pageKeywords}" name="Keywords" />
<meta content="#{pageDescription}" name="Description" />
<link href="~css/bootstrap/css/bootstrap.css?v=#{cssVersion}" rel="stylesheet" />
<link href="~css/wojilu._base.css?v=#{cssVersion}" rel="stylesheet" />
#{siteSkinContent}
<script>var __funcList = []; var _run = function (aFunc) { __funcList.push(aFunc); }; var require = { urlArgs: 'v=#{jsVersion}' };</script>
<!--[if IE 6]>
<link href="~css/bootstrap/ie6.css?v=#{cssVersion}" rel="stylesheet">
<link href="~css/wojilu.core.ie6.css?v=#{cssVersion}" rel="stylesheet">
<script>_run( function() { if($.browser.msie && $.browser.version=="6.0") wojilu.ui.resetDropMenu(); });</script>
<![endif]-->
</head>

<body>
#{topNav}
<div class="container page-container" id="page-container">
    #{header}
    <div id="page-main-wrap">
    #{layout_content}
    </div>
    <div id="footerTopAd" class="adItem" data-ad="Footer">#{adFooter}</div>
    <div class="row" id="footer-row">       
        <div class="span12 footer" id="footer">        
            <div class="footer-inner">
	            <div>#{footerMenus}</div>
	            <div>Powered by <a href="http://www.wojilu.com" target="_blank">我记录1.9</a> <a href="http://www.miibeian.gov.cn" target="_blank">#{siteBeiAn}</a></div>
	            <div>Processed in <span id="elapseTime">0</span> seconds, <span id="sqlQueries">0</span> queries</div>
	            <div><a href="#{customSkinLink}" id="customSkin" class="frmBox" title="_{customSkin}"></a></div>
            </div>
        </div>
    </div>
</div>
<script>
    _run(function () {
        require(['wojilu.core.base'], function (x) { x.customSkin().backTop(); });
    });
</script>
<script data-main="~js/main" src="~js/lib/require-jquery-wojilu.js?v=#{jsVersion}"></script>
#{statsJs}
</body>
</html>
