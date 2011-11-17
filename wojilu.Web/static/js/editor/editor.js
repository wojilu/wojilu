wojilu.editorConfig = {
    
    arrToolbar1 : [ 'bold', 'italic', 'underline', 'separator', 'fontFamily', 'fontSize', 'separator', 'forecolor', 'backcolor', 'emotion', 'pic', 'flash', 'separator', 'link', 'unlink', 'table', 'inserthorizontalrule', 'separator', 'about' ],
    arrToolbar2 : [ 'justifyleft', 'justifycenter', 'justifyright', 'separator', 'indent', 'outdent', 'undo', 'redo', 'separator', 'insertunorderedlist', 'insertorderedlist', 'superscript', 'subscript', 'strikethrough', 'removeFormat', 'separator', 'copy', 'cut', 'delete', 'paste','addCode' ],
    
    basicToolbar : [ 'bold', 'forecolor', 'fontFamily', 'fontSize', 'underline', 'strikethrough', 'separator', 'link', 'emotion', 'pic', 'flash', 'inserthorizontalrule','addCode' ],
    
    fontNames : [
        ['宋体', '宋体'],
        ['楷体_GB2312', '楷体'],
        ['黑体', '黑体'],
        ['微软雅黑', '微软雅黑'],
        ['隶书', '隶书'],
        ['仿宋_GB2312', '仿宋'],
        ['arial, helvetica, sans-serif', 'Arial'],
        ['Arial Black', 'Arial Black'],
        ['Verdana, Arial, Helvetica, sans-serif', 'Verdana'],
        ['courier new, courier, mono', 'Courier New'],
        ['times new roman, times, serif', 'Times New Roman']
    ],
        
    lang : {
    
        ok:'确定',
        closeBox:'关闭',
        urlError:'请输入网址',
        
        insertEmotions:'插入表情符',
        defaultColor:'自动',
        insertTable:'插入表格',
        
        url:'网址',
        width:'宽度',
        height:'高度',
        
        imgInsertTitle:'根据图片链接',
        imgUploadTitle:'图片上传',
        imgMyTitle:'浏览我的图片',
        imgInsert:'插入图片',
        imgUpload:'上传图片',
        
        flashInsert:'插入flash',
        addLink:'加入链接',        
        
        aboutUs : '<strong>『我记录』在线编辑器</strong><br/><a href="http://www.wojilu.com" target="_blank">www.wojilu.com</a> &copy; 1999-2011',
        sourceCode : '源代码'        
    },
    
    colors : {
    	'$000000':'黑色', '$993300':'褐色', '$333300':'橄榄色', '$003300':'深绿', '$003366':'深青', '$000080':'深蓝', '$333399':'靛蓝', '$333333':'灰色80%',
    	'$800000':'深红', '$ff6600':'橙色', '$808000':'深黄', '$008000':'绿色', '$008080':'青色', '$0000ff':'蓝色', '$666699':'蓝灰', '$808080':'灰色50%',
    	'$ff0000':'红色', '$ff9900':'浅橙色', '$99cc00':'酸橙色', '$339966':'海绿色', '$33cccc':'水绿色', '$3366ff':'浅蓝', '$800080':'紫罗兰', '$999999':'灰色40%',
    	'$ff00ff':'粉红', '$ffcc00':'金色', '$ffff00':'黄色', '$00ff00':'鲜绿', '$00ffff':'青绿', '$00ccff':'天蓝', '$993366':'梅红', '$c0c0c0':'灰色25%',
    	'$ff99cc':'玫瑰红', '$ffcc99':'茶色', '$ffff99':'浅黄', '$ccffcc':'浅绿', '$ccffff':'浅青绿', '$99ccff':'淡蓝', '$cc99ff':'淡紫', '$ffffff':'白色'
    },
    
    emotions : {
    	'$001':'微笑','$002':'大笑','$003':'抛媚眼','$004':'惊讶','$005':'吐舌头、扮鬼脸','$006':'热烈','$007':'生气、黑脸','$008':'困惑','$009':'尴尬','$010':'悲伤',
    	'$011':'狂笑','$012':'晕、难以理解','$013':'扮酷','$014':'吐','$015':'偷笑','$016':'色、流口水','$017':'hoho','$018':'咬牙切齿','$019':'悄悄话','$020':'撞墙',
    	'$021':'大哭','$022':'书呆子','$023':'打哈欠','$024':'砸、敲头','$025':'汗','$026':'拍手','$027':'狂怒、砍人','$028':'捂嘴偷笑','$029':'不是、不要','$030':'orz (失意体前屈, 或五体投地)',
    	'$031':'厉害、牛、强','$032':'差劲、弱','$033':'握手、或英雄所见略同','$034':'竖起中指(fuck you)','$035':'OK、好的','$036':'女孩','$037':'男孩','$038':'左侧拥抱','$039':'右侧拥抱','$040':'骷髅头',
    	'$041':'爱你、真心','$042':'心碎','$043':'红唇、飞吻','$044':'鲜花、玫瑰','$045':'花儿凋零','$046':'沉睡的月亮、或晚安','$047':'星星','$048':'太阳','$049':'彩虹','$050':'雨伞',
    	'$051':'咖啡','$052':'蛋糕','$053':'音乐','$054':'电影','$055':'电视、视频','$056':'汽车','$057':'飞机','$058':'照相机','$059':'时钟','$060':'礼物',
    	'$061':'狗狗','$062':'小猫','$063':'猪头','$064':'蜗牛','$065':'岛屿','$066':'足球','$067':'电话','$068':'灯泡','$069':'臭大粪、shit'
    },
   
    tblSize : { X : 10, Y : 8 }
};

