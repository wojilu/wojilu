jQuery.ajaxSettings.traditional = true;

var wojilu = new Object();

wojilu.path = {};
wojilu.path.app = '';
wojilu.path.st = wojilu.path.app + '/static';
wojilu.path.js = wojilu.path.st + '/js';
wojilu.path.img = wojilu.path.st + '/img';
wojilu.path.siteAndApp = 'http://' + window.location.host + wojilu.path.app;
wojilu.path.flash = wojilu.path.st + '/flash';'';

wojilu.ctx = {
    isSubmit : true,
    isValid : true,

    box : {
    	sizeCache : null,
    	//isShowing : false,
        isShowing : {},
    	param : null,
    	//title : '对话窗口',
        title : {},
    	pages : [],
    	isBack : false
    }
};

var logger = {
	info : function(msg) {
		if( !document.all ) console.log( msg );		
	}
};

$.fn.check = function(mode) {
	var mode = mode || 'on';
	return this.each(function() {
		switch(mode) {
		case 'on':
			this.checked = true;
			break;
		case 'off':
			this.checked = false;
			break;
		case 'toggle':
			this.checked = !this.checked;
			break;
		}
	});
};

$.fn.chkVal = function() {
    var choiceList = [];
    this.each( function(i){
        choiceList.push($(this).val());
    });
    return choiceList;
};

wojilu.str = {

    isNull : function( txt ) {
        return txt=='undefined' || txt==null || txt.length<1;
    },

    hasText : function( txt ) {
        return !wojilu.str.isNull( txt );
    },

    startsWith : function( txt, value ) {
        return ( txt.substr( 0, value.length ) == value );
    },

    endsWith : function( txt, value ) {
        return ( txt.substr( txt.length-value.length, txt.length ) == value ) ;
    },

    isJson : function( obj ) {
        return typeof(obj) == "object" && Object.prototype.toString.call(obj).toLowerCase() == "[object object]" && !obj.length;    
    },
    
    isInt : function( text ) {
        var i = parseInt(text);
        if (isNaN(i)) {return false;};
        if (i.toString() == text) {return true;};
        return false;
    },

    replaceAll : function( strText, strTarget,  strSubString ){
        var intIndexOfMatch = strText.indexOf( strTarget );
        while (intIndexOfMatch != -1) {
            strText = strText.replace( strTarget, strSubString );
            intIndexOfMatch = strText.indexOf( strTarget );
        };
        return strText;
    },

    endsWith : function ( txt, value ) {
        return ( txt.substr( txt.length-value.length, txt.length ) == value ) ;
    },

    trimStart : function( txt, val ) {
        if( wojilu.str.startsWith( txt, val ) ==false ) {return txt;};
        return txt.substring( val.length, txt.length );
    },

    trimEnd : function( txt, val ) {
        if( wojilu.str.isNull( txt ) ) return txt;
        if( !wojilu.str.endsWith( txt, val ) ) return txt;
        if( txt == val ) return '';
        return txt.substr( 0, txt.length-val.length );
    },

    trimExt : function( txt ){
        var extPosition = txt.search( /\.[^\./]*$/i );
        if( extPosition>=0 ) {return txt.substring(0, extPosition );};
        return txt;
    },

    trimHost : function( txt ) {
        if( wojilu.str.startsWith( txt, 'http://' ) ==false ) {return txt;};
        var result = wojilu.str.trimStart( txt, 'http://' );
        var slashIndex = result.indexOf( '/' );
        return result.substring( slashIndex, result.length );
    },

    getExt : function( txt ){
        return txt.replace( wojilu.str.trimExt(txt), '' );
    }
};

