wojilu.tool.share(); // 启用分享功能


define( [], function() {

	var _check = function( moderators, creatorId, tagAction ) {

		var isModerator = function(name) {
			for( var i=0;i<moderators.length;i++ ) {
				if( moderators[i] == name ) return true;
			}
			return false;
		};

		var showMTool = function() {

			if( ctx.viewer.IsAdministrator || isModerator(ctx.viewer.obj.Name) ) {
				$('.postAdmin').show();
				$('.cmdTag').show();
				
				$('.post-admin-items a').each( function() {
					$(this).attr( 'href', $(this).parent().attr( 'cmdurl' ) );
				});
			}
			else{
			
				if( ctx.viewer.Id == creatorId ) {                
					$('.cmdTag').show();
				}
				
				$('.editCmd').each( function() {
				
					var thisCreatorId = parseInt( $(this).attr( 'data-creatorId' ) );
					if( thisCreatorId == ctx.viewer.Id ) $(this).show();
					
				});
		   }
		   
		};

		wojilu.site.load( showMTool );

		$('.cmdTag').click( function() {
			var ps = $(this).position(); // 获取当前被点击的元素的位置
			$('#tagBox').css( 'top', ps.top+20 ).css( 'left', ps.left ).toggle('fast'); // 通过绝对定位来显示图层
			var tagValue = $(this).prev().text();
			$('#tagValue').val( tagValue );
			$('#tagId').val( $(this).prev().attr( 'id' ) );
		});

		$('#tagCancel').click( function() {
			$("#tagBox").toggle('fast');
		});

		$('#btnSaveTag').click( function() {
			var btn = $(this);
			btn.attr( 'disabled', 'disabled' );
			var tagValue = $('#tagValue').val();
			var tagSpan = $( '#'+$('#tagId').val() );
			var postId = tagSpan.attr( 'postId' );
			$.post( tagAction.toAjax(), {'postId':postId ,'tagValue':tagValue}, function(data) {
				btn.attr( 'disabled', false );
				if( 'ok'==data ) {
					tagSpan.text( tagValue );
					$("#tagBox").toggle('fast');
				}
				else {
					alert( data );
                }
			});
		});
		
	};

    return {check:_check};

});
