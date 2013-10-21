
<style>

.siteUrl {width:98%;}
.txtCode {width:230px; height:50px; }
.formLeft {width:60px; vertical-align:top; text-align:right;}
.formLeft3 {width:100px; vertical-align:top; text-align:right;}

fieldset {border:0px #ccc solid;}

table.tabHeader { margin-left:0px;}
div.tabMain { margin-left:0px;}
</style>

<div>


<div style="margin-bottom:10px;border:1px solid #ccc; padding:10px 10px; background:#fff;">
采集功能使用说明：<a href="http://www.wojilu.com/forum1/topic/3021" target="_blank" style="font-size:12px;">http://www.wojilu.com/forum1/topic/3021 <img src="~img/s/external-link.png" /></a>
</div>


<form method="get" id="myform">
    <fieldset id="step1">
    
    <table class="tabHeader" style="width:100%; " cellpadding="0" cellspacing="0">
        <td class="otherTab" style="width:6%"></td>
        <td class="currentTab" style="width:28%">第一步：抓取链接列表</td>
        <td class="otherTab" style="width:28%">第二步：抓取详细页</td>
        <td class="otherTab" style="width:28%">第三步：保存模板</td>
        <td class="otherTab" style="width:10%"></td>
    </table>    
    <div class="tabMain" style="width:100%; padding-top:15px; padding-bottom:20px;">
    
    <table style="width:98%;margin:10px; margin-top:0px; " cellpadding="5" cellspacing="0">

        <tr>
            <td class="formLeft">
                列表网址</td>
            <td>
                <input id="listUrl" name="spiderTemplate.ListUrl" type="text" class="siteUrl" style="width:500px;" />
                </td>
        </tr>
        
        <tr>
            <td class="formLeft">页面编码</td>
            <td>
                 <select id="listEncoding" name="listEncoding">
                    <option value="">默认</option>
                    <option value="utf-8">utf-8</option>
                    <option value="gb2312">gb2312</option>
                    <option value="GBK">GBK</option>
                    <option value="Big5">Big5</option>
                </select>
            
            </td>
        </tr>

        <tr>
            <td style="vertical-align:top; padding-top:22px;" class="formLeft">匹配规则</td>
            <td>
                <div class="red font12" id="lblBeginEnd1">任务说明：抓取所有链接集合(css路径方式)</div>
                <div id="bodyPatternWrap1">
                <div>链接集合设置(css路径方式)<textarea id="ListBodyPattern" name="ListBodyPattern" style="width:98%; height:30px;"></textarea></div>
                <div>匹配链接模板设置(通配符方式)<textarea id="ListPattern" name="ListPattern" style="width:98%; height:30px;"></textarea></div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <input id="btnGetList" class="btn" type="button" value="抓取文章列表" />
                <span id="loading"></span>
                </td>
        </tr>
        </table>
        </div>
        </fieldset>
        

        <fieldset id="step2" style=" display:none;">
        <table class="tabHeader" style="width:100%; " cellpadding="0" cellspacing="0">
            <td class="otherTab" style="width:6%"></td>
            <td class="otherTab" style="width:28%">第一步：抓取链接列表</td>
            <td class="currentTab" style="width:28%">第二步：抓取详细页</td>
            <td class="otherTab" style="width:28%">第三步：保存模板</td>
            <td class="otherTab" style="width:10%"></td>
        </table>    
        <div class="tabMain" style="width:100%;padding-top:15px; padding-bottom:20px;">
        <table id="" style="width:100%;  margin:10px; margin-top:0px;">
        <tr id="outputPanel" style="display:none"><td class="formLeft">抓取列表结果</td><td ><textarea id="txtOutput" style="width:98%; height:200px;"></textarea></td></tr>
        
        
        <tr><td style="vertical-align:top;" class="formLeft">详细页网址</td>
        <td><input id="detailUrl" name="detailUrl" type="text" class="siteUrl" style="width:500px;" />
                详细页编码 <select id="detailEncoding" name="detailEncoding">
                    <option value="">默认</option>
                    <option value="utf-8">utf-8</option>
                    <option value="gb2312">gb2312</option>
                    <option value="GBK">GBK</option>
                    <option value="Big5">Big5</option>
                </select>
        
        <br />
        <span class="" id="selectPanel"></span>

        </td>
        </tr>
        <tr>
            <td style="vertical-align:top; padding-top:22px;" class="formLeft">匹配规则</td>
            <td>
                <div class="red font12" id="lblBeginEnd2">任务说明：抓取网页内容设置(css路径方式)</div>
                <div id="bodyPatternWrap2">
                <div><textarea id="DetailPattern" name="DetailPattern" style="width:98%; height:50px;"></textarea></div>
                </div>
            </td>
        </tr>
        <tr><td>&nbsp;</td><td>
            <input id="firstUrl" type="hidden" />
            <input id="detailTitle" type="hidden" />
            <input id="btnGetDetail" class="btn" type="button" value="抓取详细页内容" />
            <span id="loadingDetail" class="left10"></span>
            <input id="return1" class="btnOther" type="button" value="_{return}" />
            
            </td></tr>

    </table>
    </div>
    </fieldset>
    
    
    <fieldset id="step3" style="display:none;">
        <table class="tabHeader" style="width:100%; " cellpadding="0" cellspacing="0">
            <td class="otherTab" style="width:6%"></td>
            <td class="otherTab" style="width:28%">第一步：抓取链接列表</td>
            <td class="otherTab" style="width:28%">第二步：抓取详细页</td>
            <td class="currentTab" style="width:28%">第三步：保存模板</td>
            <td class="otherTab" style="width:10%"></td>
        </table>    
        <div class="tabMain" style="width:100%;padding-top:15px; padding-bottom:20px;">
        
        <table id="Table1" style="width:100%;  margin:10px; margin-top:0px;">
            <tr id="doutPanel" style="display:none"><td class="formLeft">抓取详细页结果</td><td ><textarea id="dout" style="width:98%; height:280px;"></textarea></td></tr>
            <tr><td class="formLeft3">采集名称</td><td><input id="siteName" type="text" /></td></tr>
            <tr><td class="formLeft3">清除标签</td><td>
                <label><input type="checkbox" name="clearTag" value="script" checked="checked" />script</label>
                <label><input type="checkbox" name="clearTag" value="iframe" checked="checked" />iframe</label>
                <label><input type="checkbox" name="clearTag" value="frame" checked="checked" />frame</label>
                <label><input type="checkbox" name="clearTag" value="object" checked="checked" />object</label>
                <label><input type="checkbox" name="clearTag" value="style" checked="checked" />style</label>
                <label><input type="checkbox" name="clearTag" value="table" />table</label>
                <label><input type="checkbox" name="clearTag" value="font" checked="checked" />font</label>
                <label><input type="checkbox" name="clearTag" value="span" />span</label>
                <label><input type="checkbox" name="clearTag" value="img" />img</label>
                <label><input type="checkbox" name="clearTag" value="a" />a</label>
                <label><input type="checkbox" name="clearTag" value="br" />br</label>
            
            </td></tr>
            <tr><td class="formLeft3">图片选项</td><td>
                <label><input id="checkPic" type="checkbox" checked="checked" /> 采集文章中图片</label></td></tr>
            <tr><td></td><td><input id="btnSave" type="button" class="btn" value="保存采集模板" />
            <span id="loadingSave" class="left10"></span>          
            <input id="return2" class="btnOther left15" type="button" value="_{return}" />
            </td></tr>
        
        </table>
        </div>
        
    </fieldset>
</form>

</div>

<script language="javascript" type="text/javascript">

_run( function() {
    var objT = #{objTemplate};
    
    if( objT.Id>0 ) {
        $('#listUrl').val( objT.ListUrl );
        $('#listEncoding').val( objT.ListEncoding );
        $('#ListBodyPattern').val( objT.ListBodyPattern.replace(/&lt;/g,"<").replace(/&gt;/g,">") );
        $('#ListPattern').val( objT.ListPattern );
        
        $('#DetailPattern').val( objT.DetailPattern );

        $('#siteName').val( objT.SiteName );
    }


    // 第一步：抓取列表
    $('#btnGetList').click( function() {

        var listPattern = $('#ListBodyPattern').val();
   
        var mybtn = $(this);
        mybtn.attr( 'disabled', 'disabled' );

        $('#loading').html( '<img src="~img/ajax/loading.gif" /> 正在抓取……请稍后' );

        var vals = $('#myform').serializeArray();

        $.post( '#{ActionLink}'.toAjax(), vals, function( data ) {
        
            $('#loading').html('');
            mybtn.attr( 'disabled', false );
            
            $('#step1').hide();
            $('#step2').show();
            
            if( $('#listEncoding').val() != '' ) $('#detailEncoding').val( $('#listEncoding').val() );
            
            if( data.IsValid==false ) {
                $('#outputPanel').show();
                $('#txtOutput').val( "采集数据失败，具体原因，请查看 /framework/log/ 下的日志。\n采集网址："+data.listUrl + '\npatternBody=' + data.patternBody+ '\npatternLinks=' + data.patternLinks );
            }
            else {
                var strData = "";
                var strDrop = '<select id="dropLink"><option value="">请选择获取的链接</option>';
                var list = data.List;
                if( list.length ==0 ) {                
                    $('#outputPanel').show();
                    $('#txtOutput').val( '没有结果\n'+vals );
                    return;
                }
                
                for( var i=0;i< list.length;i++ ) {
                    strData += list[i].Title + '_' + list[i].Url + "\n";
                    strDrop += '<option value="'+list[i].Url+'">'+list[i].Title+'</option>';
                }
                
                strDrop += "</select>";
            
                $('#outputPanel').show();
                $('#txtOutput').val( strData );
                $('#step2').show();
                $('#selectPanel').html( strDrop );
                
                $('#dropLink').change( function() {
                    $('#detailUrl').val( $(this).val() );
                });
            }
        });
        
        return false;
    });
    
    $('#return1').click( function() {
        $('#step1').show();        
        $('#step2').hide();
    });

    // 第二步：抓取详细页
    $('#btnGetDetail').click( function() {
    
        $('#loadingDetail').html( '<img src="~img/ajax/loading.gif" /> 正在抓取……请稍后' );

        var mybtn = $(this);
        mybtn.attr( 'disabled', 'disabled' );

        $.post( '#{detailAction}'.toAjax(), 
            {
                'detailUrl': $('#detailUrl').val(), 
                'detailTitle': $('#detailTitle').val(),
                'DetailPattern': $('#DetailPattern').val(),
                'detailEncoding':$('#detailEncoding').val()
            }, 

            function( data ) {
            
                mybtn.attr( 'disabled', false );
                
                $('#step2').hide();
                $('#step3').show();
                $('#loadingDetail').html('');
                $('#doutPanel').show();
                $('#dout').val( data );
        });
    });
    
    $('#return2').click( function() {
        $('#step2').show();        
        $('#step3').hide();
    });
    
    // 第三步：保存模板
    $('#btnSave').click( function() {
        $('#loadingSave').html( '<img src="~img/ajax/loading.gif" /> 保存……请稍后' );
        var mybtn = $(this);
        mybtn.attr( 'disabled', 'disabled' );

        var listUrl = $('#listUrl').val();
        var siteName = $('#siteName').val();
        var listBodyPattern = $('#ListBodyPattern').val();
        var listPattern = $('#ListPattern').val();
        var DetailPattern = $('#DetailPattern').val();
        $.post( '#{saveLink}'.toAjax(), {
            'tid': objT.Id,
            'siteName':siteName,
            'listUrl':listUrl,
            'ListBodyPattern':listBodyPattern,
            'ListPattern':listPattern,
            'DetailPattern':DetailPattern,
            'listEncoding':$('#listEncoding').val(),
            'detailEncoding':$('#detailEncoding').val(),
            'clearTag':$("input[name='clearTag']:checked").chkVal(),
            'checkPic':$('#checkPic').attr("checked")
        }, function(data) {
            mybtn.attr( 'disabled', false );
            $('#loadingSave').html('');
            if( data=='ok') {
                //alert( '保存成功' );
                $('#loading').html( '<span style="color:red;font-weight:bold;">保存成功</span>' );
                wojilu.tool.forward( '#{returnUrl}' );
            }
            else {
                alert( data.Msg );
            }
        });
        
    });
    
    
});

</script>