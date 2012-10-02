
var groupAdmin = function(btnID, actionName) {
	$("#"+btnID).click( function() {
		var actionUrl = $( '#ActionUrl' ).val() + '?' + wojilu.tool.getRandom();
		var choiceList = getChoiceList();
		if( choiceList.length == 0 ) { alert( lang.exSelect ); return; }
		if( actionName == "deletetrue" && confirm( lang.deleteTrueTip )==false  ) return;
		$.post( actionUrl, {"choice":choiceList, "action":actionName}, function(data){
			if( data == 'error' ) { alert( lang.exop ); }	else { alert( data ); wojilu.tool.reloadPage();}
		});
	});
}


$(document).ready( function() {

	groupAdmin( "btnPass", "pass" );
	groupAdmin( "btnRemove", "remove" );
	groupAdmin( "btnBlacklist", "blacklist" );
	groupAdmin( "btnAddadmin", "addadmin" );
	groupAdmin( "btnDeleteadmin", "deleteadmin" );

	$( '#joinGroup' ).click( function() {
		var actionUrl = window.location.href;
		var ext = actionUrl.getExt();
		actionUrl = actionUrl.replace( ext, '/GroupMain/JoinGroup'+ext ) + "?" + wojilu.tool.getRandom();
		$.post( actionUrl, function(data) {
			alert(data);wojilu.tool.reloadPage();
		});
	});

	$( '.groupaddfriend' ).click( function() {
		var actionUrl = $(this).attr( 'addUrl' );
		$.post( actionUrl, function(data) {
			if( data=='notlogin' ){
				alert( lang.exLogin );
			}
			else if( data=='self' ) {
				alert( lang.exAddSelf );
			}
			else if( data=='isfriend' ) {
				alert( lang.hasBeenFriend );
			}
			else if( data=='add' ) {
				alert( lang.addFriendOk );
			}
			else {
				alert( lang.exop );
			}
		});
	});


});