function val( ele ) {return document.getElementById(ele).value;}
function val( ele, val ) {document.getElementById(ele).value=val;}

wojilu.editor = function( param ) {

    // 配置
    this.editorPath;
    this.skinPath;
    this.imgPath;    
    this.emPath;   
    
    this.fontNames = wojilu.editorConfig.fontNames;
    
    this.basicToolbar = wojilu.editorConfig.basicToolbar;
    this.arrToolbar1 = wojilu.editorConfig.arrToolbar1;    
    this.arrToolbar2 = wojilu.editorConfig.arrToolbar2;
    
    this.lang = wojilu.editorConfig.lang;    
    this.colors = wojilu.editorConfig.colors;
    this.emotions = wojilu.editorConfig.emotions;
    this.tblSize = wojilu.editorConfig.tblSize;
    
    // 属性
    this.id;
    this.frmId;
    this.config;
    this.name;
    this.editorPanel;
    
    this.html;
    this.editor;
    this.doc;
    this.IsReadOnly = false;

    this.hiddenEle;    
    this.toolBar1Prefix;
    this.toolBar1Suffix;
    this.toolBar2Prefix;
    this.toolBar2Suffix;    
    
    this.selection = { range : null, type : null };
    
    // 初始化    
    this.initEditor = function(param) {    
    
        this.id = this.getEditorId();
        this.frmId = this.id+'Frame';
        
        this.config = param;    
        
        if( !this.config.height ) { this.config.height = '300px'; };
        if( !this.config.name ) { this.name = 'Content'; } else { this.name = this.config.name; };
        
        if( !this.config.editorPath ) {this.editorPath='editor/';} else { this.editorPath = this.config.editorPath; };
        if( wojilu.str.endsWith( this.editorPath, '/' )==false ) {this.editorPath=this.editorPath+'/';};
        this.skinPath = this.editorPath + 'skin/';
        this.imgPath = this.skinPath + 'toolbar/';
        this.emPath = this.skinPath + 'em/';
        
        if( !this.config.content ) { this.html=''; } else {this.html=this.config.content;};
                
        this.hiddenEle = this.getHiddenEle( this.name, this.frmId, this.config.height, this.html );
    	this.toolBar1Prefix = '<div class="editorToolBar" style="position:relative;" id="'+this.id+'Toolbar"><table class="editorToolBar1 ebarInner"><tr>';
    	this.toolBar1Suffix = '<td class="wojilu_tool_more"><img src="'+this.imgPath+'down.gif"/></td></tr></table>';
    	this.toolBar2Prefix = '<table class="editorToolBar2 ebarInner" style="position:relative;display:none;"><tr>';
        this.checkSourceCode = '<td class="wojilu_tool_source"><div class="viewSource"><input id="chksrc'+this.id+'" type="checkbox"/><label for="chksrc'+this.id+'">'+this.lang.sourceCode+'</label></div></td>';
    	this.toolBar2Suffix = '</tr></table></div>';
    };
    
     this.getEditorId = function() {        
        if( !wojilu.ctx.editorList ) {
            wojilu.ctx.editorList = new Array();
        };
        var result = 'wojiluEditor' + (wojilu.ctx.editorList.length+1);
        wojilu.ctx.editorList.push( result );
        return result;
    };
    
    this.getHiddenEle = function( name, frmId, height, content ) {
        var result = '<input type="hidden" id="'+name+'" name="'+name+'" value=\''+content+'\' />';
        result += '<div style="padding:0px;border:0px red solid;"><iframe class="wojiluEditorFrame" id="'+frmId+'" name="'+frmId+'" width="100%" height="'+height+'" frameborder="0" scrolling="auto" style="margin:0px;width:100%;border:0px #000 solid;"></iframe></div>';
        return result;
    };
    
    this.initEditor(param);
};