wojilu.tool = {
    
    getPageHead : function() {
        return document.getElementsByTagName('HEAD').item(0);
    },

    refreshImg : function( vimg ) {
        if( !vimg ) return;
        var imgSrc = vimg.attr('src');
        if( imgSrc ) {
            vimg.attr( 'src', vimg.attr('src').toAjax() );
        }
    },
    
    getCurrentFrmId : function() {
        if( top===self ) return null;
        var arrFrames = parent.document.getElementsByTagName("IFRAME");
        for (var i = 0; i < arrFrames.length; i++) {    
            if (arrFrames[i].contentWindow == window) return arrFrames[i].id;
        }
        return null;
    },
    
    resizeBox : function(x,y,iframeId) {
    	var width = x;
    	var height = y;
        var bid = wojilu.ui.box.id;
    	wojilu.ui.box.setWidthHeight( bid, width, height );        
        $('#'+iframeId).css( 'width', width+'px').css( 'height', height+'px');
        wojilu.ui.box.resetPos(bid);        
    	$('#box'+bid).css( 'height', (height+26)+'px');
    },
    
    resizeFrame : function( frmId, height ) {
        $('#'+frmId ).height( height );
        if( top===self ) return;
        var docHeight = parseInt( $(document).height() );
        window.parent.wojilu.tool.resizeFrame( wojilu.tool.getCurrentFrmId(), docHeight );
    },
    
    addBoxHeight : function(y) {
        var boxId = wojilu.ui.box.getId();
        var t = $('#boxFrm'+boxId);
        t.height( t.height()+y );
        var b = $('#box'+boxId);
        b.height( b.height()+y );
   },

    resizeImg : function( target, width, height ) {
        if( width && target.width>width){target.width=width;};
        if( height && target.height>height){target.height=height;};
    },
    
    htmlReplace : function( targetId, val ) {
        $('#'+targetId).html( val );
    },
    
    htmlAppend : function( targetId, val ) {
        $('#'+targetId).append( val );
    },
    
    reloadPage : function(hash) {
        if( hash )
            window.location = window.location+hash;
        else
            window.location = window.location;

    },

    getRootParent:function(win){
        if( !win ) win = window;
        if(win.parent==win) { return win; }
        return wojilu.tool.getRootParent(win.parent);
    },
    
    forwardPage : function( url, time ) {
        if( !time ) {time = 500;};
        setTimeout( function(){
            wojilu.tool.getRootParent(window).location.href=url;
        }, time);
    },

    getParentCount:function(win,count,layoutCount){
        if(win.parent==win) { return win; }
        if( count==layoutCount ) return win;
        count = count+1;
        return wojilu.tool.getParentCount(win.parent,count,layoutCount);
    },

    forwardPart:function( url, layoutCount, time ) {
        if( !time ) {time = 500;};

        var getParentFrmNoLayout = function(win) {

            if( top===self ) return '';
            var arrFrames = win.parent.document.getElementsByTagName("IFRAME");
            for (var i = 0; i < arrFrames.length; i++) {    
                if (arrFrames[i].contentWindow == win && arrFrames[i].className == 'frmLinkPage') {
                    return $('#'+arrFrames[i].id, win.parent.document).attr( 'nolayout' );
                }
            }
            return '';
        };

        setTimeout( function(){
            var win = wojilu.tool.getParentCount(window,0,layoutCount);
            if( url=='' || url=='#' ) {
                win.location.href=win.location.href;
            }
            else {
                var nolayout = getParentFrmNoLayout(win);
                if( nolayout ) {
                    url = wojilu.tool.appendQuery( url, 'nolayout='+nolayout );
                }
                win.location.href=url;
            }
        }, time);
    },

    addRefreshToHash : function( url ) { // 在url中添加刷新信息，避免#不刷新的问题:

        if( wojilu.str.startsWith( url, '/' ) ) url = 'http://'+window.location.host+url;
        var currentUrlPath = window.location.href;
        var currentHash = currentUrlPath.indexOf( '#' );
        if( currentHash>0 ) currentUrlPath = currentUrlPath.substring( 0, currentHash );
        
        if( wojilu.str.startsWith( url, currentUrlPath+'#'  ) ) {
            var hashIndex = url.indexOf( '#' );
            var urlPath = url.substring( 0, hashIndex );
            var urlFragment = url.substring( hashIndex, url.length );
            return urlPath+'?reload=true'+urlFragment;
        }
        return url;
    },

    forward : function( url, time ) { // 如果实在框架中跳转，则自动加上frm后缀，避免加载layout
        if( !time ) {time = 500;};
        setTimeout( function(){
            if( window.location.href.indexOf( 'frm=true' )>0 && url.indexOf( 'frm=true' )<0 ) {
                if( url.indexOf( '?' )>0 ) {
                    window.location.href=url+'&frm=true';
                }
                else {
                    window.location.href=url+'?frm=true';
                } 
            }
            else {
                window.location.href=url;
            }
        }, time );
    },
    
    loadJs : function( filePath ) {
        loadJsFull( wojilu.path.js + '/' + filePath );
    },

    loadJsFull : function( filePath ) {
        var script = document.createElement('script');
        script.src = filePath;
        script.type = 'text/javascript';
        wojilu.tool.getPageHead().appendChild(script);
    },
    
    loadCss : function( filePath ) {
        var style = document.createElement('link');
        style.href = filePath;
        style.rel = 'stylesheet';
        style.type = 'text/css';
        wojilu.tool.getPageHead().appendChild(style);
    },
    
    getByteLength : function ( str ) {
        if( wojilu.str.isNull( str ) ) return 0;
        var pattern = /[\u4e00-\u9fa5]/g;
        var arrChineseChar = str.match(pattern);
        if( arrChineseChar ) return str.length + arrChineseChar.length;
        return str.length;
    },

    getTimePrivate : function(seperator, isMilliseconds){
       var result = '';
       var d = new Date();
       result += d.getHours() + seperator;
       result += d.getMinutes() + seperator;
       result += d.getSeconds();
       if( isMilliseconds ) {result += seperator + d.getMilliseconds();};
       return result;
    },

    getDayPrivate : function(seperator){
       var d = new Date();
       var result = '';
       result += d.getFullYear() + seperator;
       result += (d.getMonth()+1) + seperator;
       result += d.getDate();
       return result; 
    },
    
    getRandom : function() {
        return Math.random()+ wojilu.tool.getDayPrivate('')+ wojilu.tool.getTimePrivate('', true);
    },

    getRandomInt : function() {
        return wojilu.tool.getDayPrivate('')+ wojilu.tool.getTimePrivate('', true);
    },

    getTime : function() {
        return wojilu.tool.getTimePrivate(':', false);
    },

    getDay : function() {
        return wojilu.tool.getDayPrivate('-');
    },

    getFileName : function(fileFullName) {
        var index = fileFullName.lastIndexOf( '\\' );
        if( index==-1 ) {index = fileFullName.lastIndexOf( '/' );};
        return fileFullName.substring(index+1);
    },

    getParams : function( scriptName ) {
        var sc=document.getElementsByTagName('script');
        
        var scriptSrcQuery = '';
        for( var i=0;i< sc.length;i++ ) {
            if( sc[i].src.indexOf( scriptName )>=0 ) {
                scriptSrcQuery = sc[i].src.split( '?' )[1];
                continue;
            };
        };
        
        var arrRawItem = scriptSrcQuery.split( '&' );
        var results = new Array();
        for( i=0;i<arrRawItem.length;i++) {
            var itemString = arrRawItem[i].split( '=' );
            results[ itemString[0] ] = itemString[1];
        };
        return results;
    },

    getQuery : function( qName, qUrl ) {
        var url = qUrl? qUrl : window.location.href;
        var arrPart = url.split( '?');
        if( arrPart.length <=1 ) {return '';};
        var arrItem = arrPart[1].split( '&' );
        for( var i=0;i<arrItem.length;i++ ) {
            if( wojilu.str.startsWith( arrItem[i], qName+'=' ) ) {				
                return wojilu.str.trimStart( arrItem[i], qName+'=' );
            };
        };
        return '';
    },

    getUrlWithoutQuery : function( url ) {
        if( !url ) return url;
        return url.split( '?')[0];
    },

    appendQuery : function ( url, queryItem ) {
    	var indexQuery = url.indexOf( '?' );
    	if( indexQuery<0 ) {
    		return url + '?'+queryItem;
    	}
        else if( url.indexOf( queryItem )>0 ) {
            return url;
        }
    	else {
            if( wojilu.str.endsWith( url, '?' ) ) return url +queryItem ;
            if( wojilu.str.endsWith( url, '&' ) )  return url +queryItem ;
            return url + '&'+queryItem ;
        };
    },

    cancelBubble : function(e) {
        if ( e && e.stopPropagation ) {
            e.stopPropagation();
        }
        else {
            window.event.cancelBubble = true;
        };
        return false;
    },
    
    makeTab : function(containerClassOrId, currentClass, otherClass) {
    	var currentUrl = wojilu.str.trimHost( window.location.href );
        currentUrl = wojilu.tool.getUrlWithoutQuery( wojilu.str.trimExt( currentUrl ));
    	$(containerClassOrId+' a' ).each( function(i) {
            var link = $(this).attr( 'href' );
            link = wojilu.tool.getUrlWithoutQuery( wojilu.str.trimExt( link ) );
            if( currentUrl.indexOf( link )>=0 || link.indexOf( currentUrl )>=0 ) {
                $(this).parent().removeClass( otherClass ).addClass( currentClass );
            };
    	});
    },
    
    writeFrm : function( frmId, htmlContent ) {
        var frmDoc;
		if( document.all ) {
            frmDoc = frames[ frmId ].document;
        } else { 
            frmDoc = $( '#'+frmId )[0].contentWindow.document; 
        };
		frmDoc.open();
		frmDoc.write( htmlContent );
		frmDoc.close();
    },

    // 返回此box的父窗口调用，为window类型
    getBoxParent : function() {
        if( top===self ) return window;
        
        var srcFrmId = null;
        var frmPath = null;
        
        var arrFrames = parent.document.getElementsByTagName("IFRAME");
        for (var i = 0; i < arrFrames.length; i++) {    
            if (arrFrames[i].contentWindow == window) { // box 所在的iframe
                var frmsrc = arrFrames[i].src;
                srcFrmId = wojilu.tool.getQuery( 'srcFrmId', frmsrc );
                if( srcFrmId=='' ) break; // 比如frmLink的页面
                
                // 得到frmPath
                frmPath = wojilu.tool.getQuery( 'frmPath', frmsrc );
                break;                
            }
        }        
        
        if( srcFrmId==null ) return window.parent;

        var getBoxFrmById = function( frmId ) {

            var arrFrames = parent.document.getElementsByTagName("IFRAME");
            for (var i = 0; i < arrFrames.length; i++) {    
                if( arrFrames[i].id == frmId ) return arrFrames[i].contentWindow;
            }        
            return null;
        };

        if( srcFrmId.startsWith ( 'boxFrm' ) ) {
            return getBoxFrmById( srcFrmId );
        }
        
        // 根据frmPath，逐级搜索所有的frmLinkPage
        var searchFrm = function( frmWin, frmNumber ) {
            var arrFrames = frmWin.document.getElementsByTagName("IFRAME");
            for (var i = 0; i < arrFrames.length; i++) {
                if( arrFrames[i].className != 'frmLinkPage' ) continue;
                
                var afrmPath = $('#'+arrFrames[i].id, frmWin.document).attr( 'frmPath' );
                if( afrmPath==frmNumber ) return arrFrames[i].contentWindow;
                var pathResult = searchFrm( arrFrames[i].contentWindow, frmNumber );
                if( pathResult!=null ) return pathResult;
            }
            return null;
        }
        
        var lnkFrm = searchFrm( window.parent, frmPath );
        if( lnkFrm == null ) return window.parent;
        return lnkFrm;
    },     

    sharePrivate : function ( ele, title, url, pic, shareLink, items ) {
        if ( !title ) { title = $('title').html(); }
        if ( !url ) { url = window.location.href; }
        if( pic ) { pic= '&pic='+pic;} else { pic='';}
        if( !shareLink ) { shareLink = '/Share/Add.aspx'; }
        if( !items ) items=['sina','tencent','renren','qzone','baidu','douban'];
        var links = {
            'sina':['v.t.sina.com.cn/share/share.php','新浪微博',0],
            'tencent':['v.t.qq.com/share/share.php','腾讯微博',1],
            'sohu':['t.sohu.com/third/post.jsp','搜狐微博',2],
            'renren':['www.connect.renren.com/share/sharer','人人网',3],
            'qzone':['sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey','QQ空间',4],
            'baidu':['apps.hi.baidu.com/share/','百度空间',5],
            'douban':['www.douban.com/recommend/','豆瓣',6],
            'buzz':['www.google.com/buzz/post','GoogleBuzz',7],
            'kaixin':['http://www.kaixin001.com/repaste/share.php','开心网',8]
        };
        var str='<table style="width:100%;">';
        str+='<tr><td style="width:50px; vertical-align:top;padding-top:4px;">分享到：</td><td style="line-height:200%;" class="snsShareCmds">';
        str+='<a class="frmBox shareSite" xwidth="480" href="'+shareLink+'?url='+encodeURIComponent(url)+'&title='+encodeURIComponent(title)+''+pic+'" title="分享">我的微博</a>';
        for( var i=0;i<items.length;i++ ) {
            str+='<a href="http://'+links[items[i]][0]+'?url='+encodeURIComponent(url)+'&title='+encodeURIComponent(title)+'" target="_blank" class="share'+links[items[i]][2]+'">'+links[items[i]][1]+'</a>';
        }
        str += '</td></tr></table>';
        ele.html( str );
        for( var i=0;i<10;i++ ) {
            $('.share'+i).css( 'background', 'url("'+wojilu.path.img+'/big/sns_share.gif") no-repeat 0px -'+(i*16)+'px' );
        }
        $('.snsShareCmds a').css( {'margin-right':'10px','height':'14px','padding-left':'18px','padding-bottom':'2px'} );
        $('.shareSite').css( 'background', 'url("'+wojilu.path.img+'/s/share.png") no-repeat' );
        wojilu.ui.frmBox( ele );
    },

    share : function( items ) {
        $('.shareCmd').each( function() {
            var data = $(this);
            var aitems = items?items:['sina','tencent'];
            wojilu.tool.sharePrivate( data, data.attr( 'data-title' ), data.attr( 'data-url' ), data.attr( 'data-pic' ), data.attr( 'data-shareLink' ), aitems );
        });
    },

    shareFull : function() {
        wojilu.tool.share( ['sina','tencent','renren','qzone','baidu','douban'] );
    }
};

