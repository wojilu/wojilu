<!DOCTYPE html>
<html lang="zh-CN">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>请上传头像</title>
<link href="~css/bootstrap/css/bootstrap.css?v=#{cssVersion}" rel="stylesheet" />
<link href="~css/wojilu._base.css?v=#{cssVersion}" rel="stylesheet" />
<script>    var __funcList = []; var _run = function (aFunc) { __funcList.push(aFunc); }; var require = { urlArgs: 'v=#{jsVersion}' };</script>

<style>
#userFace img { border-radius:4px; padding:5px; border:1px solid #ccc; background:#fff; box-shadow:3px 3px 3px #ccc;}
</style>
</head>

<body>
<div style="width:450px; margin:50px auto; background:#fff; padding:50px;">
    <h1>上传头像</h1>
    <div style="border-top:1px solid #ddd; margin-top:10px;"></div>
    <div style="margin-top:10px; margin-left:10px; color:#666;">上传头像可以获得更多关注、更多回应，谢谢您的支持！<br />请上传 <span class="label label-warning">美观、大方</span> 的图片。<span class="label label-warning">头像会经过管理员手工审核</span></div>

#{layout_content}

</div>

<script data-main="~js/main" src="~js/lib/require-jquery-wojilu.js?v=#{jsVersion}"></script>

</body>
</html>

