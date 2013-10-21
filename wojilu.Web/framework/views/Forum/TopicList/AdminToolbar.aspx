<style>
.moderatorAdminbar {margin:15px auto; width:98%;}
.moderatorAdminbar td {padding:3px 0px 3px 0px;}
</style>
<table class="moderatorAdminbar alert alert-info">
	<tr>
		<td rowspan="2" style="vertical-align:top;width:55px;padding:5px; border-right:1px #ccc dotted;">
			<label class="checkbox">
                <input type="checkbox" id="forumPostSelectAll" class="selectAll" />
			    _{checkAll}
            </label>
		</td>
		<td style="padding-left:10px; width:120px;">
			<span class="ajaxCmd btn" url="#{adminSticky}"><i class="icon-arrow-up"></i><span class="cmdSticky"> :{sticky}</span></span>			
			<a href="#{stickyOrderLink}" class="" style="color:#005eac;" title=":{cmdSortTip}">&rsaquo;&rsaquo; :{pSortStickyTopic}</a>	
		</td>
		<td><div id="forumAdminGlobalSticky" class="hide">#{adminGsticky}</div></td>
		<td style="padding-left:10px;">
			<span class="ajaxCmd btn" url="#{adminPicked}"><i class="icon-star"></i> <span class="cmdPick"> :{pick}</span></span>
			<span class="ajaxCmd btn" url="#{adminHighlight}"><i class="icon-fire"></i> :{cmdHighlight}</span>
			<span class="ajaxCmd btn" url="#{adminLock}"><i class="icon-lock"></i> <span class="cmdLock"> _{lock}</span></span>
			<span class="ajaxCmd btn left10" url="#{adminDelete}"><i class="icon-trash"></i> <span class="cmdDelete"> _{delete}</span>	</span>
		</td>
		<td style="padding-left:10px;">
			<span class="ajaxCmd btn" url="#{adminCategory}"><i class="icon-th"></i> :{setTopicCategory}</span>				
		</td>
	</tr>
	<tr>
		<td style="padding-left:10px;">
			<span class="ajaxForumPost btn" url="#{adminStickyUndo}"><i class="icon-arrow-down"></i> :{cmdUnSticky}</span>
			
		</td>
		<td><div id="forumAdminGlobalStickyUndo" class="hide">#{adminGstickyUndo}</div></td>
		<td style="padding-left:10px;">
			<span class="ajaxForumPost btn" url="#{adminPickedUndo}"><i class="icon-minus-sign"></i> :{cmdUnPick}</span>
			<span class="ajaxForumPost btn" url="#{adminHighlightUndo}"><i class="icon-minus-sign"></i> :{unHighlight}</span>
			<span class="ajaxForumPost btn" url="#{adminLockUndo}"><i class="icon-minus-sign"></i> :{cmdUnlock}</span>
		
		</td>
		<td style="padding-left:10px;"><div id="forumAdminMove" class="hide">#{adminMove}</div></td>
	</tr>
</table>

<script>

_run( function() {
    var msg = '_{plsSelect}';
    
	$('.ajaxForumPost').click( function() {
		var url = $(this).attr( 'url' );
		if( url=='undefined' || url==null ) {
			alert( lang.exUrl );
			return false;
		}
		url = url.toAjax();

        var data = getParamValue();
        if( !data ) {
            return false;
        }
        
        url = url + '&' + data.replace( /postSelect/g, 'ids' );
        $.post( url, function(data) {
            if( 'ok'==data) {
                wojilu.tool.reloadPage();
            }
            else {
                alert( data );
            }
        });
	
	});

	$( '.ajaxCmd' ).click( ajaxCmdCallback );
	

    
    function getParamValue() {
        var ctl = $("input[name='postSelect']:checked");
        if( ctl.length==0 ) {
            alert( msg );
            return false;
        }
        
        var pValue = ctl.serialize();
        if( wojilu.str.hasText( pValue )==false ) {
            alert( msg );
            return false;
        }
        
        return pValue;
    };

	function ajaxCmdCallback() {

		var url = $(this).attr( 'url' );
		if( url=='undefined' || url==null ) {
			alert( lang.exUrl );
			return false;
		}
		url = url.toAjaxFrame();

        var data = getParamValue();
        if( !data ) {
            return false;
        }
        
        url = url + '&' + data.replace( /postSelect/g, 'ids' );
        var actionUrl = url;
        var boxTitle = '';
        
        var boxWidth = 500;
        var boxHeight = 200;
        
        var titleHeight = 26;
        var contentHeight = boxHeight-titleHeight; // 减去标题高度

        
        var ifrm=wojilu.ui.box.getId();
        var frmId = 'boxFrm'+ifrm;
        var frmClass = 'boxFrm';
        var loadingId = frmId+'Loading';
        var loadingDiv = '<div id="'+loadingId+'" style="width:'+boxWidth+'px;height:'+contentHeight+'px;text-align:center;"><img src="'+wojilu.path.img+'/ajax/big.gif" style="margin-top:30px;"/></div>';        
        var frmHtml = '<iframe id="'+frmId+'" class="'+frmClass+'" src="'+actionUrl+'" frameborder="0" width="'+boxWidth+'" scrolling="no" style="display:none;padding:0px;margin:0px;border:0px red solid;height:'+contentHeight+'px;"></iframe>';
        wojilu.ui.box.showBoxString( frmHtml, boxWidth, boxHeight, boxTitle, loadingDiv );

        
        return false;
		
	}
    
});

</script>
