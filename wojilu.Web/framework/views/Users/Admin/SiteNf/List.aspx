<!-- copy from '\views\Users\Admin\Notification\List.html' -->


<div style="margin:0px 0px 10px 0px;">

<script>

// 只有这里不同
var newNotificationTextId = 'siteNotificationText';
var newNotificationPanel = 'siteNotification';

// 下面和Admin\Notification\List.html完全一样

_run( function() {


	var isFriendRequest = function( ele ) {
	    var friendTd = ele.parent().next('.friendCmdRow');
	    return friendTd.length == 1;
	};
	
	var hasRead = function( ele ) {
	    var tbl = ele.parent().parent(); // td>tr>tbody
	    if( tbl[0].nodeName.toUpperCase()!='TABLE' ) tbl = ele.parent().parent().parent();
	    if( tbl.attr( 'class' ).indexOf( 'nfread' )>=0 ) {
	        return true;
	    }
	    return false;
	};
	
	var getParentTd = function( alink ) {
	    if( alink.parent().attr( 'class' )=='nfMsg' ) {
	        return alink.parent();
	    }
	    else {
	        return getParentTd( alink.parent() );
	    }
	};
	
	var hideNotification = function() {
	    $('#'+newNotificationPanel, wojilu.tool.getRootParent().document ).hide();
	};
	
	var subtractNotification = function() {
	
		var newNotificationText = $('#'+newNotificationTextId, wojilu.tool.getRootParent().document );
		var intCount = parseInt( newNotificationText.text() );
		var newCount = intCount -1;
		
		if( newCount<=0 ) {
		    hideNotification();
		}
		else {
		    newNotificationText.text( newCount );
		}
	}
	
	//----------------------------------------------------------------------------

    $('.nfMsg a').each( function() {
    
	    var alink = $(this);
	    var ele = getParentTd(alink);	 
	    
	    if( hasRead(ele) ) return;
	    
        var url = $(this).attr( 'href' );
        $(this).attr( 'href', 'javascript:' );
        $(this).attr( 'data-url', url );
    });

    // 标记为全部已读
    $('#cmReadAll').click( function() {
		var actionUrl = '#{lnkReadAll}'.toAjax();
        $.post( actionUrl, function(data) {
			if( 'ok'==data ) {
                $('.nfLine').addClass( 'nfread' );
                $('.friendCmdRow').hide();                
                hideNotification();
			}
			else {
				alert( data );
			}
        });        
    });
    
    // 标记为已读
    var readMsg = function( ele ) {
		var nfid= $(ele).attr( 'nfid' );
		var thisLine = $( '#nfLine' + nfid);
		var friendCmd = $('#friendCmd'+nfid);
		
		var actionUrl = $(ele).attr( 'action' ).toAjax();
		$.post( actionUrl, function(data) {
			if( 'ok'==data ) {
				thisLine.addClass( 'nfread' );
				friendCmd.hide();
				subtractNotification(); // 更新通知数
			}
			else {
				alert( data );
			}
		});
	};

    // 点击已阅图标
	$('.deleteNotification').click( function() {
	    readMsg( this );
	});

	
	// 点击提醒中的链接：a)标记阅读 b)跳转页面
	$('.nfMsg a').click( function() {
	
	    $('#msgLoading').show();
	
	    var alink = $(this);
	    var ele = getParentTd(alink);	   
	     
	    // 检查是否为已阅通知	    
	    if( hasRead(ele) ) {
	        $('#msgLoading').hide();

	        var url = alink.attr( 'href' ).split( 'nolayout' )[0];
	        url = wojilu.str.trimEnd( url, '?' );
	        wojilu.tool.forwardPage( url, 0 );
	        return false;
	    }
	    
	    // 检查是否是好友请求
	    if( isFriendRequest( ele ) ) {
	        $('#msgLoading').hide();
	        return;
	    }

		var nfid= $(ele).attr( 'nfid' );
		var thisLine = $( '#nfLine' + nfid);
		var friendCmd = $('#friendCmd'+nfid);
		
		var actionUrl = $(ele).attr( 'action' ).toAjax();
		$.post( actionUrl, function(data) {
			if( 'ok'==data ) {
				thisLine.addClass( 'nfread' );
				friendCmd.hide();
				$('#msgLoading').hide();
				window.location.href=  alink.attr( 'data-url' );
			}
			else {
			    $('#msgLoading').hide();
				alert( data );
				return true;
			}
		});
		
		return false;
	});

});

</script>


<style>
table.nfLine{border-top:1px #ddd dotted;font-size:12px; margin-left:0px}
table.nfLine td{padding:3px 3px 3px 3px;}
table.nfLine:hover {background:#f2f2f2;}
table.nfLine .quote {color:#aaa;}

table.nfread {color:#aaa; font-size:12px}
table.nfread a{color:#aaa;}
table.nfread img {display:none; }

</style>

<div>

    <table style="width: 100%; margin-bottom:10px;">    
	    <tr>
	    <td style="text-align:center;">
	    <span id="msgLoading" style="display:none;background:red; padding:3px 10px 3px 10px; color:#fff; text-align:center;">loading...</span>
	    </td>
	    <td style=" text-align:right;width:150px;" class="#{readAllClass}">
	    
	    <span class="cmd" id="cmReadAll">_{markAllRead}</span>
	    </td></tr>
    </table>

	<!-- BEGIN notifications -->
	
	<table style="width: 100%" class="nfLine #{notification.ReadClass}" id="nfLine#{notification.Id}">
		<tr>
			<td class="note" style="font-size:10px;margin-right:10px;width:120px">#{notification.Created}</td>
			<td class="nfMsg" nfid="#{notification.Id}" action="#{notification.ReadLink}">#{notification.Msg}</td>
			<td style="width:20px;vertical-align:top;padding-top:5px;text-align:right">
			<img src="~img/delete.gif" class="deleteNotification" nfid="#{notification.Id}" style="cursor:pointer" action="#{notification.ReadLink}"/>
			</td>
		</tr>
		
		<!-- BEGIN friendCmd -->
		<tr id="friendCmd#{notification.Id}" class="friendCmdRow">
		<td>&nbsp;</td>
		<td><div style="margin-left:20px;"><span href="#{notification.ApproveFriendLink}" class="putCmd">_{allow}</span> | <span href="#{notification.RefuseFriendLink}" class="putCmd">_{refuse}</span></div></td>
		<td></td>
		</tr>						
		<!-- END friendCmd -->						
	</table>	
	<!-- END notifications -->
	
	<div>#{page}</div>
	
	
</div>

</div>


