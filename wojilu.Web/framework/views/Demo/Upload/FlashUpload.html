<style>
.demoTitle {margin:20px 0px; font-size:30px; border-bottom:1px #ccc solid; padding:5px 10px;}
</style>

<div style="margin:30px;">
    <div class="demoTitle">flash上传(带进度条) </div>
    <div style="padding:10px 20px;">
        <div><input id="myFile" name="myFile" type="file" /></div>
        <div id="uploadResult" style="margin-top:10px;"></div>
        <div id="uploadProgressBar"></div>
        <div id="imgList"></div>
    </div>
</div>


<script type="text/javascript">

    _run(function () {

        // 总共两个步骤——

        // 1) 上传的配置信息
        var cfg = wojilu.upload.getSettings();

        cfg.uploader = '#{uploadLink}'.toAjax(); // 接受上传的网址
        cfg.postData = #{authJson}; // 客户端验证信息(cookie等)
        cfg.fileTypeExts = '*.jpg;*.gif;*.png;'; // 允许上传的文件类型
        cfg.fileTypeDesc = 'Image Files'; // 选择框中文件类型描述
        cfg.queueID = 'uploadProgressBar'; // 显示进度条的容器

        // 推荐自定义上传处理结果
        cfg.errors= '';
        cfg.validCount = 0;
        cfg.errorCount = 0;

        // 每个文件上传成功之后在这里处理。data是服务端返回的数据。
        cfg.onUploadSuccess = function (file, data, response) {
            var obj = eval( '('+data+')');

            if( !obj.IsValid ) {
                cfg.errors += obj.Msg + '<br>';
                cfg.errorCount += 1;
            }
            else {
                cfg.validCount += 1;
                $('#imgList').prepend( '<div style="margin:5px;"><img src="'+obj.Info+'" /></div>' );
            }
        };

        cfg.onQueueComplete = function (data) {

            var msg = cfg.validCount + ' 个文件上传成功. ';
            if( cfg.errors.length>0 ) msg += '<br>' + cfg.errorCount + ' 个错误：<br>'+cfg.errors;
            $('#uploadResult').html( msg );

            // 重置上传数量
            cfg.errors = '';
            cfg.validCount = 0;
            cfg.errorCount = 0;

        };

        // 2) 将普通上传控件转化为 flash 上传控件
        wojilu.upload.initFlash( '#myFile', cfg );

    });
</script>


<div style="margin:10px 30px; padding:10px;">

    <div style="margin:0px 0px 20px 0px;">
        <div>【优点】实时反映上传进度和速度，一目了然</div>
        <div>【缺点】不是所有平台都兼容，比如 iPad 就不行</div>
    </div>

    <div style="margin:0px 0px 20px 0px;">
        <div>【注意】上传文件 "最大值" 受两个配置决定：</div>
        <div>1) 网站根目录下的 /web.config 中的 	&lt;httpRuntime <span style="color:Blue; font-weight:bold;">maxRequestLength="800000"</span> /&gt;</div>
        <div>2) 同时还要修改 /framework/config/site.config 中的 <span style="color:Blue; font-weight:bold;">UploadPicMaxMB</span>(图片最大上传值) 和 <span style="color:Blue; font-weight:bold;">UploadFileMaxMB</span>(文件最大上传值) 。可在综合系统后台 "网站配置" 中修改。</div>
    </div>

    <div class="strong">客户端</div>
    <div style="margin-bottom:20px;">
        <div>【说明】<br />1) 先在页面中放一个传统的上传控件(即type=file的控件)<br />2) 然后放两个容器div，一个显示进度条(即下面的uploadProgressBar)，一个显示最终上传的统计结果(即下面的uploadResult)<br />3) 如果需要将上传的图片实时显示出来，还可以定义一个 imgList 容器</div>
        <pre class="brush: c-sharp;">
            &lt;div&gt;&lt;input id="myFile" name="myFile" type="file" /&gt;&lt;/div&gt;
            &lt;div id="uploadResult"&gt;&lt;/div&gt;
            &lt;div id="uploadProgressBar"&gt;&lt;/div&gt;
            &lt;div id="imgList"&gt;&lt;/div&gt;
        </pre>
    </div>



    <div class="strong">js 脚本：</div>
    <div>
        <pre class="brush: javascript;">

    _run(function () {

        // 总共两个步骤——

        // 1) 上传的配置信息
        var cfg = wojilu.upload.getSettings();

        cfg.uploader = '&#35;{uploadLink}'.toAjax(); // 接受上传的网址
        cfg.postData = &#35;{authJson}; // 客户端验证信息(cookie等)
        cfg.fileTypeExts = '*.jpg;*.gif;*.png;'; // 允许上传的文件类型
        cfg.fileTypeDesc = 'Image Files'; // 选择框中文件类型描述
        cfg.queueID = 'uploadProgressBar'; // 显示进度条的容器

        // 推荐自定义上传处理结果
        cfg.errors= '';
        cfg.validCount = 0;
        cfg.errorCount = 0;

        // 每个文件上传成功之后在这里处理。data是服务端返回的数据。
        cfg.onUploadSuccess = function (file, data, response) {

            /* 如果是data是json类型数据，可以先eval然后处理*/
            var obj = eval( '('+data+')');

            /* 如果要查看json字符串，可以用 JSON.stringify( objJson ); */

            // 记录错误
            if( !obj.IsValid ) {
                cfg.errors += obj.Msg + '<br>';
                cfg.errorCount += 1; // 将错误上传次数+1
            }
            else {
                cfg.validCount += 1; // 将正确上传的次数+1
                // 其他自定义操作
                $('#imgList').prepend( '&lt;div style="margin:5px;"&gt;&lt;img src="'+obj.Info+'" /&gt;&lt;/div&gt;' );
            }


        };

        cfg.onQueueComplete = function (data) {

            /* 对自定义的上传结果进行汇总显示 */
            var msg = cfg.validCount + ' 个文件上传成功. ';
            if( cfg.errors.length>0 ) msg += '<br/>' + cfg.errorCount + ' 个错误：<br/>'+cfg.errors;
            $('#uploadResult').html( msg );

            // 重置上传数量
            cfg.errors = '';
            cfg.validCount = 0;
            cfg.errorCount = 0;

            /* 插件自带的处理结果(不推荐)
            var msg = data.successful_uploads + ' 个文件上传成功. ';
            if( data.upload_errors&gt;0 ) msg += data.upload_errors + ' 个错误.';
            $('#uploadResult').text( msg );*/

        };

        // 2) 将普通上传控件转化为 flash 上传控件
        wojilu.upload.initFlash( '#myFile', cfg );

    });
        </pre>
    </div>

    <div class="strong">服务端：</div>
    <div>
        <pre class="brush: c-sharp;">
        public void FlashUpload() {
            set( "uploadLink", to( FlashSave ) ); // 接受上传的网址
            set( "authJson", ctx.web.GetAuthJson() );

            // 如果有其他安全cookie，请指定。否则没有权限
            //set( "authJson", ctx.web.GetAuthJson( "_cookieName" ) );
        }

        public void FlashSave() {

            Result result = Uploader.SaveImg( ctx.GetFileSingle() );

            if (result.HasErrors) {
                echoError( result ); // 返回错误信息
                return;
            }

            String picName = result.Info.ToString(); // 获取图片名称
            String picUrl = strUtil.Join( sys.Path.Photo, picName ); // 获取图片完整路径
            picUrl = Img.GetThumbPath( picUrl, ThumbnailType.Medium );// 获取中等缩略图

            echoJsonMsg( "ok", true, picUrl ); // 将图片网址返回给客户端
        }

        </pre>
    </div>
        

</div>