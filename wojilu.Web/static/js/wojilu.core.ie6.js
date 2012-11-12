$(document).ready( function() {

    $('.dropdown').unbind('click').hover(

        function() {
            var item = $('.dropdown-menu',this);
            var tp = wojilu.position.getTarget(this);
            var mWidth = item.width();var menuX = tp.x;
            var mLeft = 0;
            if( menuX+ mWidth > document.body.clientWidth ) mLeft = $(this).width()-mWidth;

            $(this).css( 'position', 'relative' ).css("zIndex",1002);
            item.css( 'position', 'absolute' ).css("zIndex",1003).css( 'top', ($(this).height())+'px' ).css( 'left', mLeft ).slideDown('fast');
        },

        function() {
            $(this).css("zIndex",1);
            var item = $('.dropdown-menu',this);
            item.hide();
        }    

    );
});
