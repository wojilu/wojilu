// use in box
define([],function(){

    scrollTo(0,0);

    var isRootWin = (window.parent == window);
    if( isRootWin )  return;

    //----------------------加载完毕，隐藏loading-----------------------------------

    var parentIframeId = wojilu.tool.getCurrentFrmId();

    $('#msgInfoGlobalSite').hide(); // 如果在框架中，则隐藏提示信息栏

    var parentIframeClass = $('#'+parentIframeId, parent.document).attr( 'class' );

    var hideParentLoading = function() {
        $('#'+parentIframeId + 'Loading', window.parent.document ).hide();
        $('#'+parentIframeId, window.parent.document ).show();
    }
    
    hideParentLoading(); 
    
    //---------------------调整iframe高度------------------------------------

    var shouldResize = true;
    var srcFrmId = wojilu.tool.getCurrentFrmId();
    if( srcFrmId != null ) {
        var scrolling = $('#'+srcFrmId, parent.document).attr( 'scrolling' );
        if( scrolling != 'no' ) {
            shouldResize = false;
        }
    }
    
    var cmdResize;
    if( shouldResize ) {
        cmdResize = setInterval( resizeParent, 50 );
    }

    var iResize = 1;
    
    function resizeParent() {
        var width = $(document).width();
        var height = parseInt( $(document).height() );
        iResize = iResize + 1;
        if( iResize>20 ) clearInterval( cmdResize ); //弹窗中只调整20次，共计50*20=1秒
        
        if( 'boxFrm' == parentIframeClass ) {
            window.parent.wojilu.tool.resizeBox(width,height, parentIframeId); // 为弹窗专门设置大小
            return;
        }

        window.parent.wojilu.tool.resizeFrame( parentIframeId, height );
    };


    //---------------------------------------------------------

    // 添加frm=true到链接中
    function toAjaxFrame(url) {
        
        if( url.indexOf( 'frm=true' )>0 ) return url;
        return wojilu.tool.appendQuery( url, 'frm=true' );
    };
    

    var frmlnk = function() {
        if( $(this).attr( 'target' )=='_blank' ) return;
        var url = toAjaxFrame($(this).attr( 'href' ));
        $(this).attr( 'href', url );
    };
    
    if( wojilu.tool.getQuery('linkTarget')=='blank') {
        $('a').attr( 'target', '_blank' );
    }
    else {
        $('a:not(.frmLink)').click(frmlnk);        
    };
    
    $('.deleteCmd').click(frmlnk);

    $('form').each( function() {
        $(this).append( '<input name="frm" type="hidden" value="true" />' );
    });

 
});