wojilu.editor.prototype = {

    $id : function ( elementName ) { 
        return document.getElementById( elementName ); 
    },
    
    put : function (str) {
        document.write(str);
    },
    
    format : function ( cmd, value ) {
        this.editor.focus();
        if( !document.all && cmd=='backcolor' ) {
            cmd='hilitecolor';
        };
        this.doc.execCommand(cmd, false, value);
    },

    insertHTML : function (html) {
	this.editor.focus();
	if (document.all) { this.addHtml( html ); } else { this.doc.execCommand( 'insertHTML', false, html ); };
    }, 

    addHtml : function ( html ) {
	var selectRange = this.selection.range;
	selectRange.pasteHTML( html  );
	selectRange.collapse( false );
	selectRange.select();
    },

    formatHandler : function (cmd) {
        var that = this;
        this.cmdCell(cmd).click(function(){
            that.format(cmd);
        });
    },
    
    td : function ( name ) {
        if( name=='seperator' ) {return '<td class="editorToolSeperator ></td>';};
        return '<td class="wojilu_tool_'+name+'"><img/></td>';
    },

    getBasicBar : function () {		
	var strBasicBar = '';
	for( i=0;i<this.basicToolbar.length;i++ ){ strBasicBar += this.td(this.basicToolbar[i]); };
	return this.toolBar1Prefix + strBasicBar + this.toolBar2Suffix;
    },

    getFullBar : function () {
	var fullToolbar1 = '';
	for( i=0;i<this.arrToolbar1.length;i++ ){ if(!document.all && (this.arrToolbar1[i]=='copy' || this.arrToolbar1[i]=='cut' || this.arrToolbar1[i]=='paste')){continue;} fullToolbar1 += this.td( this.arrToolbar1[i] ); };
	var fullToolbar2 = '';
	for( i=0;i<this.arrToolbar2.length;i++ ){ if(!document.all && (this.arrToolbar2[i]=='copy' || this.arrToolbar2[i]=='cut' || this.arrToolbar2[i]=='paste')){continue;} fullToolbar2 += this.td( this.arrToolbar2[i] ); };
	return this.toolBar1Prefix + fullToolbar1 + this.toolBar1Suffix + this.toolBar2Prefix + fullToolbar2 + this.checkSourceCode + this.toolBar2Suffix;
    },
    
    getBar : function() {
        if( this.config.toolbarType == 'basic' ) return this.getBasicBar();
        if( this.config.toolbarType == 'full' ) return this.getFullBar();
        return this.getBasicBar();
    },
    
    cmdCell : function (s) { return $('.wojilu_tool_'+s, this.editorPanel); },
    
    addimg : function (s) {
        $('.wojilu_tool_'+s+' img').attr('src', this.imgPath+s+'.gif' );
    },
    
    addImgs : function () {
	for( i=0;i<this.arrToolbar1.length;i++ ){
            var cmd = this.arrToolbar1[i];
            if( cmd=='separator' ) {
                this.cmdCell(cmd).addClass( 'editorSeparator' );
            };
            this.addimg( cmd );
        };
	for( i=0;i<this.arrToolbar2.length;i++ ){
            var cmd = this.arrToolbar2[i];
            if( cmd=='separator' ) {
                this.cmdCell(cmd).addClass( 'editorSeparator' );
            };
            this.addimg( cmd );
        };
    },
   
    writeContentToEditor : function ( htmlContent ) {        
	this.doc.open();
	this.doc.write( htmlContent );
	this.doc.close();
    },
    
    isHtmlChecked : function() {
        return $('#chksrc'+this.id).attr( 'checked' );
    },
    
    makeWritable : function () {
    
        var htmlContent = this.html;
        if( !document.all && wojilu.str.isNull( this.html ) ) {
            htmlContent = '<br/>'+this.html;
        }
        var frameHtml = '<html><link rel="stylesheet" type="text/css" href="'+this.skinPath+ 'style.css'+'" /><body style="background:#fff;border:0px #aaa solid;margin:5px;padding:0px;font-family:verdana;font-size:12px;line-height:150%;">\n' + htmlContent + '\n</body></html>';

	if( document.all ) { this.editor = frames[ this.frmId ]; } else { this.editor = this.$id( this.frmId ).contentWindow; };
	this.doc = this.editor.document;

        var that = this;
        if (document.all) {
            this.writeContentToEditor(frameHtml);            
            if (!this.IsReadOnly) this.doc.designMode = 'on';            
            
            this.editor.attachEvent('onblur', function() {
                if( that.isHtmlChecked() ) {
                    val( that.name, that.doc.body.innerText );
                }
                else {
                    val( that.name, that.doc.body.innerHTML);
                };
            });
        }
        else {
            if (!this.IsReadOnly) this.$id(this.frmId).contentDocument.designMode = 'on';
            this.writeContentToEditor(frameHtml);
            this.editor.addEventListener('blur', function() {
                if( that.isHtmlChecked() ) {
    				var html =that.doc.createRange();
    				html.selectNodeContents(that.doc.body);
                    val( that.name, html );
                }
                else {
                    val( that.name, that.doc.body.innerHTML );
                };
            
            }, false );
            
        };
    }

};

// 主要事件处理
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

wojilu.editor.prototype.addCallback = function () {
    
    // 浏览器内置的格式化命令，可以直接操作
    var fcmds = ['bold', 'italic', 'underline', 'justifyleft', 'justifycenter', 'justifyright', 'indent', 'outdent', 'undo', 'redo',  'superscript', 'subscript', 'strikethrough', 'removeformat', 'unlink', 'insertunorderedlist', 'insertorderedlist', 'copy', 'cut', 'paste', 'delete']; 
    for( var i=0;i<fcmds.length;i++ ) {
        this.formatHandler(fcmds[i]);
    };    
    
    // 自定义的格式化命令（带弹窗或下拉菜单的）
    var dcmds = ['table', 'fontFamily', 'fontSize', 'forecolor', 'backcolor', 'emotion', 'pic', 'flash', 'link', 'about', 'addCode'];
    for( var i=0;i<dcmds.length;i++ ) {
        this.dlgHandler(dcmds[i]);
    };
    
    var that = this;
    this.cmdCell( 'inserthorizontalrule' ).click( function() {
        that.cacheSelection(); // 针对IE
        that.insertHTML( '<hr>' );
    });
    
    var ocmds = ['source'];
    for( var i=0;i<ocmds.length;i++ ) {
        eval( 'this.'+ocmds[i]+'Handler();' );
    };
    
    this.ebarMoreHandler();
};