String.prototype.trimStart = function(str) { return wojilu.str.trimStart(this,str);};
String.prototype.trimEnd = function(str) { return wojilu.str.trimEnd(this,str);};
String.prototype.toInt = function() { return parseInt(this)};
String.prototype.cssVal = function() { return this.trimEnd('px').toInt();};
String.prototype.startsWith = function(str) { if (str.length > this.length) return false; return this.substr(0, str.length) == str;};

String.prototype.toAjax = function(onlyRadom) {
    var strAjax = onlyRadom ? '' : '&ajax=true';
	var indexQuery = this.indexOf( '?' );
	if( indexQuery<0 ) {
		return this + '?rd=' + wojilu.tool.getRandom() + strAjax;
	}
	else {
		var queryString = this.substring( indexQuery+1, this.length );
		var url = this.substring( 0, indexQuery );
		var newQueryString = '';
		var arrQueryItem = queryString.split( '&' );
		for( i=0;i<arrQueryItem.length;i++ ) {
			var item = arrQueryItem[i];
			if( wojilu.str.startsWith( item, 'rd=' ) || wojilu.str.startsWith( item, 'ajax=' ) ) {continue;};
			newQueryString += item;
			if( i<arrQueryItem.length-1 ) {newQueryString += '&';};
		};
		return url + '?rd=' + wojilu.tool.getRandom() + strAjax + '&' + newQueryString;
	}
};

String.prototype.toAjaxFrame = function() {
    var url = this;
    if( url.indexOf( 'frm=true' )>0 ) return url;
	var indexQuery = url.indexOf( '?' );
    var rd = '&rd='+wojilu.tool.getRandom();
	if( indexQuery<0 ) {
		return url + '?frm=true'+rd;
	}
	else {
        if( wojilu.str.endsWith( url, '?' ) ) return url + 'frm=true'+rd;
        if( wojilu.str.endsWith( url, '&' ) )  return url + 'frm=true'+rd;
        return url + '&frm=true'+rd;
    };
};

wojilu.position = {
    getMouse : function(ev){
    	ev = ev || window.event;
    	if(ev.pageX || ev.pageY) return {x:ev.pageX, y:ev.pageY};
    	return { x:(ev.clientX + document.documentElement.scrollLeft - document.body.clientLeft), y:(ev.clientY + document.documentElement.scrollTop - document.body.clientTop) };
    },

    getTarget : function(target){
    	var left = 0; var top  = 0;
    	while (target.offsetParent){ left += target.offsetLeft; top += target.offsetTop; target = target.offsetParent; };
    	left += target.offsetLeft; top += target.offsetTop;
    	return {x:left, y:top};
    },

    getMouseOffset : function(target, ev){
    	ev = ev || window.event;
    	var targetPosition = wojilu.position.getTarget(target);
    	var mousePosition  = wojilu.position.getMouse(ev);
    	return {x:mousePosition.x - targetPosition.x, y:mousePosition.y - targetPosition.y};
    }
};

wojilu.ui = new Object();

var shouldHide = function( e, menuList, menuMore, isMore ) {    
    var pi = wojilu.position.getMouse(e);
    var piMenu = wojilu.position.getTarget( menuList[0] );
    var piMore = wojilu.position.getTarget( menuMore[0] );
    
    var minX = piMenu.x;
    var minY = piMore.y;        
    var maxX = piMenu.y+menuList.height();
    if( isMore ) maxX = piMore.x + menuMore.width();        
    var maxY = piMenu.y+menuList.height();
    
    if( pi.x< minX ) return true;
    if( pi.y< minY ) return true;
    if( pi.x> maxX ) return true;
    if( pi.y> maxY ) return true;
    return false;
};

