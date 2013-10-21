<style>
.stepPanel {margin-bottom:30px; padding-bottom:30px;}
#step2, #step3 { display:none;}
.formLeft {width:90px; vertical-align:top;}
</style>

<div style="width:98%; ">
<div id="step1" class="stepPanel" action="">
    <table class="tabHeader" style="width:100%; " cellpadding="0" cellspacing="0">
        <tr>
            <td class="otherTab" style="width:5%"></td>
            <td class="currentTab" style="width:30%">第一步：选择数据源</td>
            <td class="otherTab" style="width:30%">第二步：选择需要导入的目标</td>
            <td class="otherTab" style="width:30%">第三步：保存导入计划</td>
            <td class="otherTab" style="width:5%"></td>
        </tr>
    </table>    
    <div class="tabMain" style="width:100%; padding-top:20px;padding-bottom:30px;">
    <table style="width:98%;margin:10px; margin-top:0px;">
        <tr>
            <td class="formLeft">数据来源：</td>
            <td id="step1Data">#{dataSrc}</td>
        </tr>
        <tr>
            <td class="formLeft">&nbsp;</td>
            <td style="padding-top:15px;">
                <input id="btnStep1" class="btn" type="button" value="下一步" />
                <input id="Button1" type="button" value="_{return}" class="btnReturn" />
                <label class="left10"><input id="chkStep1" type="checkbox" /> 全选</label>
            </td>
        </tr>
    </table>
    </div>
</div>


<div id="step2" class="stepPanel" action="">
    <table class="tabHeader" style="width:100%; " cellpadding="0" cellspacing="0">
        <tr>
            <td class="otherTab" style="width:5%"></td>
            <td class="otherTab" style="width:30%">第一步：选择数据源</td>
            <td class="currentTab" style="width:30%">第二步：选择需要导入的目标</td>
            <td class="otherTab" style="width:30%">第三步：保存导入计划</td>
            <td class="otherTab" style="width:5%"></td>
         </tr>
   </table>    
    <div class="tabMain" style="width:100%; padding-top:20px;padding-bottom:30px;">
    <table style="width:98%;margin:10px; margin-top:0px;">
        <tr>
            <td class="formLeft">导入到：</td>
            <td id="step2Data">
            <!-- BEGIN apps -->
            <div style="margin-bottom:10px;">
                <div style="background:#f2f2f2; border-bottom:0px #ccc solid; padding:2px 10px; margin:5px 0px;">
                    <div style="float:left;"><strong>#{appName}</strong></div>
                    <div style="float:right;"><label><input class="chkApp" type="checkbox" /> 全选</label></div>
                    <div class="clear"></div>
                </div>
                <div style="padding:5px 10px; color:#666;">#{dataTarget}</div>
            </div>
            <!-- END apps --></td>
        </tr>
        <tr>
            <td class="formLeft">&nbsp;</td>
            <td>
                <input id="btnStep2" class="btn" type="button" value="下一步" />
                <input id="return1" class="btnOther" type="button" value="_{return}" />
            </td>
        </tr>
    </table>
    </div>
</div>

<div id="step3" class="stepPanel" action="#{step3Action}">
    <table class="tabHeader" style="width:100%; " cellpadding="0" cellspacing="0">
        <tr>
            <td class="otherTab" style="width:5%"></td>
            <td class="otherTab" style="width:30%">第一步：选择数据源</td>
            <td class="otherTab" style="width:30%">第二步：选择需要导入的目标</td>
            <td class="currentTab" style="width:30%">第三步：保存导入计划</td>
            <td class="otherTab" style="width:5%"></td>
        </tr>
    </table>    
    <div class="tabMain" style="width:100%; padding-top:20px;padding-bottom:30px;">
    <table style="width:98%;margin:10px; margin-top:0px;">
        <tr>
            <td class="formLeft">计划的名称</td>
            <td><input id="importName" name="Name" type="text" /></td>
        </tr>
        <tr>
            <td class="formLeft">投递人</td>
            <td><input id="userName" name="UserName" type="text" /></td>
        </tr>
        <tr>
            <td class="formLeft">是否需要审核</td>
            <td><label><input id="importIsApprove" name="IsApprove" type="checkbox" /> 需要审核</label></td>
        </tr>
        <tr>
            <td class="formLeft">&nbsp;</td>
            <td>
                <input id="btnStep3" class="btn" type="button" value="保存导入列表" />
                <input id="return2" class="btnOther left15" type="button" value="_{return}" />
            </td>
        </tr>
    </table>
    </div>
</div>
<div>&nbsp;</div>
</div>

<script>
_run( function() {

    var objImport = #{itemJson};
    if( objImport.Id>0 ) {
        $('input[type=checkbox]', $('#step1Data') ).val( objImport.DataSrcIds );
        $('input[type=checkbox]', $('#step2Data') ).val( objImport.TargetIds );
        $('#importName').val( objImport.Name );
        $('#userName').val( objImport.Creator );
        if( objImport.IsApprove ) $('#importIsApprove').click();
    }

    $('#chkStep1').click( function() {
        if( $(this).attr( 'checked' ) ) {
            $('input[type=checkbox]', $('#step1Data') ).check('on');
        }
        else {
            $('input[type=checkbox]', $('#step1Data') ).check('off');
        }
    });
    
    $('.chkApp').click( function() {
        var nextPanel = $(this).parent().parent().parent().next();
        if( $(this).attr( 'checked' ) ) {
            $('input[type=checkbox]', nextPanel ).check('on');
        }
        else {
            $('input[type=checkbox]', nextPanel ).check('off');
        }
    });

    function loading( ele ) {
        ele.attr( 'disabled', 'disabled' ).after( '<span><img src="~img/ajax/loading.gif" /> 正在提交……请稍后</span>' );
        return ele.parents('.stepPanel').attr( 'action' ).toAjax();
    }
    
    function xLoading( ele, isHide ) {
        ele.attr( 'disabled', false ).next().remove();
        if( !isHide ) ele.parents('.stepPanel').hide().next().show();
    }
    
    $('.stepPanel .btnOther').click( function() {
        $(this).parents('.stepPanel').hide().prev().show();
    });
    
    //-----------------------------------------------------------------------

    $('#btnStep1').click( function() {
        var btn = $(this);
        var actionUrl = loading( btn );
        xLoading( btn );
    });
    
    $('#btnStep2').click( function() {
        var btn = $(this);
        var actionUrl = loading( btn );
        xLoading( btn );
    });
    
    $('#btnStep3').click( function() {
        var btn = $(this);
        var actionUrl = loading( btn );
        
        var iname = $('#importName').val();
        var isChk = $('#importIsApprove').attr("checked") ? 1:0;
        var srcIdValue = $("input[name='dataSrc']:checked").chkVal();
        var targetValue = $("input[name='dataTarget']:checked").chkVal();
        var uName = $('#userName').val();
       
        $.post( actionUrl, {id:objImport.Id, name:iname,  isApprove:isChk, srcIds:srcIdValue, targetIds:targetValue, userName:uName}, function(data) {
            xLoading( btn, true );

            var m = data;
            if( m.IsValid ) {
                wojilu.tool.forward( '#{returnUrl}', 0 );
            }
            else {
                alert( m.Msg );
            }
            
        });
    });

    
});
</script>