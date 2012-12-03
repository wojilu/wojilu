define([],function(){

    
    //----------------------加载完毕，隐藏loading-----------------------------------

    var parentIframeId = wojilu.tool.getCurrentFrmId();

    var parentIframeClass = $('#'+parentIframeId, parent.document).attr( 'class' );

    var hideParentLoading = function() {
        window.parent.wojilu.ui.__removeFrameLinkPrev( parentIframeId );
        $('#'+parentIframeId + 'Loading', window.parent.document ).hide();
        $('#'+parentIframeId, window.parent.document ).show();
    }

    /*
    这个只是在翻页链接中才需要，而且在翻页列表超过一屏的情况下
    var pifrmPosition = $('#'+parentIframeId, window.parent.document ).position();
    if( pifrmPosition !=null ) {
        wojilu.tool.getRootParent().scrollTo(0,pifrmPosition.top);
    }*/
    
    hideParentLoading(); 
    
    //---------------------调整iframe高度------------------------------------

    var resizeParent = function() {
        var width = $(document).width();
        var height = parseInt( $(document).height() );
        
        if( 'boxFrm' == parentIframeClass ) {
            window.parent.wojilu.tool.resizeBox(width,height, parentIframeId); // 为弹窗专门设置宽度
            return;
        }

        window.parent.wojilu.tool.resizeFrame( parentIframeId, height );
    };
    
    resizeParent();
    
    setInterval( function() {
        var docHeight = parseInt( $(document).height() );
        window.parent.wojilu.tool.resizeFrame( parentIframeId, docHeight );
    }, 50 ); 


    //---------------------------------------------------------

    var getParentFrmNoLayout = function(win) {

        if( top===self ) return '';
        var arrFrames = win.parent.document.getElementsByTagName("IFRAME");
        for (var i = 0; i < arrFrames.length; i++) {    
            if (arrFrames[i].contentWindow == win && arrFrames[i].className == 'frmLinkPage') {
                return $('#'+arrFrames[i].id, win.parent.document).attr( 'nolayout' );
            }
        }
        return '';
    };

    var nolayout = getParentFrmNoLayout(window);
    
    if( wojilu.tool.getQuery('linkTarget')=='blank') {
        $('a').attr( 'target', '_blank' );
    }
    else if( nolayout ){
        $('a').not('.frmLink').not('.frmBox').click(function(){
            if( $(this).attr( 'target' )=='_blank' ) return;
            var url = $(this).attr( 'href' );
            if( url.indexOf( 'nolayout=' )>0 ) return;
            url = wojilu.tool.appendQuery( url,'nolayout='+nolayout );
            $(this).attr( 'href', url );
        });        

        $('form').each( function() {
            $(this).append( '<input name="nolayout" type="hidden" value="'+nolayout+'" />' );
        });
    };

});