wojilu.editor.prototype.dlgHandler = function (cmd) {
    var that = this;
    this.cmdCell(cmd).click(function() {
    
        that.cacheSelection();
        that.dialog( cmd, this );
        
        $('.closeSpan').click(function() {
            $('#'+$(this).attr('target')).hide();
            that.restoreSelection();
        });
        
    });
};

wojilu.editor.prototype.cacheSelection = function () {
    if( !document.all ) {return;};
    this.editor.focus();
    this.selection.range = this.doc.selection.createRange();
    this.selection.type = this.doc.selection.type;
};
    
wojilu.editor.prototype.dialog = function (cmd, target) {

    var divId = cmd + 'Box';
    var dlg = this.$id(divId);
    var hasDlg = ( dlg==null || dlg == 'undefined' )?false:true;
    if( hasDlg ) {
        $(dlg).show();
        return;
    } 
    else {
        var divString;
        eval( 'divString = this.'+cmd+'Dialog();' );
        $( 'body' ).append( divString );
        dlg = this.$id(divId);
    
        var tp = wojilu.position.getTarget(target);
        $(dlg).css( 'display', 'block' ).css( 'position', 'absolute' ).css( 'zIndex', 98 )
            .css( 'left', tp.x - dlg.offsetWidth/2 + 15 ).css( 'top', tp.y + target.offsetHeight - 2 )
            .css( 'font-family', 'verdana' ).css( 'font-size', '12px' ).addClass( 'editorCmdBox' );
            
        eval( 'this.'+cmd+'Handler();' );
    }
};

wojilu.editor.prototype.restoreSelection = function () {
    if( document.all && this.selection.range && this.selection.type!='Control') {
        this.selection.range.select();
    };
};
    
wojilu.editor.prototype.ebarMoreHandler = function () {
    var that = this;
    $('.wojilu_tool_more', that.editorPanel ).click( function() {
        $('.editorToolBar2', that.editorPanel ).toggle();            
        var arrow = $('img', this);
        if( wojilu.str.endsWith( arrow.attr('src'), 'down.gif' ) ) {
            arrow.attr( 'src', that.imgPath+'right.gif' );
        }
        else {
            arrow.attr( 'src', that.imgPath+'down.gif' );
        };            
    });
};

// 对话框
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
wojilu.editor.prototype.closecmd = function (boxId) {
    return '<span class="closeSpan" target="'+boxId+'" unselectable="on"><img src="'+this.emPath+'close.gif"/> '+this.lang.closeBox+'<span>';
};
    
wojilu.editor.prototype.closeImg = function (boxId) {
    return '<span class="closeSpan" target="'+boxId+'" unselectable="on"><img src="'+this.emPath+'close.gif"/><span>';
};

//----------------------------------------------------------------
wojilu.editor.prototype.addCodeDialog= function () {
    var addCodeBoxId = 'addCodeBox';
    return '<div id="'+addCodeBoxId+'" style="width:400px;height:240px;background:#f2f2f2;padding:10px 10px 10px 20px; border:1px #aaa solid;">'+
        '<table style="width:390px"><tr><td>请选择代码类型：<select><option value="csharp">c#</option><option value="java">java</option><option value="c">c</option><option value="python">python</option><option value="ruby">ruby</option><option value="vb">vb</option><option value="php">php</option><option value="delphi">delphi</option><option value="js">js</option><option value="xml">xml</option><option value="sql">sql</option><option value="css">css</option><option value="text">纯文本</option></select></td><td style="text-align:right;">'+this.closeImg( addCodeBoxId )+'</td></tr></table><div><textarea style="width:380px;height:180px"></textarea></div><div><input type="submit" class="btn btns" value="插入代码" /></div><div style="clear:both;"></div></div>';
}

wojilu.editor.prototype.addCodeHandler= function () {
    var that = this;
    var addCodeBoxId = 'addCodeBox';
    var codeBox = $('#'+addCodeBoxId );
    $('input[type=submit]', codeBox ).click( function() {
        var codeType = $('select', codeBox ).val();
        var code = $('textarea', codeBox ).val();
        if( $.trim( code )=='' ) {
            alert( '请填写代码！' );
            $('textarea', codeBox ).focus();
            return;
        }
        code = code.replace( /</g, '&lt;' ).replace( />/g, '&gt;' );
        that.cacheSelection(); // 针对IE
        if( codeType=='text' ) {
            that.insertHTML( code.replace(/\n/g,"<br/>") );
        }
        else {
            that.insertHTML( '<div class="hide">-----code-----</div><pre class="brush: '+codeType+';" >'+code+'</pre><div class="hide">-----code-----</div>' );
        }
        codeBox.hide();
    });
}
    
