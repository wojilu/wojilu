<div id="mbPubBox" style="width:490px;margin:auto;">

<div style="padding:10px 20px 20px 20px;">

<style>
#mbPubBox {background:url("~img/big/mb/mbPubBg.gif");}
.restCount {font-size:22px; font-weight:bold; color:#1d91c2;}
.restCountWarning {font-size:22px; font-weight:bold; color:#e56f04;}

#txtContent {
border-radius: 3px 3px 3px 3px;-moz-border-radius: 3px 3px 3px 3px;-webkit-border-radius: 3px 3px 3px 3px;
box-shadow:0px 2px 10px #aaa; -webkit-box-shadow: 0px 2px 10px #aaa;-moz-box-shadow:0px 2px 10px #aaa; 
}
</style>

    <div>
        <div style="float:left;font-size:14px; font-weight:bold;">分享到我的微博</div>
        <div style="float:right;"><span id="wmsg">还可以输入 <span class="restCount">#{mbTotalCount}</span> 字</span></div>
        <div class="clear"></div>    
    </div>
    <div><textarea id="txtContent" name="Content" style="width:100%; height:80px;border:1px #ccc inset; font-size:14px; line-height:130%;  ">#{mbContent}</textarea></div>
    <div style="margin-top:5px;">
        <input id="btnPublish" type="button" class="btn" value="_{publish}" />
        <span id="loading"></span>
        <input id="srcType" name="srcType" type="hidden" value="shareBox" />
        
    </div>
    <!-- BEGIN pic -->
    <div style="padding:15px;">
        <img src="#{picThumbSrc}" style="width:120px; height:120px;" />
        <input id="picUrl" name="picUrl" type="hidden" value="#{picSrc}" />
    </div>
    <!-- END pic -->
</div>

<script>
_run( function() {
    require(['wojilu.app.microblog'], function( x ) {
        x.bindMbEvent( '' , #{mbTotalCount}, '#{ActionLink}');
        x.countStr(#{mbTotalCount});
    });
});
</script>
</div>