wojilu.ui.clickMenu = function(eleId) {
    var ele = eleId ? $( eleId + ' .clickMenu' ) : $( '.clickMenu' );
    ele.click( function() {
	var tp = wojilu.position.getTarget(this);
        var item = $( '#'+$(this).attr('list') ).appendTo( 'body' );
        item.css( 'position', 'absolute' ).css( 'zIndex', 999 ).css( 'left' , tp.x ).css( 'top', tp.y + this.offsetHeight );
        item.show().mouseout(function(){ $(this).hide(); }).mouseover( function() {$(this).show();});
    }).mouseout( function(e) {
        var menuList = $( '#'+$(this).attr('list') );
        if( shouldHide( e, menuList, $(this), true ) ) {
            menuList.hide();
        }
    });  
};

wojilu.ui.menu = function(eleId) {
    var ele = eleId ? $( eleId + ' .menuMore' ) : $( '.menuMore' );
    ele.hover(
        function() {
            var item = $( '#'+ $(this).attr( 'list' ) );
            var tp = wojilu.position.getTarget(this);
            var mWidth = item.width();var menuX = tp.x;
            var mLeft = 0;
            if( menuX+ mWidth > document.body.clientWidth ) mLeft = $(this).width()-mWidth;

            $(this).css( 'position', 'relative' );
            item.css( 'position', 'absolute' ).css( 'top', ($(this).height())+'px' ).css( 'left', mLeft ).slideDown('fast');
        },
        function() {
            $( '#'+ $(this).attr( 'list' ) ).hide();
        }    
    );
};

wojilu.ui.tab = function() { wojilu.tool.makeTab( '.otherTab', 'currentTab', 'otherTab' );};

wojilu.ui.pageReturn = function() {
	$( '.btnReturn' ).click( function() {history.back();} );
};

wojilu.ui.boxCancel = function() {
    $( '.btnCancel' ).click( function() {window.parent.wojilu.ui.box.hideBox();} );
};

wojilu.ui.tip = function() {
	var tipInputs = $( '.tipInput' );
	if( tipInputs.length>0 ) {
	    function chkInputTip() {
		    if( $(this).val() !='' && $(this).val() != $(this).attr('tip') ) return;
		    $(this).val( $(this).attr('tip') );
		    $(this).addClass('inputTip');
	    };
	    tipInputs.each( chkInputTip );
	    tipInputs.blur( chkInputTip );
	    tipInputs.click( function() {
		    if( $(this).val() == $(this).attr('tip') ) $(this).val('');
		    $(this).removeClass('inputTip');
	    });
	    tipInputs.parents("form").submit( function () {//此处ltcszk贡献代码(http://www.wojilu.com/ltcszk)。
            tipInputs.click();
	    });

	};
};

wojilu.ui.tree = function() {
    $('.parentNode').click( function() {
        $(this).toggleClass( 'expandNode' );
        $(this).toggleClass( 'collapseNode' );
        $(this).next().slideToggle(100);
    });
};

wojilu.ui.postBack = function(control, httpMethod) {
    var isFrm = window.location.href.indexOf( 'frm=true' )>0;
    var href = $(control).attr( 'href' ).toAjax(true);
    var formId = 'hiddenForm' + wojilu.tool.getRandomInt();
    var postForm = '<form method="POST" action="'+href +'" id="'+formId+'" style="display:none;">'+
        '<input name="Submit1" type="submit" value="__hiddenForm">';
    if(isFrm) {
        postForm +='<input name="frm" type="hidden" value="true" />';
    }
    postForm +='<input name="_httpmethod" type="hidden" value="'+httpMethod+'" /></form>';
    
    $(control).append( postForm );		
    $( '#' +formId ).submit();    
};

wojilu.ui.httpMethod = function(eleId) {
    var elePost = eleId ? $( '.postCmd',$(eleId) ) : $( '.postCmd' );
    var eleDelete = eleId ? $( '.deleteCmd',$(eleId) ) : $( '.deleteCmd' );
    var elePut = eleId ? $( '.putCmd',$(eleId) ) : $( '.putCmd' );
    
    elePost.unbind('click').click( function() { wojilu.ui.postBack(this, 'POST');return false;});
    eleDelete.unbind('click').click( function() { if( confirm( lang.deleteTip ) ) wojilu.ui.postBack(this, 'DELETE'); return false; });    
    elePut.unbind('click').click( function() { wojilu.ui.postBack(this, 'PUT');return false;});

    var isDeleteClick = false;
    var eleAjaxDelete = eleId ? $('.ajaxDeleteCmd',$(eleId) ) : $('.ajaxDeleteCmd');
    eleAjaxDelete.unbind('click').click( function() {
        var cmd = $(this); 
        var ps = cmd.position();	    
        var boxHtml ='<div id="deleteMsgBox" class="ebox" style="width:190px; padding:10px 5px; text-align:center;">'+
            '<div style="margin-bottom:10px;">确实删除吗？</div>'+
            '<div>'+
            '    <input id="btnDeleteConfirm" class="btn btns" type="button" value="确定" /><span id="btnDeleteLoading"></span>'+
            '    <input id="btnDeleteCancel" class="btnOther btns" type="button" value="取消" style="margin-left:20px;" />'+
            '    <input id="adDeleteUrl" type="hidden" />'+
            '    <input id="adRemoveId" type="hidden" />'+
            '</div></div>';
    
        var box = $('#deleteMsgBox');
        if( box.length == 0 ) {$('body').append( boxHtml );box = $('#deleteMsgBox');}
        
        // 本段代码 rubywu 贡献(http://www.wojilu.com/rubywu)
        var psleft = ps.left - 50;
        if (ps.left + 190 > document.body.clientWidth) psleft = document.body.clientWidth - 240;        
        $('#deleteMsgBox').css( 'top', ps.top+20 ).css( 'left', psleft ).slideDown('fast');     
        $('#adDeleteUrl').val( cmd.attr( 'href' ) );
        $('#adRemoveId').val( cmd.attr( 'removeId' ) );
        
        $('#btnDeleteConfirm').click( function() {
            if( isDeleteClick ) return;    	    
            isDeleteClick = true;
            
            var btnDelete = $(this);
            $('#btnDeleteLoading').html( '<img src="'+wojilu.path.img+'/ajax/loading.gif"/>' );
            
            var url = $('#adDeleteUrl').val().toAjax();
            var removeId = $('#adRemoveId').val(  );

            $.post( url, {'_httpmethod':'DELETE'}, function(data) {
                $('#btnDeleteLoading').html( '');
                if( data=='ok' ) {
                    if( $('#'+removeId)[0].tagName.toLowerCase()=='div') {
                        $('#'+removeId).slideUp();
                    }
                    else {
                        $('#'+removeId).hide();
                    };
                    $('#deleteMsgBox').hide();
                    isDeleteClick = false;
                }
                else {
                    alert( data );
                }
            });        
            
            return false;
        });
        
        $('#btnDeleteCancel').click( function() {
            $('#deleteMsgBox').slideUp('fast');
        });

        return false;    
    });

};

