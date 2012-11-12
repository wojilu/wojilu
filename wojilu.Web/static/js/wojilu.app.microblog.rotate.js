
define( [], function() {

    var _rtie = function( mypic, rotationVal ) {
        var p = mypic[0];

        var iet = 0;
        var angle = rotationVal;
        if( rotationVal>270 ) {
            angle = rotationVal % 360;
        }
        else if( rotationVal<-270 ) {
            angle = rotationVal % 360;
        }    
        
        if( angle==-90 ) iet = 3;
        if( angle==-180 ) iet = 2;
        if( angle==-270 ) iet = 1;
        
        if( angle==90 ) iet = 1;
        if( angle==180 ) iet = 2;
        if( angle==270 ) iet = 3;
        
        p.style.filter= 'progid:DXImageTransform.Microsoft.BasicImage(rotation='+iet+')';
    };

    var _rotationPic = function( mypic, rotationVal ) {
        if( document.all ) {
            _rtie( mypic, rotationVal );
            return;
        }
        mypic.css( '-webkit-transform', 'rotate('+rotationVal+'deg)' );
        mypic.css( '-moz-transform', 'rotate('+rotationVal+'deg)' );
        mypic.css( 'transform', 'rotate('+rotationVal+'deg)' );    
    };

    return { rotationPic:_rotationPic  }

});
