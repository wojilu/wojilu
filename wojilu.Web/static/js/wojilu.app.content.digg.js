define( [], function() {

    var getDiggNum = function( ele ) {
        var strDiggNum = $('.diggNum', $(ele)).text();
        var diggNum = parseInt( strDiggNum.substring( 1, strDiggNum.length-1 ) );
        return diggNum;
    };
    
    var getDiggResult = function( diggValue, cmd ) {
    
        var _diggNumUp = getDiggNum( '#btnDiggUp' );
        var _diggNumDown = getDiggNum( '#btnDiggDown' );
        
        var diggNumUp = _diggNumUp;
        var diggNumDown = _diggNumDown;
        
        if( cmd=='up' ) {
            diggNumUp = _diggNumUp + diggValue;
        }
        else if( cmd=='down' ) {
            diggNumDown = _diggNumDown + diggValue;
        }        

        var percentUp = ( diggNumUp/(diggNumUp+diggNumDown) )*100;
        var percentDown = ( diggNumDown/(diggNumUp+diggNumDown) )*100;
        
        percentUp = Math.round(percentUp * 100)/100 + '%';
        percentDown = Math.round(percentDown * 100)/100 + '%';
                
        return { upPer: percentUp, downPer: percentDown, upValue : diggNumUp, downValue : diggNumDown };
    };
    
    var setEle = function( result, cmd ) {
        var ele = '#btnDiggUp';
        if( cmd=='down' ) ele =  '#btnDiggDown';
        $('.diggNum', $(ele)).text( '('+ result[cmd+'Value'] +')' );        
        $('.diggPerBar span', $(ele)).css( 'width', result[cmd+'Per'] );
        $('.diggPerNum', $(ele)).text( result[cmd+'Per'] );    
    };
    
    var diggCmd = function( diggValue, cmd ) {
    
        var result = getDiggResult( diggValue, cmd );
        setEle( result, 'up' );
        setEle( result, 'down' );
    };


    var setValue = function( obj ) {
       $('#btnDiggUp .diggNum').text( obj.diggUp ); 
       $('#btnDiggDown .diggNum').text( obj.diggDown ); 

       $('#btnDiggUp .diggPerNum').text( obj.diggUpPercent+'%' );
       $('#btnDiggUp .diggPerBar span').css( 'width', obj.diggUpPercent+'%' );

       $('#btnDiggDown .diggPerNum').text( obj.diggDownPercent+'%' );
       $('#btnDiggDown .diggPerBar span').css( 'width', obj.diggDownPercent+'%' );
    };
    
    //--------------------------------------------------------------------------

    var initDigg = function( btnId, lnk ) {
        var cmd = 'up';
        if( btnId.indexOf( 'Down' )>0 ) cmd = 'down';
        $('#'+btnId).click( function() {
            $.post( lnk.toAjax(), function(data) {
                if('ok'==data){
                    diggCmd( 1, cmd );
                }
                else {
                    alert( data);
                }
            });
        });
    }

    return { init : initDigg, setValue : setValue }

});