wojilu.ui.valid = function() {

    var arrRule = new Array();
    arrRule['name'] = /^[a-zA-Z]{1}([a-zA-Z0-9]|[_]){2,19}$/;          //英文开头，可数字、下划线，3-20个字符
    arrRule['name_cn'] = /^[a-zA-Z\u4E00-\u9FA5]{1}([0-9a-zA-Z \u4E00-\u9FA5]|[_]){1,19}$/;   //可中英文，长度2-19
    arrRule['password'] = /^.{4,20}$/;
    arrRule['int'] = /^[0-9]{1,10}$/;
    arrRule['email'] = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    arrRule['tel'] = /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/;
    arrRule['mobile'] = /^(13[0-9]|15[0|1|2|3|5|6|7|8|9]|18[0|5|6|7|8|9])\d{8}$/;
    arrRule['url'] = /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/;
    arrRule['money'] = /^\d+(\.\d+)?$/;
    arrRule['zip'] = /^[0-9]\d{5}$/;
    arrRule['qq'] = /^[1-9]\d{4,9}$/;

    validPage();

    function validPage() {
        var validator = $( '.valid' );
        if( validator.length<=0 ) return;
        $( '.valid' ).each( addValid );
        var form = $( '.valid' ).parents( 'form' );
        form.submit( function() {
            wojilu.ctx.isValid = true;
            $( '.valid', $(this) ).each( validOne );
            return wojilu.ctx.isValid;
        });
    };

    function addValid() {
        var validSpan = $(this);
        var target = getTarget(validSpan);
        var isShow = validSpan.attr( 'show' );
        if( 'true'==isShow ) validSpan.html( validSpan.attr('msg') );
        var inputType = target.attr( 'type' );
        if(  inputType == 'hidden' ) {
            editorBlur( target, validSpan );
        }
        else if( inputType == 'checkbox' ) {
            var chks = $('input:checkbox[name="'+target.attr('name')+'"]');
            chks.click( function() {validInput(target, validSpan);});
        }
        else if( inputType  == 'radio' ) {
            var rdos = $('input:radio[name="'+target.attr('name')+'"]');
            rdos.click( function() {validInput(target, validSpan);});
        }
        else {
            target.blur( function() {validInput(target, validSpan);});
        };
    };
    
    function editorBlur(target,validSpan) {
        var editor = getEditor(target);
        if( editor == null ) {
            target.blur( function() {validInput(target, validSpan);});
            return;
        };
        
        if (document.all) {
            editor.attachEvent('onblur', function() { validInput(target, validSpan); });
        }
        else {
            editor.addEventListener('blur', function() { validInput(target, validSpan); }, false);
        };
    };
    
    function getEditor(target) {
        var frm = target.next().children('.wojiluEditorFrame');
        if( frm.size()==0 ) return null;
        if( document.all ) { return frm[0]; } else { return frm[0].contentWindow; };
    };

    function validOne() {
        var validSpan = $(this);
        var target = getTarget(validSpan);
        validInput(target, validSpan);
    };

    function setMsg( result, validSpan, msg ) {
        var target = getTarget(validSpan);
        var mode = validSpan.attr( 'mode' );
        if( result==-1 ) {
            if( 'border' == mode ) {
                setErrorMsgSimple( validSpan, msg );
            }
            else {
                setErrorMsg( validSpan, msg );
            }
        }
        else {
            if( 'border' == mode ) {
                setOkMsgSimple(validSpan);
            }
            else {
                setOkMsg( validSpan );
            }
        };
    };

    function setErrorMsg( validSpan, msg ) {
        if( !msg ) msg = lang.exFill;
        validSpan.html( '<span class="validError">'+msg+'</span>' );
        validSpan.css( 'border', '1px #fed22f solid' );
        validSpan.css( 'background', '#ffe45c' );
        validSpan.css( 'color', '#666' );
        wojilu.ctx.isValid = false;
    };
	
    function setErrorMsgSimple( validSpan, msg ){
        var target = getTarget(validSpan);
        if( target.attr( 'type' )=='hidden' ) {
            //editor
            target.parent().parent().addClass( 'inputWarning' );
        }
        else if( target.attr( 'type' )=='checkbox' ) {
            target.parent().parent().addClass( 'inputWarning' );
        }
        else if( target.attr( 'type' )=='radio' ) {
            target.parent().parent().addClass( 'inputWarning' );
        }
        else {
            target.addClass( 'inputWarning' );
        };
        wojilu.ctx.isValid = false;
    };

    function setOkMsg( validSpan ) {
        validSpan.html( '<span class="validOk">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>' );
        validSpan.css( 'border', '0px #ffd324 dotted' );
        validSpan.css( 'background', '#fff' );
    };
    
    function setOkMsgSimple(validSpan) {
        var target = getTarget(validSpan);		
        if( target.attr( 'type' )=='hidden' ) {
            target.next().removeClass( 'inputWarning' );
        }
        else if( target.attr( 'type' )=='checkbox' ) {
            target.parent().parent().removeClass( 'inputWarning' );
        }
        else if( target.attr( 'type' )=='radio' ) {
            target.parent().parent().removeClass( 'inputWarning' );
        }
        else {
            target.removeClass( 'inputWarning' );
        };
    };

    function isValNull( target ) {

        var inputValue = target.val();
        if( target.attr( 'type' )=='checkbox' ) {
            inputValue = $('input:checkbox[name="'+target.attr('name')+'"]:checked').val();
        }
        else if( target.attr( 'type' )=='radio' ) {
            inputValue = $('input:radio[name="'+target.attr('name')+'"]:checked').val();
        }
        
        if( inputValue=='undefined' || inputValue==null || inputValue.length<1 ) return true;
        var stip = target.attr( 'tip' );        
        if( stip && inputValue == stip  ) return true;
        return false;
    };

    function validInput(target, validSpan) {

        var inputValue = target.val();
        var rule = validSpan.attr( 'rule' );
        var msg = validSpan.attr( 'msg' );
        var mode = validSpan.attr( 'mode' );
        var ajaxAction = validSpan.attr( 'ajaxAction' );        

        if( isValNull(target) ) {
            if( 'border' == mode ) {
                setErrorMsgSimple(validSpan, msg);
            }
            else {
                setErrorMsg( validSpan, msg );
            }
            return;
        };

        if( rule==undefined || rule==null || rule.length==0 ) {
        
            if( wojilu.str.hasText( ajaxAction ) ) {
                checkAjaxResult(target, inputValue, validSpan, ajaxAction);
                return;
            }
            else {
                if( 'border' == mode ) {
                    setOkMsgSimple(validSpan);
                } else {
                    setOkMsg( validSpan );
                }
                return;
            };
        };

        for( ruleKey in arrRule ) {
            if( rule==ruleKey ) {
            checkResult( target, inputValue, arrRule[ruleKey], validSpan, msg, ajaxAction );
            return;
            }
        };
            
        if( rule=='same' ) {
            var type = validSpan.attr( 'type' );
            var targetStr = validSpan.attr( 'target' );
            var target = getTargetSelector( targetStr, validSpan );
            var isSame = ( inputValue==target.val() ? 0:-1);
            setMsg( isSame, validSpan, msg );
            return;
        };

        if( rule=='password2' ) {
            var result = inputValue.search( arrRule['password'] );
            var form = validSpan.parents( 'form' );
            var isSame = ( inputValue==$( ':password', form[0] ).not(getTarget(validSpan)).val() );
            var pwdResult = (result==-1 || isSame==false)?-1:0;
            setMsg( pwdResult, validSpan, msg );
            return;
        };

        checkResult( target, inputValue, rule, validSpan, msg, ajaxAction );        
                
        return;
    };
    
    function checkResult( target, inputValue, rule, validSpan, msg, ajaxAction ) {
        var result = inputValue.search( rule );
        if( result == -1 || wojilu.str.isNull(ajaxAction) )
            setMsg( result, validSpan, msg );
        else {
            checkAjaxResult(target, inputValue, validSpan, ajaxAction);
        };
    };
    
    function checkAjaxResult(target, inputValue, validSpan, ajaxAction) {
        var cname = target.attr( 'name' );
        var isChecked = target.attr( 'isChecked' );
        if( 'true'==isChecked ) return;

        wojilu.ctx.isValid = false;
        var pdata = new Object();
        pdata[ cname ] = inputValue;
        $.post( ajaxAction.toAjax(), pdata, function(data) {

            var aResult  =data;
            var aMsg = aResult.Msg;
            result = aResult.IsValid?1:-1;
            if( result==1 ) {
                target.attr( 'isChecked', 'true' );
            }
            setMsg( result, validSpan, aMsg );
        });
    };

    function getTarget(validSpan) {
        var target = validSpan.attr( 'to' );
        if( target=='undefined' || target==null ) {
            return validSpan.prev();
        }
        else {
            return getTargetSelector( target, validSpan );
        };
    };

    function getTargetSelector( target, validSpan ) {
        var form = validSpan.parents( 'form' );
        var tt = $("[name='"+target+"']", form);
        return tt;
    };
};

