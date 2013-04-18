define(['wojilu.core.ems', 'wojilu.app.microblog.view'], function(x, viewHelper) {

    function _getEmotionTable() {
        var trS = '<table cellpadding="3" class="emSelector"><tr id="emRow1">';
        var num=1;
        var icount = 0;
        for( i= 1;i<70;i++ ) {
            
            if( icount>9 ) {
                num+=1;
                trS+= '</tr>\r\r<tr id="emRow'+num+'">' ;
                icount =0;
            }
            
            var number = i;
            if( i<10 ) {
                number = '00' + i;
            }
            else if(i<100){
                number='0'+i;
            };
            
            trS+= '<td class="emotionItem" emId="'+x.ems['$'+number]+'"><img src="'+wojilu.path.js+'/editor/skin/em/'+number+'.gif" title="'+x.ems['$'+number]+'" /></td>';
            
            icount++;
        }
        trS+='</tr></table>';

        return trS;
    }
    

    function addTextarea( eleId, tag ) {
        var txt =  tag;
        var ele = document.getElementById(eleId);
        var selection = document.selection;
        ele.focus();
        if (typeof ele.selectionStart != "undefined") {
            var s = ele.selectionStart;
            ele.value = ele.value.substr(0, ele.selectionStart) + txt + ele.value.substr(ele.selectionEnd);
            ele.selectionEnd = s + txt.length;
        } else if (selection && selection.createRange) {
            selection.createRange().text = txt;
        } else {
            ele.value += txt;
        }
    }

    function selectTagTip( eleId, tag ) {
        var txt =  "#" + tag + "#";
        var ele = document.getElementById( eleId );
        var oVal = ele.value;
        ele.focus();
        if (window.getSelection){        
            var beginIndex = oVal.indexOf(txt)+1;
            var endIndex = beginIndex + txt.length-2;
            ele.setSelectionRange( beginIndex, endIndex );
        }
        else {
            var range=ele.createTextRange();
            range.findText(tag);
            range.select(); 
        }
    }

    function addAndSelect( eleId, txt ) {
        addTextarea( eleId, '#'+txt+'#' );
        selectTagTip( eleId, txt );
    }

    // IE6根据上传结果显示预览图
    function _setBgPic( thumbUrl, picUrl ) {
        $('#uploadPicThumb').html( '<img src="'+thumbUrl+'" />' );
        $('#picUrl').val( picUrl );
        $('#mcmdImg').click();
        
        $('#picUploadBox').slideUp('fast');
        $('#lnkPicUplader').click();
    }

    var mbBlogCount;
    var _countStr = function(total) {
        mbBlogCount = '还可以输入 <span class="restCount">'+total+'</span> 字';
        var inputLength = $( '#txtContent' ).val().length;
        var restCount = total - inputLength;
        if( restCount>=0 ) {
            $( '#wmsg' ).html( '还可以输入 <span class="restCount">'+restCount+'</span> 字' );
        }
        else {
            var zCount = -restCount;
            $( '#wmsg' ).html( '已超出 <span class="restCountWarning">'+zCount+'</span> 字' );
        }
    };

    function _bindMbEvent(getVideoUrl, total, pubUrl ) {


        $('#mcmdEmotion').click( function() {       
            var ps = $(this).position();
            $('#emBox').css( 'top', ps.top+20 ).css( 'left', ps.left-40 ).slideDown('fast');
        });
        
        $('.emotionItem').click( function() {
            var emId = $(this).attr( 'emId' );
            addTextarea( 'txtContent', '['+emId+']' );
            $('#emBox').hide();
        });
        
        $('.mBoxClose').click( function() {
            $(this).parent().slideUp('fast');
            $('#lnkPicUplader').click();
        });
        
        $('#mcmdTag').click( function() {
            addAndSelect( 'txtContent', '请在这里输入自定义话题' );
        });
        
       
        $('#piccmdUpload').click( function() {
            
            if( $.browser.msie && $.browser.version == "6.0" ) {
                var ps = $(this).position();
                $('#iePicBox').css( 'top', ps.top+20 ).css( 'left', ps.left-100 ).slideDown('fast');
                $('#lnkPicUplader').click();
            }
            else {
                var ps = $(this).position();
                $('#picUploadBox').css( 'top', ps.top+20 ).css( 'left', ps.left-100 ).slideDown('fast');
            }
        });
        
        $('#btnVideo').click( function() {
            var videoUrl = $('#videoUrl').val();
            $.post( getVideoUrl.toAjax(), {'videoUrl':videoUrl}, function( data ) {
                var msg = data;//eval( '('+data+')' );
                if( msg.IsValid ==false ) {
                    alert( msg.Msg );
                }
                else {
                    var videoId = msg.Info;
                    var mcontent = $('#txtContent').val();
                    $('#txtContent').val( mcontent + ' ' + videoUrl );
                    $('#videoId').val( videoId );
                }
            });
        });

        $('#mcmdVideo').click( function() {
            var ps = $(this).position(); // 获取当前被点击的元素的位置
            $('#videoBox').css( 'top', ps.top+20 ).css( 'left', ps.left-130 ).slideDown('fast'); // 通过绝对定位来显示图层
        });

        var pCount = function() { _countStr(total);};
        
        $( '#txtContent' ).keydown(pCount);
        $( '#txtContent' ).keyup(pCount);


        $('#btnPublish').click( function() {
            var btn = $(this);
            $('#loading').html( '<img src="'+wojilu.path.img+'/ajax/loading.gif"/>' );
            var mycontent = $( '#txtContent' ).val();
            var picUrl = $( '#picUrl' ).val();
            var videoId = $( '#videoId' ).val();
            var srcType = $('#srcType' ).val();

            if( wojilu.str.isNull( mycontent ) ) {			
                $('#loading').html( '' );
                alert( '请填写内容' );
                $( '#txtContent' ).focus();
                return false;
            }
            
            btn.attr( 'disabled', 'disabled' );
            
            var url = pubUrl.toAjax();
            $.post( url, { 'Content':mycontent, 'PicUrl':picUrl, 'videoId':videoId, 'srcType':srcType }, function( data ) {

                btn.attr( 'disabled', false );
                
                var msg =data; 
                
                $('#loading').html( '' );

                if( msg.IsValid ) {
                    if( msg.SrcType == 'shareBox' ) {
                        $('#mbPubBox').html( '<div style="margin:10px auto;margin-top:30px;text-align:center;font-size:20px;font-weight:bold;width:120px;height:55px;line-height:55px;color:#666;" class="okBig">操作成功！</div>' );
                        setTimeout( function() {
                            wojilu.tool.getBoxParent().wojilu.ui.box.hideBox();	
                        }, 900);
                    }
                    else if( msg.SrcType == 'mbHome' ) {

                        $( '#txtContent' ).val( '');
                        $('#wmsg').html( mbBlogCount );

                        $('#newMicroblogPanel').prepend( msg.Info );

                        var newBlog =$($('.mblogOne', $( '#newMicroblogPanel'))[0]);
                        newBlog.hide();
                        newBlog.fadeIn('slow');

                        // 给新博增加点击事件
                        viewHelper.addBlogEvent( newBlog );
                        wojilu.ui.frmBox( newBlog );
                        wojilu.ui.frmUpdate( newBlog );
                        wojilu.ui.httpMethod( newBlog );
                        $('.favCmd', newBlog ).hide();

                        $('#uploadPicThumb').html( '' );
                        $('#picUrl').val( '' );
                        $('#videoId').val( '' );

                        newBlog.css( 'padding', '5px' ).css( 'background', '#fffbe2' ).css( 'border', '1px #fed22f solid' );
                        setTimeout( function() {
                            newBlog.css( 'background', '#fff' ).css( 'border-top', '0px' ).css( 'border-left', '0px' ).css( 'border-right', '0px' );
                        }, 2000 );
                    }
                    else {
                        wojilu.tool.getRootParent().wojilu.tool.forward( msg.ForwardUrl,0);
                    }
                }
                else {
                    alert( '对不起,操作出错='+msg.Msg );
                }
                
            });
        });


    }

    return {
        bindMbEvent : _bindMbEvent,
        getEmotionTable : _getEmotionTable,
        countStr : _countStr 
    };

});
