<style>
.userAvatar li {margin:5px;}


img.blogPic{cursor: url("~img/cursor/zoomout.cur"), pointer; border:1px #ccc solid; padding:3px;max-width:170px;max-height:300px;}
img.blogPicM{cursor: url("~img/cursor/zoomin2.cur"), pointer; margin:10px 0px 10px 15px;}

div.mpicWrap { border:1px #ccc solid; background:#f2f2f2;}

div.picTool { margin:10px 0px 0px 10px;}
div.picTool span { color:#005eac; margin-right:5px;margin-left:5px; padding-left:12px;}

div.picTool span.cmdZoomin { background:url("~img/s/pictool.png");background-repeat:no-repeat; background-position:-23px 2px;}
div.picTool span.cmdViewOPic { background:url("~img/s/pictool.png");background-repeat:no-repeat;  background-position:-36px 2px;}
div.picTool span.cmdTurnLeft { background:url("~img/s/pictool.png");background-repeat:no-repeat;  background-position:0px 2px;}
div.picTool span.cmdTurnRight { background:url("~img/s/pictool.png");background-repeat:no-repeat; background-position:-10px 2px;}

div.picTool span a { background:#f2f2f2;}

.mpicPanel { overflow:hidden;}

</style>
        
        <div id="blogList">
        <!-- BEGIN list -->
        <div class="blogItem" style="border-bottom:1px #ccc dotted; margin:5px 20px 5px 20px; padding:10px; font-size:14px;">
            <div style="float:left;padding-right:10px;"><a href="#{m.UserLink}"><img src="#{m.UserFace}" style="border:1px #ddd solid; padding:3px;width:48px;" /></a></div>
            <div style="float:left;width:460px; ">
                <div style=" line-height:150%;word-break: break-all;word-wrap: break-word;"><a href="#{m.UserLink}">#{m.UserName}</a>：#{m.Content}</div>
		        <!-- BEGIN pic -->
		        <div><img class="blogPic" src="#{blog.PicSmall}" pic="#{blog.PicMedium}" /></div>
		        <div class="hide mpicWrap">
		            <div class="picTool"><span class="cmdZoomin"><a href="javascript:void(0);" id="cmdZoomin">收起</a></span> <span class="cmdViewOPic"><a href="#{blog.PicOriginal}" target="_blank">查看大图</a></span> <span class="cmdTurnLeft"><a href="javascript:void(0);" id="cmdTurnLeft">向左转</a></span> <span class="cmdTurnRight"><a href="javascript:void(0);" id="cmdTurnRight">向右转</a></span></div>
		            <div class="mpicPanel"></div>
		        </div>
		        <!-- END pic -->



		        <!-- BEGIN video -->
		        <div style="position:relative;">
		        <a href="#{blog.ShowLink}"><img src="#{blog.FlashPic}" style="float:left;padding:3px; border:1px #ccc solid;" />
		        <img class="blogVideo" src="~img/m/videoplay.png" style="position:relative;float:left; top:35px; left:-85px; cursor:pointer;" /></a>
		        </div>
		        <!-- END video -->
                <div style=" clear:both;"></div>

                <div class="note font12">#{m.Created}</div><div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>        
        <!-- END list -->
        </div>


<script>

_run( function() {

    require( ['wojilu.app.microblog.rotate'], function( x ) {
        
        $('.blogPic').click( function() {
            $(this).parent().hide();        
            var panel = $(this).parent().next();
            $( '.mpicPanel', panel ).html( '<img class="blogPicM" src="'+$(this).attr( 'pic' ) +'" />' );
            panel.show();        
        });
        
        $( '.mpicPanel' ).click( function() {
            var sPanel = $(this).parent().prev();
            $(this).parent().hide();
            sPanel.show();
        });
        
        $( '#cmdZoomin' ).click( function() {
            var sPanel = $(this).parent().parent().parent().prev();
            $(this).parent().parent().parent().hide();
            sPanel.show();
        });
        
        var rotate = function( ele, rotationVal ) {
            x.rotationPic( $( 'img', $(ele).parent().parent().next() ), rotationVal );
        };
        
        var tc = 0;
        $( '#cmdTurnLeft' ).click( function() {
            tc += 90;
            rotate( this, -tc );
        });
        
        $( '#cmdTurnRight' ).click( function() {
            tc -= 90;
            rotate( this, -tc );
        });
    });
});
</script>
