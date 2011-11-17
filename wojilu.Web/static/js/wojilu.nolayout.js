
    
$(document).ready( function() {

    //----------------------加载完毕，隐藏loading-----------------------------------

    var parentIframeId = wojilu.tool.getCurrentFrmId();

    var parentIframeClass = $('#'+parentIframeId, parent.document).attr( 'class' );

    var hideParentLoading = function() {
        $('#'+parentIframeId + 'Loading', window.parent.document ).hide();
        $('#'+parentIframeId, window.parent.document ).show();
    }
    
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
