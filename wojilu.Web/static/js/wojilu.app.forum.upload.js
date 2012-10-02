
define( [], function() {

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


    var _addFile = function( objFile, deleteLink ) {

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

    return {addFile:_addFile};

});