//----------------------------------------------------------------
wojilu.editor.prototype.tableDialog = function () {
    var tblBoxId = 'tableBox';
    var lblInsert = this.lang.insertTable;
    var tblString = '<div id="'+tblBoxId+'"><table><tr><td colspan="9" style="width:145px;border:0px;"><div id="lblTableInfo">'+lblInsert+'</div></td><td style="border:0px;">'+this.closeImg(tblBoxId)+'</td></tr></table><table id="tbl" class="drawTable" cellspacing="0" cellpadding="0">';

    for( y=1;y<this.tblSize.Y+1;y++ ) { 
        tblString += '<tr>';
        for( x=1;x<this.tblSize.X+1;x++ ) {
            tblString += '<td x="'+x+'" y="'+y+'" unselectable="on" ></td>';	
        }
        tblString += '</tr>'; 
    }

    tblString += '</table></div>';        
    return tblString;
};

wojilu.editor.prototype.tableHandler = function () {
    var that = this;
    var tableBox = $('#tableBox');
    $('.drawTable',tableBox).mouseout( function() {
        that.clearCellBg();
    });
    
    $('.drawTable td',tableBox).mouseover( function() {
        var x = $(this).attr('x');
        var y = $(this).attr('y');
        that.hlCellBg(x,y);
    }).unbind('click').click( function() {
        var x = $(this).attr('x');
        var y = $(this).attr('y');
        that.insertTable(x,y);
        tableBox.hide();
    });        
};
    
wojilu.editor.prototype.hlCellBg = function ( x, y ) {	
    $('#lblTableInfo').html( x + "×" + y +  " " + this.lang.insertTable );
    var rows = this.$id('tbl').getElementsByTagName('tr'); 
    for( var m=0;m< y;m++ ) { 
        var cols = rows[m].getElementsByTagName('td');
        for( var n=0;n<x;n++ ) {
            cols[n].style.background = "#808000"; 
        };
    };
};

wojilu.editor.prototype.clearCellBg = function () {
    var cell = this.$id('tbl').getElementsByTagName('td');
    for( var i=0;i<cell.length;i++ ) { 
        cell[i].style.background = "#ffffff"; 
    };
};
    
wojilu.editor.prototype.insertTable = function ( x, y ) {
    var strTable = '<table style="width:60%;border-collapse:collapse;" border="1">\n';
    for( row=0; row<y; row++ ) {
        strTable +='<tr>';
        for( col=0; col<x; col++ ) {
            strTable += '<td>&nbsp;</td>';
        };
        strTable += '</tr>\n';
    };
    strTable += '</table>';
    this.insertHTML( strTable );		
};

//----------------------------------------------------------------
  
wojilu.editor.prototype.fontFamilyDialog = function () {
    var boxId = 'fontFamilyBox';
    var fname = function( name, text ) { return '<div unselectable="on" style="font-family:'+name+'" fontName="'+name+'" class="fontFamilyItem">'+text+'</div>'; };
    var result = '<div id="'+boxId+'" style="width:150px;background:#fff;border:1px outset;"><div style="padding:5px 10px;" class="fontFamilyContainer">';
    
    for( var i=0;i<this.fontNames.length;i++ ) {
        result += fname( this.fontNames[i][0], this.fontNames[i][1] );        
    };
    
    result += '</div></div>';
    return result;
};

wojilu.editor.prototype.fontSizeDialog = function () {
    var boxId = 'fontSizeBox';
    var fsize = function( size, pt ) { return '<div unselectable="on" style="font-size:'+pt+';" fontSize="'+size+'" class="fontSizeItem">'+pt+'</div>'; };
    var result = '<div id="'+boxId+'" style="width:138px;background:#fff;border:1px outset;"><div style="padding:5px 10px;" class="fontSizeContainer">';
    result += fsize(2,'10pt') + fsize(3,'12pt')  + fsize(4,'14pt') + fsize(5,'18pt') + fsize(6,'24pt') + fsize(7,'36pt');
    result += '</div></div>';
    return result;
};

wojilu.editor.prototype.fontFamilyHandler = function () {
    var fontFamilyBox = $('#fontFamilyBox');
    fontFamilyBox.mouseout( function() {
        $(this).hide();
    });
    
    $('.fontFamilyContainer', fontFamilyBox).mouseover( function() {
        fontFamilyBox.show();
    });

    var that = this;
    $('.fontFamilyItem', fontFamilyBox).unbind('click').click( function() {
        var fontName = $(this).attr('fontName');
        that.format( 'FontName', fontName );
        fontFamilyBox.hide();
    });
};
    
wojilu.editor.prototype.fontSizeHandler = function () {
    var fontSizeBox = $('#fontSizeBox');
    fontSizeBox.mouseout( function() {
        $(this).hide();
    });
    
    $('.fontSizeContainer', fontSizeBox).mouseover( function() {
        fontSizeBox.show();
    });

    var that = this;
    $('.fontSizeItem', fontSizeBox).unbind('click').click( function() {
        var fontSize = $(this).attr('fontSize');
        that.format( 'FontSize', fontSize );
        fontSizeBox.hide();
    });
};

//----------------------------------------------------------------

wojilu.editor.prototype.forecolorDialog = function () {
    return this.getColorDlg('forecolorBox', '#000000');
};
    
wojilu.editor.prototype.backcolorDialog = function () {
    return this.getColorDlg('backcolorBox', '#ffffff');
};
    
