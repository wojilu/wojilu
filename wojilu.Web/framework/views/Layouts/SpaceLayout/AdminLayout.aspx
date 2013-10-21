<!DOCTYPE html>
<html lang="zh-CN">
<head>
<title>#{pageTitle}</title>
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" /> 

<meta content="#{pageKeywords}" name="Keywords" />
<meta content="#{pageDescription}" name="Description" />
<style>
#topNavWrap {background:url("~skin/site/topNavBg.png");}
</style>

<link href="~css/wojilu._common.css?v=#{cssVersion}" rel="stylesheet" type="text/css" />
<link href="~css/wojilu._admin.css?v=#{cssVersion}" rel="stylesheet" type="text/css" />
#{siteSkinContent}
<script>var __funcList = []; var _run = function (aFunc) { __funcList.push(aFunc); }; var require = { urlArgs: 'v=#{jsVersion}' };</script>
<script>
_run(function () {
	$('#uaContainer').height( $(window).height()-$('#topNav').height() -2  );
	setInterval( function() { $.post( '#{lostPage}', function() {});}, 1000*60*10 );	
});
</script>

</head>

<style>
body {background:#ddf3f7; }

#uaContainer { border-top:0px #fff solid;width:960px;margin:auto}

#sidebar { background:#f8fbff; width:158px; vertical-align:top; padding:0px; border-right:0px #cdcdcd solid; box-shadow:inset -5px -1px 30px #ddf3f7;padding-right:5px;}
#uaMain { vertical-align:top; background:#f6f6f6; background:#fff;}

#sidebar ul {}
#sidebar li { margin:0px 0px; font-size:14px; font-weight2:bold; padding:8px 15px;}
#sidebar li:hover { background:#fff;}

#sidebar .userToolbar a { margin-right:5px;}

#appSetting { margin-top:15px;}
#appSetting a {font-weight:normal; text-align:center;}


</style>

<body>

#{topNav}

#{adminMain}

<script data-main="~js/main" src="~js/lib/require-jquery-wojilu.js?v=#{jsVersion}"></script>
<script>require(["wojilu._admin"])</script>
#{statsJs}
<div style="margin:auto;width:950px; text-align:center;display:none;">Processed in <span id="elapseTime">0</span> seconds, <span id="sqlQueries">0</span> queries<div>
</body>
</html>
