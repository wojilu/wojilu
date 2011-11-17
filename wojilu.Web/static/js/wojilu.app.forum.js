
var forumUploadFiles = 0;
var newRow;


var addIdToEnd = function( ids, fileId ) {
    if( wojilu.str.isNull( ids ) ) return fileId;
    if( wojilu.str.endsWith( ids, ',' ) ) return ids+fileId;
    return ids+','+fileId;
};

// 从 ids 中移除 fileId
var removeId = function( ids, fileId ) {
    var result = '';

    if( wojilu.str.isNull( ids ) ) return result;            
    
    var tempIds = ','+ids;
    if( wojilu.str.endsWith( tempIds, ',' )==false ) tempIds = tempIds+',';
    var target = ','+fileId+',';
    var tindex = tempIds.indexOf( target );
    
    if( tindex>=0 ) {
        result = tempIds.substr( 1, tindex ) + tempIds.substring( tindex+target.length, tempIds.length+1 );
    }
    else {
        result = ids;
    }
    
    if( wojilu.str.endsWith( result, ',' ) ) {
        return wojilu.str.trimEnd( result, ',' );
    }
    
    return result;    
};

// 如果ids中已经存在fileId，则保持不动
// 最终的结果ids末尾没有逗号
var addId = function( ids, fileId ) {
    var result = '';

    if( wojilu.str.isNull( ids ) ) return fileId;            
    
    var tempIds = ','+ids;
    var target = ','+fileId+',';
    var tindex = tempIds.indexOf( target );
    
    
    if( tindex>=0 ) {
        result = ids;
    }
    else {
        result = addIdToEnd( ids, fileId );
    }
    
    if( wojilu.str.endsWith( result, ',' ) ) {
        return wojilu.str.trimEnd( result, ',' );
    }
    
    return result;  
};


var addFile = function( objFile, deleteLink ) {

    var ss = 'style="float:left;margin:10px; height:150px; width:120px; line-height:135%;"';
    var ssi = 'style=" cursor:pointer;text-decoration:none;"';
    var ssd = 'style=" cursor:pointer;text-decoration:none;color:Red;"';

    var divId = 'file'+objFile.Id;
    var htmlOne;
    
    if( objFile.IsImage ) {
        htmlOne = '<div id="'+divId+'" '+ss+'><img src="'+objFile.FileThumbUrl+'" style="width:90px;"/><br/><span id="cmdInsertImg" '+ssi+'>&rsaquo;&rsaquo; 插入编辑器</span><br/><span id="cmdDeleteImg" '+ssd+'>× 删除</span></div>';
    }
    else {
        var fileDes = wojilu.str.hasText( objFile.Description ) ? objFile.Description : objFile.FileName;
        htmlOne = '<div id="'+divId+'" '+ss+'>文件：'+fileDes+'<br/><span id="cmdDeleteImg" '+ssd+'>× 删除</span></div>';

    };
    
    $('#uploadFiles').append( htmlOne );
    
    var ids = $('#uploadFileIds').val();
    var newIds = addId( ids, objFile.Id );
    $('#uploadFileIds').val( newIds );   
    
    $( '#cmdDeleteImg', '#'+divId).click( function() {
        $(this).parent().remove();

        
        $.post( deleteLink, {"Id":objFile.Id}, function(data) {
            var ids = $('#uploadFileIds').val();
            var newIds = removeId( ids, objFile.Id );
            $('#uploadFileIds').val( newIds );
        });             
    });
    
    $( '#cmdInsertImg', '#'+divId).click( function() {
        ContentEditor.insertImg( objFile.FileMediuUrl );
    });    

};

function showContent(content) {
    $('#forumPrice').html( content );
};

$(document).ready( function() {

    // 帖子显示部分图片限制大小
	$( '#attachmentPanel img' ).each( function() {
		if( $(this).width()>680) $(this).width(680);		
	});
    
    $('input[name=postSelect]').click( function() {
		var thisRow = $(this).parent().parent().parent().parent();
		if( this.checked )
			thisRow.css( 'background', '#FFFFE0' );
		else
			thisRow.css( 'background', '' );
	});

    // 版主管理
	$("#forumPostSelectAll").click( function() {
		if( this.checked )
			$("#forumPostList :checkbox").check('on');
		else
			$("#forumPostList :checkbox").check('off');
	});
    
    // 图片上传
	$('.uploadInput').change( addRow );

	function addRow() {
		var thisCell = $(this).parent();
		var thisRow = $(this).parent().parent();
		newRow = thisRow.clone();
		var thisTable = thisRow.parent();        

		// 将上传控件隐藏，将提示信息显示
		var lnkDeleteId = 'deleteUpload'+new Date().getTime();
		var lnkInfo = 'lnkFile'+new Date().getTime();
		var imgPath = $(this).val();
		var fileName = '<span id="'+lnkInfo+'" >'+wojilu.tool.getFileName(imgPath)+'</span>';
		var fileInfo = '<span class="cmd" id="'+lnkDeleteId+'">'+lang.del+'</span> ' + fileName;
		$(this).hide();
		$(this).before( fileInfo );

		// 添加新的一行上传控件
		thisRow.after( newRow );        
        forumUploadFiles ++;
		
        resetNewInput(  );

		// 给新的上传控件添加事件？
        var cloneInput = newRow.find( '.uploadInput' );
		cloneInput.change( addRow );

		// 判断新行是否显示
		var fileAmount = parseInt( $('#fileAmount').val() );
        if( forumUploadFiles>= fileAmount ) newRow.hide();
		
		// 给删除命令添加callback
		$('#'+lnkDeleteId).click( removeAttach );
	};
    
    // firefox中需要将克隆的上传控件reset()
    function resetNewInput() {
		var cloneInput = newRow.find( '.uploadInput' );
		var tempForm = $('#tempForm');
		if( tempForm.length ==0 ) {
			$('body').append( '<form id="tempForm" style="display:none;"></form>' );
			tempForm = $('#tempForm');
		}
		$('#tempForm').append( cloneInput );
		$('#tempForm')[0].reset();
		var firstCloneCell = newRow.find( 'td' )[0];
		$(firstCloneCell).append( cloneInput ); // reset之后再恢复到newRow中    
    };

	function removeAttach() {
		var arrRows = $(this).parent().parent().parent().find( 'tr' );
		var thisRow = $(this).parent().parent();
		var thisTable = thisRow.parent();
		thisRow.remove();
        forumUploadFiles --;
        
		var fileAmount = parseInt( $('#fileAmount').val() );
        if( forumUploadFiles<fileAmount ) newRow.show();
        
	};

});