wojilu.ui.ajaxFormCallback = function(thisForm,validFunc) {
    if( wojilu.ctx.isValid==false ) return false;
        
    var actionUrl = $(thisForm).attr( 'action' ).toAjax();
    var loadingInfo = $(thisForm).attr( 'loading' );
    loadingInfo = loadingInfo ? loadingInfo : "loading...";
    
    var formValues = $(thisForm).serializeArray();
    
    var btnSubmit = $( ':submit', thisForm );
    btnSubmit.attr( 'disabled', 'disabled' );
    btnSubmit.after( ' <span class="loadingInfo"><img src="'+wojilu.path.img+'/ajax/loading.gif"/>' + loadingInfo + '</span>' );
    
    $.post( actionUrl, formValues, function( data ) {
    
        var customCallbackName = $(thisForm).attr( 'callback' );
        if( customCallbackName ) {
            var isContinue = eval( customCallbackName+'(thisForm,data)' );
            if( isContinue==false ) {
                btnSubmit.attr( 'disabled', false ); 
                return false;
            }
        };
        
        if( wojilu.str.isJson( data )==false ) {
            $( '.loadingInfo', thisForm ).html('');
            btnSubmit.attr( 'disabled', false ); 
            alert( data );
            return false;
        };
            
        var result = data;
        $( '.loadingInfo' ).html('');        

        if(result.IsValid){
            validFunc(thisForm,btnSubmit,result);
        }else{
            alert( result.Msg );
            btnSubmit.attr( 'disabled', false );
        };
        
        return false;                   
    });
    
    return false;
};

wojilu.ui.ajaxForward = function( result ) {
    var ftime = result.Time ? result.Time : 800;
    wojilu.tool.forwardPart( result.ForwardUrl,result.PartNumber, ftime );
};

wojilu.ui.ajaxUpdateForm = function( ucallback ) {

    var validFunc = function(thisForm,btnSubmit,result) {
    
        if( result.ForwardUrl ) {
            wojilu.ui.ajaxForward( result );
        };
    
        var insertType = $(thisForm).attr( 'insertType' );
        
        if( 'prepend' == insertType ) {
            $('#'+result.Info).prepend( result.Msg );
        } else {
            $('#'+result.Info).append( result.Msg );
        };
        // 插入元素之后，需要刷新parent
        if( ucallback ) ucallback();
        
        thisForm.reset();
        btnSubmit.attr( 'disabled', false );
    };

    $( '.ajaxUpdateForm' ).submit( function() {
        return wojilu.ui.ajaxFormCallback(this, validFunc);
    });
};

wojilu.ui.ajaxPostForm = function() {

    var validFunc = function(thisForm,btnSubmit,result) {
        if( wojilu.str.hasText(result.Msg) ) {        
            var waringInfo = $( '.warning', thisForm );
            if( waringInfo && waringInfo.length>0 ) {
                waringInfo.show();
                waringInfo.html( '<div class="opok">'+result.Msg+'</div>' );
            }
            else
                $(thisForm).prepend( '<div class="warning strong" style="margin:5px 5px 10px 5px;padding:5px 15px;"><div class="opok">' + result.Msg + '</div></div>' );       
        };
            
        if( result.ForwardUrl ) {
            wojilu.ui.ajaxForward( result );
            return;
        };        
        
        if( wojilu.str.hasText(result.Msg) ) {
            setTimeout( function() {
                var newTip = $('.warning', thisForm );
                newTip.hide();
                btnSubmit.attr( 'disabled', false );
            }, 1500 );
            return;
        }

        btnSubmit.attr( 'disabled', false );        
    };

    $( '.ajaxPostForm' ).submit( function() {
        return wojilu.ui.ajaxFormCallback(this, validFunc);
    });
};

