define(['wojilu.core.drag'], function( sort ) {
    
    var saveCallback = function() {
     	var newlayout = sort.result();
    	var actionUrl = $( '#sectionContainer' ).attr( 'target' ) + "?layout=" + newlayout + "&r=" + wojilu.tool.getRandom();
     	$.post( actionUrl, function(data) {
    		if( 'ok'!=data )
                alert( lang.exDrag+data );
    	});
    };

    // sort and drag
	sort.to( 'sectionContainer', 'adminColumn', 'adminSection', 'adminSectionTitleInternal', saveCallback );
	
	$( '#chkUpload' ).click( function(){
		$( '#lnkDiv' ).toggle();$( '#uploadDiv' ).toggle();
	});



	var hideSection = function( targetId, current ) {
        
        // 将其他区块的标题挪来
        var ctitle = $('#sectionTitleText'+targetId).html();
        $('.sectionTitleText', current ).append( ctitle );
        
        // 将其他区块的内容挪来，并隐藏
        var targetSection = $('#section'+targetId);
        targetSection.hide();        
        var targetContent = $('#sectionContentText'+targetId);
        $('.sectionContent', current ).append( targetContent );
        targetContent.hide();
        
        // 给tab添加点击切换事件
        var tabSwitch = function() {
            // 切换tab
            $('.sectionTitleCurrentTab', current).removeClass( 'sectionTitleCurrentTab' ).addClass( 'sectionTitleTab' );
            $(this).removeClass('sectionTitleTab').addClass( 'sectionTitleCurrentTab' );
            $('.sectionTitleTab', current).mouseover( tabSwitch );
            
            // 显隐内容
            $('.sectionContentText', current).hide();
            $( '#'+$(this).attr('contentId') ).show();
        };
        
        $('.sectionTitleTab', current).mouseover( tabSwitch );

	}
	
	// 区块合并
	$('.adminSection').each( function() {
	    var current = this;
	    var targetId = $(current).attr( 'combineIds' );
	    if( targetId ) {
            // 本区块的标题加上class
            $('.sectionTitleText div', current ).removeClass('sectionTitleTab').addClass( 'sectionTitleCurrentTab' );
	        var arrIds = targetId.split( ',' );
	        for( var i=0;i<arrIds.length;i++){
	            hideSection( arrIds[i], current );
	        }
	    }
	});


});