wojilu.editor.prototype.getColorDlg = function (boxId, dcolor) {
    var colorTableString = '<div id="'+boxId+'" class="colorBox"><table class="tblColorSelector" cellspacing="0" cellpadding="3"><tr><td colspan="7" class="colorNormal" style="text-align:left;cursor:pointer;"><table><tr><td><table style="background:'+dcolor+';" class="colorInner"><tr><td></td></tr></table></td><td style="width:;text-align:center;">'+this.lang.defaultColor+'</td></tr></table></td><td>'+this.closeImg(boxId)+'</td></tr><tr>';

    var colorNum = 1;
    for( var p in this.colors ) {
        if( colorNum == 9 || colorNum==17 || colorNum==25 || colorNum ==33 ) { colorTableString += '</tr>\n<tr>'; }

        var cp = p.substring(1,p.length);
        var strNum = colorNum;
        if( colorNum<10 ) strNum = "0"+colorNum;
        var ctitle = eval( 'this.colors.$'+cp);
        colorTableString += '<td class="colorNormal" title="'+ctitle+'"><table style="background:#'+cp+';" class="colorInner"><tr><td></td></tr></table></td>';

        colorNum += 1;
    };
    colorTableString += '</tr></table></div>';
    return colorTableString;
};

wojilu.editor.prototype.forecolorHandler = function () {
    this.colorHandler('forecolor');
};
    
wojilu.editor.prototype.backcolorHandler = function () {
    this.colorHandler('backcolor');
};
    
wojilu.editor.prototype.colorHandler = function (cmd) {
    var that = this;
    var colorBox = $('#'+cmd+'Box');
    $('.colorNormal', colorBox).unbind('click').click( function() {
        var innerBox = $('.colorInner', this );
        colorBox.hide();
        that.restoreSelection();
        var colorValue = innerBox.css( 'background-color' );
        that.format( cmd, colorValue );
    });        
};

//----------------------------------------------------------------
    
wojilu.editor.prototype.emotionDialog = function () {
    var emBoxId = 'emotionBox';
    var emsString = '<div id="'+emBoxId+'" style="background:#ffffff;border:1px solid #aaa;padding:10px;"><table cellpadding="3" class="emSelector"><tr><td colspan="9" style="font-size:12px;">'+this.lang.insertEmotions+'</td><td colspan="1" style="text-align:center;">'+this.closeImg(emBoxId)+'</td></tr>';

    var trS = '<tr id="emRow1">';
    var num=1;
    for( i= 1;i<70;i++ ) {
        if( i== 11 || i==21 || i==31 || i==41 || i==51 || i==61  ) {
            num+=1;
            trS+= '</tr>\r\r<tr id="emRow'+num+'">' ;
        };
        
        var number = i;
        if( i<10 ) {
            number = '00' + i;
        }
        else if(i<100){
            number='0'+i;
        };
        
        trS+= '<td class="emotionItem"><img src="'+this.emPath+number+ '.gif" title="'+eval( 'this.emotions.$'+number )+'" /></td>';
    }
    trS+='</tr></table></div>';

    emsString = emsString + trS;
    return emsString;
};

wojilu.editor.prototype.emotionHandler = function () {
    var that = this;
    var emBox = $('#emotionBox');
    $('.emotionItem', emBox).unbind('click').click( function() {
        var imgPath = $('img', this).attr('src');
        emBox.hide();
        that.restoreSelection();
        that.format( 'InsertImage', imgPath );        
    });
};
    
//----------------------------------------------------------------

function addEditorPic( editorString, murl ) {
    eval( editorString+'.insertImg("'+murl+'");' );
};

function addEditorPicAndLink( editorString, murl, picLink ) {
    eval( editorString+'.insertImgAndLink("'+murl+'", "'+picLink+'");' );
};


wojilu.editor.prototype.picDialog = function () {
    var imgBoxId = 'picBox';
    var result = '<div class="getImgBox" id="'+imgBoxId+'">';
    result += '<table cellpadding="0" cellspacing="0" class="insertTable">';
    result += '	<tr unselectable="on">';
    result += '		<td class="tabItem currentTab" id="addPicUrl">'+this.lang.imgInsertTitle+'</td>';
    result += '		<td class="tabItem" id="uploadPic"></td>';
    result += '		<td class="tabItem" id="myPics"></td>';
    result += '		<td class="closeTab">'+this.closecmd(imgBoxId)+'</td>';
    result += '	</tr>';
    result += '	<tr>';
    result += '		<td colspan="4" class="actionPanel">';
    result += '		<div class="insertPannel">'+this.lang.url+':&nbsp;<input class="editorImgUrl" type="text" />&nbsp;<input class="btnInsertImg btn btns" type="button" value="'+this.lang.imgInsert+'" /></div>';
    result += '		</td>';
    result += '	</tr>';
    result += '</table>';
    result += '</div>';    
    return result;
};

