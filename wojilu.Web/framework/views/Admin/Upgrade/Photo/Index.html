<div>

<style>
input.num { width:60px;}
.btnBig {width:150px;}
</style>

<script>
    _run(function () {

        var myTimer;

        function getStatus() {
            $.post('#{processLink}'.toAjax(), function (data) {
                $('#txtOutput').html(data);
                if (data.indexOf('操作结束') >= 0) {
                    clearInterval(myTimer);
                    $('#loading').html('<img src="~img/m/ok.gif" /> 操作结束');
                }
                else if (data.indexOf('nostart') >= 0) {
                    clearInterval(myTimer);
                    $('#loading').html('<img src="~img/m/ok.gif" /> 操作尚未开始');
                }
                $('.btn').attr('disabled', false);
            });
        };


        $('.myForm').submit(function () {
            var form = $(this);
            var obj = form.serializeArray();
            var url = form.attr('action').toAjax();
            $.post(url, obj, function (data) {
                if (data != 'ok') { alert(data); return; }

                // 每隔200毫秒轮询结果并显示
                myTimer = setInterval(getStatus, 200);
                $('#loading').remove();
                $('.btn', form).parent().append('<span id="loading" style="margin-left:10px;"><img src="~img/ajax/ajaxloading.gif" /></span>');
                $('.btn').attr('disabled', true);

            });
            return false;
        });

    });
</script>

	<div id="listWrap">
	    <div style="margin-bottom:5px; font-size:14px; font-weight:bold;">重新生成缩略图</div>
        <div style="font-size:14px; color:red; margin-top:15px;">如果服务器性能较低，ID之间的数值不要填写太多；建议每次生成少量图片，多次进行。</div>

        <table style="width:98%;">
        <tr>
            <td style="width:280px; vertical-align:top;">
            

                <div>
	                <div style="margin:30px 0px 10px 15px;">
	                    <form action="#{lnkFaceSave}" method="post" class="myForm">
                        <div>
                            开始ID <input type="text" name="startId" class="num right20" />                
                            结束ID <input type="text" name="endId" class="num" />    
                        </div>
                        <div style="margin-top:5px;">
                            <input id="Submit1" type="submit" value="生成头像的缩略图" class="btn btnBig" style="float:left;" />
	                    </div>
                        </form>
	                </div>
                    <div style=" clear:both;"></div>

	                <div style="margin:30px 0px 10px 15px;">
	                    <form action="#{lnkPhotoSave}" method="post" class="myForm">
                        <div>
                            开始ID <input type="text" name="startId" class="num right20" />                
                            结束ID <input type="text" name="endId" class="num" />    
                        </div>
                        <div style="color:#666; margin:5px 0px; line-height:28px; height:28px;"><label><input type="checkbox" name="onlyComputerSize" value="1" /> 只统计图片大小，不生成缩略图</label></div>
                        <div style="margin-top:5px;">
                            <input id="Submit2" type="submit" value="生成图片的缩略图" class="btn btnBig" style="float:left;" />
                        </div>
	                    </form>
	                </div>
                    <div style=" clear:both;"></div>

                </div>

            </td>
            <td id="txtOutput" style=" vertical-align:top;"></td>
        </tr>
        
        </table>

	</div>	


</div>

<style>
#listWrap {padding:5px 10px;margin-top:20px; margin-left:10px;}
</style>