// 使用方法：
//    wojilu.ui.box.showBoxString( html, 200, 100, title );
wojilu.ui.box = {

    id : 0, mouseOffset : null, init : function() {},hideBg:function() {},

    getId:function() {
        var frmdoc = !(top===self)?window.parent.document:document;
        var arrFrames = frmdoc.getElementsByTagName("IFRAME");
        var ifrm=1;
        for (var i = 0; i < arrFrames.length; i++) {    
            if (arrFrames[i].className == 'boxFrm') ifrm++;
        };
        return ifrm;
    },
    
    setWidthHeight:function(bid,width,height){
        var box = $('#box'+bid);
        if(width) { if(width < $(window).width()) { box.css('width',width + 'px'); } else { box.css('width',($(window).width() - 50) + 'px');} };
        if(height) { if(height < $(window).height()) { box.css('height',height + 'px'); } else { box.css('height', ($(window).height() - 50) + 'px'); } };
    },
    
    resetPos:function(bid) {
        var box = $('#box'+bid); var boxWrap = $('#boxWrap'+bid); var bWidth = box.css( 'width' ).cssVal(); var bHeight = box.css( 'height' ).cssVal();
		var box_x = ( $(window).width()  - bWidth  ) / 2;
		var box_y = ( $(window).height() - bHeight ) / 2 + $(document).scrollTop() -60;
		boxWrap.css( 'left', box_x + 'px' );
		boxWrap.css( 'top', box_y + 'px' );    
    },
    
	showBoxString : function(content, boxWidth, boxHeight, btitle, loadingDiv){
    
        if( !(top===self) ) { window.parent.wojilu.ui.box.showBoxString(content,boxWidth,boxHeight,btitle,loadingDiv);return;}
    
        this.id ++; var bid = this.id;if( !btitle ) btitle=lang.boxTitle;
        var zIndex = 100*bid;

        var boxtext = '<div id="overlay'+bid+'" class="overlay" style="display:none;z-index:'+(zIndex)+'"></div>'+
        '<table id="boxWrap'+bid+'" class="boxWrap" style="display:none;margin:auto;" cellspacing="0" cellpadding="0"><tr><td class="btl"></td><td class="btc"></td><td class="btr"></td></tr>'+
        '<tr><td class="bml"></td><td class="bmc">'+
        '<div id="box'+bid+'" class="box"><div id="boxInner'+bid+'" class="boxInner">'+
        '<div id="boxTitle'+bid+'" class="boxTitle">'+
        '<div id="boxTitleText'+bid+'" class="boxTitleText">'+ btitle +'</div>'+
        '<div id="boxClose'+bid+'" class="boxClose" onClick="wojilu.ui.box.hideBox('+bid+')" title="'+lang.closeBox+'"></div>'+
        '<div style="clear:both;"></div></div>'+
        '<div id="boxContents'+bid+'" class="boxContents"></div>'+
        '</div></div>'+
        '</td><td class="bmr"></td></tr>'+
        '<tr><td class="bbl"></td><td class="bbc"></td><td class="bbr"></td></tr></table>';
        
        $( 'body' ).append( boxtext );
        $( '#boxTitle'+bid ).mousedown( function(e) {        
            var boxMove = function(e) { var target = $('#boxWrap'+bid);e = e || window.event; var newPos = wojilu.position.getMouse(e); target.css( 'left', newPos.x-wojilu.ui.box.mouseOffset.x ).css( 'top', newPos.y-wojilu.ui.box.mouseOffset.y ); };            
            var boxEndMove = function(e) { if( document.all ) { document.onmousemove=null; document.onmouseup=null; $('#boxTitle'+bid)[0].releaseCapture();} else { $(document).unbind( 'mousemove' ).unbind( 'mouseup' );} };
            if( document.all ) { document.onmousemove=boxMove; document.onmouseup=boxEndMove; $( '#boxTitle'+bid )[0].setCapture(); } else { $(document).mousemove( boxMove ); $(document).mouseup( boxEndMove ); }
            wojilu.ui.box.mouseOffset = wojilu.position.getMouseOffset( $( '#boxWrap'+bid )[0], e);        
        });
        
        // 第二部分：弹窗默认大小
        var box = $('#box'+bid);
        box.css('width', '400px').css('height', '200px');        

        var boxWrap = $('#boxWrap'+bid);
		var oeverlay = $('#overlay'+bid);
        this.setWidthHeight(bid,boxWidth, boxHeight);    
        oeverlay.css('width', '100%' ).css('height', $(document).height() + 'px' ).show();
		boxWrap.css( 'position', 'absolute' ).css( 'z-index',(zIndex+1) );
        
        var bWidth = box.css( 'width' ).cssVal();
        var bHeight = box.css( 'height' ).cssVal();        
		var box_x = ( $(window).width()  - bWidth  ) / 2;
		var box_y = ( $(window).height() - bHeight ) / 2 + $(document).scrollTop() -60;
		boxWrap.css( 'left', box_x + 'px' ).css( 'top', box_y + 'px' ).show();
        
        $('#boxContents'+bid).append( loadingDiv ).append( content );
        $('#boxTitleText'+bid).html( btitle );
	},
    
	hideBox : function(bid){
        if( !(top===self) ) { window.parent.wojilu.ui.box.hideBox();return;}
        if( !bid ) bid = wojilu.ui.box.id;
        $('#boxWrap'+bid).remove(); $('#overlay'+bid).remove();
		wojilu.ctx.box.isShowing = false; wojilu.ctx.box.param = null;    
        wojilu.ui.box.id--; return false;
	}
}

wojilu.ui.frmBox = function(ele) {

    var objEle = ele ? $('.frmBox', $(ele) ) : $('.frmBox' );
    objEle.click( frmBoxCallback );

    function frmBoxCallback() {
    
        var boxTitle = $(this).attr( 'title' );
        var actionUrl = $(this).attr( 'href' ).toAjaxFrame();

        var srcFrmId = wojilu.tool.getCurrentFrmId();
        if( srcFrmId != null ) {actionUrl = actionUrl + '&srcFrmId='+srcFrmId;}
        if( srcFrmId!=null && srcFrmId.startsWith('frmLinkPage') ) { // 在frmLink的页面中点击弹窗
            var frmPath = $('#'+srcFrmId, parent.document).attr( 'frmPath' );
            actionUrl = actionUrl + '&frmPath='+frmPath;
        };
        
        var boxWidth = $(this).attr( 'xwidth' );
        var boxHeight = $(this).attr( 'xheight' );
        if( !boxWidth ) boxWidth=500;
        if( !boxHeight ) boxHeight = 200;        
        var titleHeight = 26;
        var contentHeight = boxHeight-titleHeight;
        
        var ifrm=wojilu.ui.box.getId();
        var frmId = 'boxFrm'+ifrm;
        var frmClass = 'boxFrm';
        var loadingId = frmId+'Loading';
        var loadingDiv = '<div id="'+loadingId+'" style="width:'+boxWidth+'px;height:'+contentHeight+'px;text-align:center;"><img src="'+wojilu.path.img+'/ajax/big.gif" style="margin-top:30px;"/></div>';        
        var frmHtml = '<iframe id="'+frmId+'" class="'+frmClass+'" src="'+actionUrl+'" frameborder="0" width="'+boxWidth+'" scrolling="no" style="display:none;padding:0px;margin:0px;border:0px red solid;height:'+contentHeight+'px;"></iframe>';
        wojilu.ui.box.showBoxString( frmHtml, boxWidth, boxHeight, boxTitle, loadingDiv );
       
        return false;
    };
};

wojilu.ui.frmLink = function() {
    var getParentFrmPath = function() {
        if( top===self ) return '';
        var arrFrames = parent.document.getElementsByTagName("IFRAME");
        for (var i = 0; i < arrFrames.length; i++) {    
            if (arrFrames[i].contentWindow == window && arrFrames[i].className == 'frmLinkPage') {
                return $('#'+arrFrames[i].id, parent.document).attr( 'frmPath' );
            }
        }
        return '';
    };
    
    var getCurrentFrmNo = function() {

        var arrFrames = window.document.getElementsByTagName("IFRAME");
        var cfrmNo = 0;
        for (var i = 0; i < arrFrames.length; i++) {
            if (arrFrames[i].className == 'frmLinkPage') cfrmNo=cfrmNo+1;
        }
        return cfrmNo+1;
    };
    
    var getFrmPath = function() {
        var pfrmPath = getParentFrmPath();
        if( pfrmPath=='' ) return getCurrentFrmNo();
        return pfrmPath+ '-'+ getCurrentFrmNo();
    };

    $('.frmLink' ).click( function() {
        var frmPath = getFrmPath();var cno = getCurrentFrmNo();

        var rUrl = $(this).attr( 'href' ); 
        if( !rUrl || rUrl=='' || rUrl=='#' ) return false;
        var nolayout=$(this).attr('nolayout');
        if( !nolayout ) alert( '请设置nolayout' );
        var frmUrl = wojilu.tool.appendQuery( rUrl, 'nolayout='+nolayout );
        var loadTo = $(this).attr( 'loadTo' );var scrolling = $(this).attr( 'scrolling' ); scrolling = scrolling? scrolling:'no'; var divLoad = $( '#' + loadTo ); if(divLoad.length==0){return;};divLoad.empty();var win=window; while( !(win === win.parent) ) {win = win.parent;};if(win.history.pushState){win.history.pushState('', '', rUrl)}; var frmId = 'frmLinkPage'+cno; var loadingId = frmId+'Loading'; var frmLinkLoading = '<div id="'+loadingId+'" style="padding:30px 50px;display2:none;"><img src="'+wojilu.path.img+'/ajax/big.gif"/></div>'; divLoad.append( frmLinkLoading ); 
        var frmHtml = '<iframe id="'+frmId+'" class="frmLinkPage" frmPath="'+frmPath+'" nolayout='+nolayout+' src="'+frmUrl+'" frameborder="0" width="100%" scrolling="'+scrolling+'" style="display:none;padding:0px;margin:0px;border:0px red solid;height:300px;"></iframe>'; divLoad.append( frmHtml ); return false;    
    });
};