wojilu.editor.prototype.picHandler = function () {

    var editorName = this.name+'Editor';
    var picBox = $('#picBox');        
    var that = this;
    
    $('.tabItem', picBox).click( function() {
        $('td.currentTab', picBox).removeClass( 'currentTab' );
        $(this).addClass('currentTab');
    });    
    
    var frmHtml = function( frmUrl ) {
        var frmId = 'picFrmEditor';
        var xwidth = '530px';
        var furl = frmUrl.toAjaxFrame()+'&editor='+editorName;
        return '<iframe id="'+frmId+'" src="'+furl+'" frameborder="0" width="'+xwidth+'" height="120" scrolling="no" style="padding:0px;margin:0px;"></iframe>';
    };    
    
    var bindBtnInsertImg = function() {
        $('.btnInsertImg', picBox).unbind('click').click( function() {
            var txtUrl = $('.editorImgUrl', picBox);
            var imgUrl = txtUrl.val();
            if( imgUrl=='' ) { alert( that.lang.urlError ); txtUrl.focus(); return false; }
            
            picBox.hide();
            that.restoreSelection();
            that.format( 'InsertImage', imgUrl );
        });
    };
    
    bindBtnInsertImg();
    
    var uploadUrl = that.config.uploadUrl;
    var myPicsUrl = that.config.mypicsUrl;
    
    $('#addPicUrl').click( function() {
        var lnkBox = '<div class="insertPannel">'+that.lang.url+':&nbsp;<input class="editorImgUrl" type="text" />&nbsp;<input class="btnInsertImg" type="button" value="'+that.lang.imgInsert+'" /></div>';
        $('.actionPanel', picBox).html(lnkBox);
        bindBtnInsertImg();
    });
    
    if( wojilu.str.hasText( uploadUrl ) ) {
        var tdUploadPic = $('#uploadPic', picBox);
        tdUploadPic.text( that.lang.imgUploadTitle );
        tdUploadPic.click( function() {
            $('.actionPanel', picBox).html(frmHtml(uploadUrl));
        });
    };
    
    if( wojilu.str.hasText( myPicsUrl ) ) {
        var tdMyPics = $('#myPics', picBox);
        tdMyPics.text( that.lang.imgMyTitle );
        tdMyPics.click( function() {
            $('.actionPanel', picBox).html(frmHtml(myPicsUrl));
        });
    };
};

wojilu.editor.prototype.insertImg = function (imgUrl) {
    this.format( 'InsertImage', imgUrl );  
};

wojilu.editor.prototype.insertImgAndLink = function (imgUrl, imgLink) {
    this.restoreSelection();
    this.insertHTML( '<a href="'+imgLink+'" target="_blank"><img src="'+imgUrl+'" /></a><br/><br/><br/>' );
};

//----------------------------------------------------------------

wojilu.editor.prototype.flashDialog = function () {
    var flashBoxId = 'flashBox';
    var result = '<div id="'+flashBoxId+'" unselectable="on">';
    result += '<table border="0">';
    result += '	<tr><td colspan="2"><table class="editorBoxTitle"><tr><td class="editorBoxTitleString">'+this.lang.flashInsert+'</td><td style="text-align:right;">'+this.closecmd(flashBoxId)+'</td></tr></table></td></tr>';
    result += '	<tr><td class="flashBoxLeft">'+this.lang.url+'</td><td><input type="text" id="editorFlashUrl" class="flashBoxUrl"/></td></tr>';
    result += '	<tr><td class="flashBoxLeft">'+this.lang.width+'</td><td><input type="text" id="editorFlashWidth" value="480" style="width:50px;"/> px</td></tr>';
    result += '	<tr><td class="flashBoxLeft">'+this.lang.height+'</td><td><input type="text" id="editorFlashHeight" value="360" style="width:50px;" /> px</td></tr>';
    result += '	<tr><td colspan="2" style="text-align:center;padding-bottom:20px;"><input class="btnInsertFlash btn btns" type="button" value="'+this.lang.flashInsert+'" /></td></tr>';
    result += '</table>';
    result += '</div>';
    return result;
};

wojilu.editor.prototype.flashHandler = function () {
    var that = this;
    var flashBox = $('#flashBox');
    $('.btnInsertFlash', flashBox).unbind('click').click( function() {
        var flashUrl = $( '#editorFlashUrl' ).val();
        if( flashUrl == '' ) { alert( that.lang.urlError ); $( '#editorFlashUrl' ).focus(); return false; }
        var flashHtml = that.getFlashHtml( flashUrl, $( '#editorFlashWidth' ).val(), $( '#editorFlashHeight' ).val() );
        flashBox.hide();
        that.restoreSelection();
        that.insertHTML(flashHtml);
    });
};    
    
wojilu.editor.prototype.getFlashHtml = function (srcUrl,width,height) {
    return '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0" width="' + width + '" height="' + height + '">  <param name="movie" value="' + srcUrl + '" /><param name="quality" value="high" /><embed src="' + srcUrl + '" quality="high" pluginspage="http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash" type="application/x-shockwave-flash" width="' + width + '" height="' + height + '"></embed></object>';
};

//----------------------------------------------------------------
    
wojilu.editor.prototype.linkDialog = function () {
    var linkBoxId = 'linkBox';
    var result ='<div id="'+linkBoxId+'" class="editorBox" unselectable="on"><div><table><tr><td>'+this.lang.url+': <input class="editorLinkUrl" type="text" value="http://" />&nbsp;<input class="btnCreateLink btn btns" type="button" value="'+this.lang.addLink+'" unselectable="on" style="margin-right:10px;"/></td><td>'+this.closecmd(linkBoxId)+'</td></tr></table></div></div>';
    return result;
};

