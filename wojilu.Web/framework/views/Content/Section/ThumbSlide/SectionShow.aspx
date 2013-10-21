<style>
    .tslidePhoto { border:0px #ccc solid;}
    .tslideText {}
    .tslideNav {background:#eee;}
    .tslideNav div {text-align:center; margin:10px; cursor:pointer;}
    .tslideNo {background:#fff;color:#333; border:1px #eee dotted;}
    .tslideNoCurrent {background:#333;color:#fff; border:3px #ea5506 solid;}
</style>

<script>

_run( function() {

    var first = $($('.tslideNav div')[0]);
    first.addClass( 'tslideNoCurrent' ).removeClass( 'tslideNo' );
    
    $('.tslideNav div').click( function() {
        var my = $(this);
        
        var target = $('#'+my.attr( 'slidePhotoId' ));
        $( 'a', target ).attr( 'href', my.attr( 'photoLink' ) );
        $( 'img', target ).attr( 'src', my.attr( 'imgUrl' ) );
        
        $('.tslideNav .tslideNoCurrent').removeClass( 'tslideNoCurrent' ).addClass( 'tslideNo' );
        my.addClass( 'tslideNoCurrent' ).removeClass( 'tslideNo' );
        
    });
    
    setInterval( function() {
        var current = $('.tslideNav .tslideNoCurrent')[0];
        var next = $(current).next();
        if( next.size()==0 ) next = $( $('.tslideNav div')[0] );
        next.click();
    }, 8000 );
});

</script>

<table style="height:120px;" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td style="text-align:center;">
            <div id="slidePhoto#{sectionId}" class="tslidePhoto">
            <!-- BEGIN first -->
            <a href="#{first.Link}"><img src="#{first.ImgUrl}"  style="width:#{first.Width}px;height:#{first.Height}px;" /></a><!-- END first -->
            </div>
        </td>
        <td id="slideNav#{sectionId}" class="tslideNav" rowspan="2" style="vertical-align:top;">
            <!-- BEGIN nav -->
            <div class="tslideNo" slidePhotoId="slidePhoto#{sectionId}" slideLinkId="slideLink#{sectionId}" photoLink="#{photo.Link}" imgUrl="#{photo.ImgUrl}" photoTitle="#{photo.Title}"><img src="#{photo.ThumbUrl}" style="width:100px;height:73px;" /></div>
            <!-- END nav -->
        </td>
    </tr>
</table>

