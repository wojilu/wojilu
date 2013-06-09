define( [], function() {

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
    
    $('.sectionTitleText div', current).mouseover( function() {
        $('.sectionTitleCurrentTab', current).removeClass( 'sectionTitleCurrentTab' ).addClass( 'sectionTitleTab' );
        $(this).removeClass('sectionTitleTab').addClass( 'sectionTitleCurrentTab' );
        $('.sectionContentText', current).hide();
        $( '#'+$(this).attr('contentId') ).show();
    });

}

var _mergeSection = function() {
	
	// 区块合并
	$('.sectionPanel').each( function() {
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

};

var _bindSendButton = function(lnkSendPost) {
    $('.btnSendPost').click( function() {
        window.location.href= lnkSendPost;
        return false;
    });
};

return {mergeSection:_mergeSection, bindSendButton:_bindSendButton}

});