wojilu.editor.prototype.linkHandler = function () {
    var that = this;
    var linkBox = $('#linkBox');
    $('.btnCreateLink', linkBox).unbind('click').click( function() {

        var lnkText = $('.editorLinkUrl', linkBox);
        var linkUrl = lnkText.val();
        
        if( linkUrl=='' || linkUrl=='http://' ) { alert( that.lang.urlError ); lnkText.focus(); return false; }
        that.createLink( linkUrl );
        linkBox.hide();
    });
};

wojilu.editor.prototype.createLink = function ( url ) {
    this.editor.focus();
    if (document.all) { this.addHtml( '<a href="'+url+'">' + this.selection.range.htmlText + '</a>' ); } else { this.doc.execCommand( 'createlink', false, url ); }       
};

//----------------------------------------------------------------

wojilu.editor.prototype.aboutDialog = function () {
    var aboutBoxId = 'aboutBox';
    return '<div id="'+aboutBoxId+'" class="editorBox"><div style="text-align:center;">'+ this.lang.aboutUs +'<br/><input id="btnOk" type="button" value="'+this.lang.ok+'" /></div></div>';
};
   
wojilu.editor.prototype.aboutHandler = function () {
    $('#btnOk').click( function() {
        $('#aboutBox').hide();
    });
};   

//----------------------------------------------------------------

function setNewLine( html ) {    
    var nh = html;        
    nh = nh.replace( /<br>/gi , '<br>\n' ); 
    nh = nh.replace( /<li/gi , '\n<li' ); 
    nh = nh.replace( /<\/li>/gi , '<\/li>\n' );         
    nh = nh.replace( /<ul/gi , '\n<ul' ); 
    nh = nh.replace( /<\/ul>/gi , '<\/ul>\n' );         
    nh = nh.replace( /<div/gi , '\n<div' ); 
    nh = nh.replace( /<\/div>/gi , '<\/div>' );         
    nh = nh.replace( /<p/gi , '\n<p' ); 
    nh = nh.replace( /<\/p>/gi , '<\/p>' );    
    return nh;
}

wojilu.editor.prototype.sourceHandler = function () {
    var that = this;
    var srcTd = this.cmdCell('source');
    var chk = $('input', srcTd);
    var viewSource = $('.viewSource', this.editorPanel );
    chk.click( function() {
    
        if( this.checked ) {                
            
            var sp = wojilu.position.getTarget(viewSource[0]);
            viewSource.appendTo($('body'));
            viewSource.css( 'position', 'absolute' ).css( 'zIndex', 99 ).css( 'left', sp.x ).css( 'top', sp.y );
            
            var toolbar = $('.editorToolBar', that.editorPanel );
            that.showTempDiv( toolbar[0] );

            if( document.all || $.browser.safari  ) {
                var htmlSrc = setNewLine( that.doc.body.innerHTML );
                that.doc.body.innerText = htmlSrc;
            }
            else {

                var htmlSrc = setNewLine( that.doc.body.innerHTML );
                var html = document.createTextNode(htmlSrc);
                that.doc.body.innerHTML = "";
                that.doc.body.appendChild(html);      
            };            
        }
        else {
            $('#tempDiv').hide();
            var td = $('.wojilu_tool_source', that.editorPanel );
            viewSource.appendTo(td).css( 'position', 'static' );
            
            if( document.all ) {
                that.doc.body.innerHTML = that.doc.body.innerText;
            }
            else {
                var html = that.doc.createRange();
                html.selectNodeContents(that.doc.body);
                that.doc.body.innerHTML = html.toString();
            };
        };
    });
};
    
wojilu.editor.prototype.showTempDiv = function (target) {
    
    var result = this.$id('tempDiv');
    if( result ) {
        $(result).show();
        this.showPosition(target, 0);  
        return;
    };
    
    var divString='<div id="tempDiv" style="width:'+$(target).width()+'px;height:'+$(target).height()+'px;background:#eeeeee;filter:alpha(opacity=70); opacity:0.7;"></div>';
    $( 'body' ).append( divString );
    this.showPosition(target, 0);    
};

wojilu.editor.prototype.showPosition = function(target, offset) {
    var tp = wojilu.position.getTarget(target);
    $('#tempDiv').css( 'position', 'absolute' ).css( 'zIndex', 9 ).css( 'left', (tp.x+offset) ).css( 'top', tp.y );
};

//----------------------------------------------------------------

wojilu.editor.prototype.render = function() {

    
    wojilu.tool.loadCss( this.skinPath + 'style.css' );    
    var toolBar = this.getBar();    
    var html = '<div id="'+this.id+'" class="wojiluEditor">';
    html += toolBar;
    html += this.hiddenEle;
    html += '</div>';
    $( '#'+this.name.replace('.','_')+'Editor' ).append( html );
    
    this.editorPanel = $('#'+this.id);
    
    this.addImgs();    
    this.makeWritable();    
    this.addCallback();
    
    var frmrId = this.frmId;

    var isPart = function() {
        if( wojilu.tool.getQuery( 'frm' ) == 'true') return true;
        if( wojilu.tool.getQuery( 'nolayout' ) !='' ) return true;
        return false;
    };

    // 弹窗中编辑器不可resize
    if( isPart() ==false ) {
        $('#'+this.id).resizable({
            resize: function(event, ui) {
                var that = $(this)[0];
                var frmr = $('#'+frmrId, that);
                $(frmr).height( $(that).height()-30 );
            }
        });
    };
    

};


