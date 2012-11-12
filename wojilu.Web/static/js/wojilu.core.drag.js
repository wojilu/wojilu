define([], function() {
var sort = {

    saveCallback : null,

	dragger : null,
    mockDiv : null,
	mouseOffset : null,    

	container : null,
	columnList : null,
	columnClass : null,
	itemClass : null,

	to : function ( containerId, columnClass, itemClass, handlerClass, saveCallback ) {
    
        if( saveCallback ) sort.saveCallback = saveCallback;
    
		sort.container = containerId;
		sort.columnClass = columnClass;
		sort.itemClass = itemClass;

		sort.columnList = [];
		$( '#' + containerId + ' .' + columnClass ).each( function() {
			var itemsInColumn = [];

			var columnId = $(this).attr('id');
			var items = $(this).find( '.' + itemClass );

			items.each( function() {
				itemsInColumn.push( $(this) );
                $( '.'+handlerClass, $(this) ).mousedown( sort.start );
			});

			sort.columnList.push( {'id':columnId, 'items':itemsInColumn} );
		});
	},

	result : function() {
		var result = '';
		$(sort.columnList).each( function(i) {
			var columnId = this.id;
			$(this.items).each( function(k) { result += columnId + '_' + this.attr('id') + '/'; });
		});
		return result.substring( 0, result.length-1 );
	},

	resetColumn : function() {
		sort.columnList = [];
		$( '#' + sort.container + ' .' + sort.columnClass ).each( function() {
			var itemsInColumn = [];

			var columnId = $(this).attr('id');
			var items = $( '#' + sort.container + ' #' + columnId + ' .' + sort.itemClass );
			items.each( function() {
				itemsInColumn.push( $(this) );
			});

			sort.columnList.push( {'id':columnId, 'items':itemsInColumn} );
		});	
	},
	
	getDragTarget : function(chkTarget){
		if( chkTarget.attr( 'class' )==sort.itemClass){
			return chkTarget;
		}
		else {
			return sort.getDragTarget(chkTarget.parent());
		}
	},
    
    getMockDiv : function(chkTarget) {
        var mdiv = '<div id="mdiv" style="border:2px #008000 dashed;margin-bottom:5px;">&nbsp;</div>';
        $('body').append( mdiv );
        return $('#mdiv');
    },

    initDragger : function() {
        sort.dragger.css( 'position', 'absolute' );
        sort.dragger.css( 'zIndex', 999 );
        sort.dragger.css( 'opacity', 0.8 );
        sort.dragger.css( 'filter', 'alpha(opacity=80)' );
    },

    endDragger : function() {
        sort.dragger.css( 'position', 'static' );
        sort.dragger.css( 'zIndex', 0 );
        sort.dragger.css( 'opacity', 1 );
        sort.dragger.css( 'filter', 'alpha(opacity=100)' );
    },

	start : function(e) {
        sort.dragger = sort.getDragTarget($(this));
        if( !sort.mockDiv ) {
            sort.mockDiv = sort.getMockDiv(sort.dragger);
        }
        sort.mockDiv.css( {height:sort.dragger.height()} );
        
        sort.mockDiv.insertBefore( sort.dragger );
        sort.mockDiv.hide();
        
        var originalWidth = sort.dragger[0].offsetWidth;
        var originalHeight = sort.dragger[0].offsetHeight;

        // 临时节点的坐标
        var targetPosition = wojilu.position.getTarget(sort.dragger[0]);
        sort.initDragger();
        sort.dragger.css( 'top', targetPosition.y);
        sort.dragger.css( 'left', targetPosition.x);
        sort.dragger.css( 'width', originalWidth );
        sort.dragger.css( 'height', originalHeight );
		sort.mouseOffset = wojilu.position.getMouseOffset(sort.dragger[0], e);
        
        sort.mockDiv.show();

		if( document.all ) {
			document.onmousemove=sort.move;
			document.onmouseup=sort.end;
			sort.dragger[0].setCapture();
		}
		else {
			$(document).mousemove( sort.move );
			$(document).mouseup( sort.end );
		}
		return false;
	},

	move : function(e) {
		if( sort.dragger == null ) return false;

		e = e || window.event;
		var newMousePosition = wojilu.position.getMouse(e);

		sort.dragger.css( 'top', newMousePosition.y - sort.mouseOffset.y);
		sort.dragger.css( 'left', newMousePosition.x - sort.mouseOffset.x);        

		$(sort.columnList).each( function(i) {

				$(this.items).each( function(k) {
					var canInsert= sort.canInsert(this[0]);
					if( canInsert.before ) { sort.resizeMockDiv(this);  sort.mockDiv.insertBefore( this );return; }
					if( canInsert.after ) {  sort.resizeMockDiv(this); sort.mockDiv.insertAfter( this ); return; }
				});
				
				if( this.items.length == 0 ) {
					var col = $('#'+this.id);
					if( sort.isInEmptyColumn(col[0]) ) { sort.mockDiv.appendTo(col); return; }
				}

		});
	},
    
    resizeMockDiv : function(target) {
        var twidth = $(target).width();
    },

	end : function(e) {
		if( sort.dragger == null ) return false;

		var dragPosition = wojilu.position.getTarget( sort.dragger[0] );
		var dragX = dragPosition.x;
		var dragY = dragPosition.y;
        
        sort.mockDiv.hide();        
        
        sort.dragger.insertBefore( sort.mockDiv );
        sort.dragger.css( 'width', '100%' );
        sort.dragger.css( 'height', '' );

        sort.mockDiv.empty();

		var targetPosition = wojilu.position.getTarget( sort.dragger[0] );

		sort.resetColumn();
		sort.stopDrag();
        sort.endDragger();
        
        if( sort.saveCallback ) sort.saveCallback();

		return false;
	},    
	
	stopDrag : function() {
		if( document.all ) {
			document.onmousemove=null;
			document.onmouseup=null;
			sort.dragger[0].releaseCapture();
		}
		else {
			$(document).unbind( 'mousemove' );
			$(document).unbind( 'mouseup' );
		}
	},

	canInsert : function( ele ) {
		var panelPosition = wojilu.position.getTarget(ele);
		var areaPercent = 8;

		var areaX_left = panelPosition.x + ele.offsetWidth/areaPercent;
		var areaX_right = panelPosition.x + ele.offsetWidth - ele.offsetWidth/areaPercent;

		var areaY_top = panelPosition.y + ele.offsetHeight/areaPercent;
		var areaY_middle = panelPosition.y + ele.offsetHeight/2;
		var areaY_bottom = panelPosition.y + ele.offsetHeight - (ele.offsetHeight/areaPercent);

		var dragCenter = sort.getDragCenter();

		var isIn_Horizontal_Area = dragCenter.x>areaX_left && dragCenter.x<areaX_right;
		var isIn_Vertical_BeforeArea = dragCenter.y>areaY_top && dragCenter.y<areaY_middle;
		var isBefore = isIn_Horizontal_Area && isIn_Vertical_BeforeArea;

		if( isBefore ) return { before:true, after:false };

		var isIn_Vertical_AfterArea = dragCenter.y>areaY_middle && dragCenter.y<areaY_bottom;
		var isAfter = isIn_Horizontal_Area && isIn_Vertical_AfterArea;
		if( isAfter ) return { before:false, after:true };

		var nextIsNull = $(ele).next().length == 0;
		var isIn_VerticalEnd = dragCenter.y>areaY_bottom;
		if( nextIsNull && isIn_Horizontal_Area &&  isIn_VerticalEnd) return { before:false, after:true };

		return { before:false, after:false };
	},

	isInEmptyColumn : function( col ) {
		var p = wojilu.position.getTarget(col);
		var c = sort.getDragCenter();
		if( c.x > p.x && c.x < (p.x+col.offsetWidth) && c.y>p.y && c.y<(p.y+col.offsetHeight) ) return true;
		return false;
	},

	getDragCenter : function() {
		var xx = parseInt( sort.dragger.css('left') ) + sort.dragger[0].offsetWidth/2;
		var yy = parseInt( sort.dragger.css('top') ) + sort.dragger[0].offsetHeight/2;
		return {'x':xx, 'y':yy};
	}

};

return sort;

});