wojilu.ui.frmLoader = function() {
    $('.frmLoader' ).each( frmLoaderCallback);

    function frmLoaderCallback() {
    
		var frmUrl = $(this).attr( 'url' ).toAjaxFrame()+'&linkTarget=blank'; // 链接在新窗口中打开，而不是在当前iframe中展开页面
        var scrolling = $(this).attr( 'scrolling' );
        scrolling = scrolling? scrolling:'no';
        
		var divLoad = $(this );        
        divLoad.empty();
        
        var frmId = 'frmLoaderPage';
        var loadingId = frmId+'Loading';
        var frmLinkLoading = '<div id="'+loadingId+'" style="padding:30px 50px;"><img src="'+wojilu.path.img+'/ajax/loading.gif"/></div>';
        divLoad.append( frmLinkLoading );
        
        var frmHtml = '<iframe id="'+frmId+'" src="'+frmUrl+'" frameborder="0" width="100%" scrolling="'+scrolling+'" style="display:none;padding:0px;margin:0px;"></iframe>';
        divLoad.append( frmHtml );
       
        return false;
    };
};

wojilu.ui.ajaxLoader = function() {
    $('.ajaxLoader').each( ajaxLoaderCallback );
    function ajaxLoaderCallback() {
		var dataUrl = $(this).attr( 'url' ).toAjax();
        var container = $(this);
        $.post( dataUrl, function(data) {
            container.html( data );
        });
    };
};

wojilu.ui.frmUpdate = function(ele) {
    var eleUpdate = ele ? $('.frmUpdate',$(ele)):$('.frmUpdate');
    eleUpdate.click( function() {

		var frmUrl = $(this).attr( 'href' ).toAjaxFrame();
		var loadTo = $(this).attr( 'loadTo' );
		var divLoad = $( '#' + loadTo );		
        var isHidden = ( divLoad.css( 'display' ) == 'none' );
        
        var txt;
        if( isHidden ) {
            txt = $(this).text();
		    $(this).attr( 'txt', txt );
        }
        else {
            txt = $(this).attr( 'txt' );
        }
        
        var txtHidden = $(this).attr( 'txtHidden' );
        if( !txtHidden ) txtHidden = txt;
		
		if( isHidden==false  ) {
		    divLoad.hide();
		    $(this).text( txt );
		    return;
		}
		
		$(this).text( txtHidden );
		
		var xwidth = divLoad.parent().width();
        
        divLoad.empty();
        
        var frmId = 'updateFrm'+loadTo;
        var loadingId = frmId+'Loading';
        var frmLinkLoading = '<div id="'+loadingId+'" style="text-align:center;"><img src="'+wojilu.path.img+'/ajax/big.gif" style="margin-top:20px;" /></div>';
        divLoad.append( frmLinkLoading );

        var frmHtml = '<iframe id="'+frmId+'" src="'+frmUrl+'" frameborder="0" width="'+xwidth+'" height="50" scrolling="no" style="dispay:none;padding:0px;margin:0px;"></iframe>';
        divLoad.append( frmHtml );
		divLoad.show();

		return false;
    });
};

wojilu.ui.ajaxUpdate = function(callback) {

	$( '.ajaxUpdate' ).click( function() {
		var actionUrl = $(this).attr( 'href' ).toAjax();
		var targetId = $(this).attr( 'update' );
		var target = $( '#' + targetId );
		$.get( actionUrl, function(data){
			target.html( data );
            if( callback ) callback();
		});
		return false;
	});
};

wojilu.ui.editFontSize = function() {

    var getFontSize = function( target ) {
        var fontSize = target.css( 'font-size' );
        var intFontSize = parseInt( wojilu.str.trimEnd( fontSize, 'px' ) );
        return intFontSize;
    };
    
    var getContent = function(target) {
        return $( '#'+ $(target).attr( 'fontTarget' ));
    };
    
    $('.fontBig').click( function() {
        var content = getContent(this);
        var fontSize = getFontSize(content);
        var maxSize = 20;
        var newSize = fontSize>=maxSize ? maxSize:(fontSize+2);
        content.css( 'font-size', newSize+'px');
    });
    
    $('.fontSmall').click( function() {
        var content = getContent(this);  
        var fontSize = getFontSize(content);
        var minSize = 12;
        var newSize = fontSize<=minSize ? minSize:(fontSize-2);
        content.css( 'font-size', newSize+'px');
    });
};

wojilu.ui.editor = function() {
    $('.wEditor').each( function() {
    
        var eName = $(this).attr( 'name' );
        var eHeight = $(this).css( 'height' );
        var ePath = $(this).attr( 'path' );
        var eContent = $(this).attr( 'content' );
        
        var editorPanel = eName.replace( '.', '_' )+'Editor';    
        $(this).after( '<div id=\"'+editorPanel+'\"></div>' );
        $(this).remove();
        
        $.getScript( ePath+'editor.js', function() {
            new wojilu.editor( {editorPath:ePath, height:eHeight, name:eName, content:eContent, toolbarType:'full', uploadUrl:'', mypicsUrl:'' } ).render();
        });
        
    });
};

wojilu.ui.doubleClick = function() {
    $('.click2').dblclick( function() {
        window.location.href = $(this).attr( 'href');
    });
};

// 向左移动的跑马灯效果
wojilu.ui.marquee = function(speed) {
    var eleMarq = $( '.marquee' );
    var marq=eleMarq[0];
    eleMarq.css( {'height':'30px', 'line-height':'30px', 'overflow':'hidden' } ).wrapInner( '<div></div>' );
    var mbody = $( 'div', eleMarq )[0];
    $( mbody ).wrap( '<div style="width:500%;"></div>' ).after( $( mbody ).html() ); // 包装&补齐尾巴
    var marqMove = function() { if(marq.scrollLeft-mbody.offsetWidth>0) {marq.scrollLeft-=mbody.offsetWidth }else{marq.scrollLeft++;} };
    var funcMarq=setInterval(marqMove,speed);
    eleMarq.mouseover( function() {clearInterval(funcMarq)} );
    eleMarq.mouseout( function() {funcMarq=setInterval(marqMove,speed)} );
    $( 'div', eleMarq ).css( 'float', 'left' );
};

wojilu.ui.autoSubmitForm = function() {
    $('.autoSubmitForm').keypress(function(e){
        var isSubmit = false;
        if(e.ctrlKey && e.which == 13 || e.which == 10) {
            isSubmit = true;
        } else if (e.shiftKey && e.which==13 || e.which == 10) {
            isSubmit = true;
        }
                
        if(isSubmit ) {
            $(this).submit();
        }
    })
};

$(document).ready( function() {
    wojilu.ui.menu();
    wojilu.ui.clickMenu();
    wojilu.ui.tab();
    wojilu.ui.httpMethod();
    wojilu.ui.frmBox();
    wojilu.ui.boxCancel();
    wojilu.ui.editor();
    wojilu.ui.frmLoader();
    wojilu.ui.ajaxLoader();
    wojilu.ui.autoSubmitForm();
});


wojilu.site = {
    load : function( callback ) {
        var userCheck = setInterval( function() {
            if( typeof(ctx) == 'undefined' || ctx==null ) return;
            clearInterval( userCheck );
            callback();
        }, 100 );
    }
};

