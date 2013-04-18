define( ['wojilu.app.microblog.rotate'], function( rt ) {

    var _init = function() {
    
        wojilu.tool.makeTab( '.mbNav', 'currentMbNav', '' );
        wojilu.ui.frmUpdate();    


        $('#lblFollow').click( function() {
            alert( '请先登录' );
        });

        $('#cmdFollow').click( function() {
            var ps = $(this).position();
            $('#myBox').css( 'top', ps.top+30 ).css( 'left', ps.left ).toggle('fast');
        });
        
        $('#btnFollow').click( function() {
            $(this).parent().parent().toggle('fast');
            var url = $(this).attr( 'url' );
            $.post( url, function(data) {
                if( 'ok'==data ) {
                    $('#cmdFollow').html( '关注成功！' );
                }
                else {
                    alert( '对不起，系统错误：'+data );
                }
            });
        });
        
        $('#cancelFollow').click( function() {
            var ps = $(this).position();
            $('#cancelBox').css( 'top', ps.top+20 ).css( 'left', ps.left ).toggle('fast');
        });
        
        $('#btnCancelFollow').click( function() {
            
            var box = $(this).parent().parent();
            $.post( $(this).attr( 'url' ), function(data) {
                if( 'ok'==data ) {
                    box.toggle('fast');
                    $('#cmdCancelFollow').html( '取消关注成功！' );
                }
                else {
                    alert( '对不起，系统错误：'+data );
                }

            });
        });
        
        $('#bntClose').click( function() {
            $(this).parent().parent().toggle('fast');
        });

        // -------------------------------------------------------------------------
        $('.addFavorite').click( function() {

            var btn = $(this);
            var ps = $(this).position();    
            if( btn.text() == '已收藏' ) return;

            var url = $(this).attr( 'to' ).toAjax();
            $.post( url, function(data) {
                var msg = data; 
                if( msg.IsValid ) {
                    btn.text( '已收藏' );
                    btn.css( 'cursor', 'text' );
                    $('#myBoxFav').css( 'top', ps.top-55 ).css( 'left', ps.left ).show();
                    setTimeout( function() {
                        $('#myBoxFav').fadeOut();
                    }, 500 );
                }
                else {
                    alert( msg.Msg );
                }
            });
        });

        $('.cancelFavorite').click( function() {

            var btn = $(this);
            var ps = $(this).position();    
            if( btn.text() == '已取消' ) return;

            var url = $(this).attr( 'to' ).toAjax();
            $.post( url, function(data) {
                var msg = data; 
                if( msg.IsValid ) {
                    btn.text( '已取消' );
                    btn.css( 'cursor', 'text' );
                    $('#myBoxFavX').css( 'top', ps.top-55 ).css( 'left', ps.left ).show();
                    setTimeout( function() {
                        $('#myBoxFavX').fadeOut();
                    }, 500 );
                }
                else {
                    alert( msg.Msg );
                }
            });
        });
        
    };
    
    // -------------------------------------------------------------------------

    function _addBlogEvent( pageContext ) {

        var elePic;
        var elePicPanel;
        var eleVideo;
        var eleZoomin;
        var eleLeft;
        var eleRight;

        if( pageContext ) {
            elePic=$('.blogPic',pageContext );
            elePicPanel=$( '.mpicPanel',pageContext  );
            eleVideo=$('.blogVideo',pageContext );
            eleZoomin=$( '.cmdZoominLink',pageContext  );
            eleLeft=$( '.cmdTurnLeftLink',pageContext  );
            eleRight=$( '.cmdTurnRightLink',pageContext  );
        }
        else {
            elePic=$('.blogPic');
            elePicPanel=$( '.mpicPanel' );
            eleVideo=$('.blogVideo');
            eleZoomin=$( '.cmdZoominLink' );
            eleLeft=$( '.cmdTurnLeftLink' );
            eleRight=$( '.cmdTurnRightLink' );
        }

        elePic.click( function() {
            
            var thisImg = $(this);    
        
            var panel = $(this).parent().next();
            var picPanel = $( '.mpicPanel', panel );
            var simg = $( 'img', picPanel );
            if( simg.length>0 ) { // 已加载
                thisImg.parent().hide();
                panel.show(); 
                return;
            }
            
            thisImg.after( '<img src="'+wojilu.path.img+'/ajax/loading2.gif" style=" float:left; position:relative; top:40px; left:-120px;" />' );
            
            var img = new Image();
            img.src = $(this).attr( 'pic' );
            
            $(img).load( function() {
                $(this).addClass( 'blogPicM' );
                picPanel.append( this );
                
                thisImg.next().remove();
                thisImg.parent().hide();
                panel.show(); 
            });        
            
                   
        });
        
        elePicPanel.click( function() {
            $(this).parent().hide();
            $(this).parent().prev().show();
        }); 
        
           
        eleVideo.click( function() {
            $(this).parent().hide();
            var flashHtml = $( '.flashHtml', $(this).parent() ).html();
            var panel = $(this).parent().next();
            $( '.mvideoPanel', panel ).html( flashHtml );
            panel.show();        
        });

        
        eleZoomin.click( function() {
            var sPanel = $(this).parent().parent().parent().prev();
            $(this).parent().parent().parent().hide();
            sPanel.show();
        });
        
        var rotate = function( ele, rotationVal ) {
            rt.rotationPic( $( 'img', $(ele).parent().parent().next() ), rotationVal );
        };
        
        var tc = 0;
        eleLeft.click( function() {
            tc += 90;
            rotate( this, -tc );
        });
        
        eleRight.click( function() {
            tc -= 90;
            rotate( this, -tc );
        });

    }
    
    
    return {
        init : _init,
        addBlogEvent : _addBlogEvent
    };

});

