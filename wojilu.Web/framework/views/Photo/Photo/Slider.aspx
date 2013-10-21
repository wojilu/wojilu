<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<title>#{pageTitle}</title>
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" /> 
<meta content="#{pageKeywords}" name="Keywords" />
<meta content="#{pageDescription}" name="Description" />

<script src="~js/lib/require-jquery-wojilu.js?v=#{jsVersion}" type="text/javascript"></script>
<script src="~js/lib/galleria/galleria-1.2.4.min.js?v=#{jsVersion}" type="text/javascript"></script>

<style>
body { font-family:Verdana;}
a { color:#fff; text-decoration:none;}
a:hover { text-decoration:underline;}

#viewCmds { color:#aaaaaa;font-size:12px; }
.turnpage {color:#aaaaaa; font-size:12px; text-align:center;margin:20px 0px 0px 0px; }

</style>

</head>

<body style="background:#000;">

<div style="width:600px; margin:auto;">
<div style="margin:20px 10px 5px 10px; text-align:center; " id="viewCmds">
    <span style="font-size:12px;"><a href="#{user.Url}">#{user.Name}</a> 的相册</span>
     - 
    <a href="#{viewNormalLink}" style="font-weight:bold; color:orange;font-size:14px;">#{album.Name}</a>

    <span style="margin-left:30px;">查看方式：</span>
    <span style="">
    <a href="#{viewNormalLink}">普通</a>
    <a href="#{viewBigLink}" style="margin-left:10px;">照片墙</a>
    <a href="#{viewSliderLink}" style="margin-left:10px; color:orange; font-weight:bold;">幻灯片</a>    
    </span>
    <table style="float:right;width:80px;" border="0" cellpadding="0" cellspacing="0"><tr><td><img src="~img/home.gif" /> </td><td><a href="#{siteLink}" style="color:#666">网站首页</a></td></tr></table>
    
</div>
<div>#{page}</div>

<div id="gallery" style="margin:0px auto;">
<!-- BEGIN list -->
    <a href="#{data.ImgMediumUrl}" rel="#{data.ImgUrl}"><img src="#{data.ImgThumbUrl}" title="#{data.TitleFull}"></a>
    <p>#{data.TitleFull}</p>
    <!-- END list -->
</div>
</div>

<script>
    Galleria.loadTheme("~js/lib/galleria/themes/classic/galleria.classic.min.js");
    $("#gallery").galleria({
        width: 600,
        height: 480
        //dataConfig: function(img) {
        //    return {description: $(img).parent().next('p').text() }
        //}
    });
</script>

#{statsJs}


</body>
</html>

