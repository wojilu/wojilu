<div style="margin:30px;">
    <div style="margin:20px 0px; font-size:30px; border-bottom:1px #ccc solid; padding:5px 10px;">简单异步上传 </div>

    <div style="padding:10px 20px;">
	    <form action="#{uploadLink}" id="myForm" name="myForm" enctype="multipart/form-data" method="post">
    	    <div>
    		    上传图片：<input type="file" id="myFile" name="myFile"/>
    	    </div>
    	    <iframe name="frmUpload" id="frmUpload" style="display:none"></iframe>
            <div id="fileStatus"></div>
        </form>   					
    </div>
</div>


<script>

    function showPic(picUrl) {
        $('#fileStatus').html('<img src="' + picUrl + '" />');
        $('#myForm')[0].reset();

        // 如果这个表单还要提交内容，那么最好重置它的targe
        $('#myForm').attr('target', '_self');
    }

    _run(function () {

        $('#myFile').change(function () {

            $('#myForm').attr( 'target', 'frmUpload'); // 直到选择文件的时候才修改
            $('#fileStatus').html('<img src="~img/ajax/ajaxloading.gif" />');
            $('#myForm').submit();
        });

    });

</script>


<div style="margin:10px 30px; padding:10px;">

    <div style="margin:0px 0px 20px 0px;">
        <div>【优点】异步上传，不会刷新页面；兼容各种平台、各种浏览器，不像flash有平台限制；</div>
        <div>【缺点】实现进度条稍微有些麻烦(需要另外通过js查询服务端进度)。</div>
    </div>

    <div class="strong">客户端</div>
    <div style="margin-bottom:20px;">
        <div>【说明】关键是使用 iframe 技术，将 form 的 target 属性指向 iframe，实际是通过 iframe 上传。<br />
    上传完毕，通过 iframe 中的 js 脚本，将上传结果返回给主页面显示。</div>
        <pre class="brush: c-sharp;">

	        &lt;form action="#{uploadLink}" id="myForm" name="myForm" enctype="multipart/form-data" method="post"&gt;
    	        &lt;div&gt;
    		        上传图片：&lt;input type="file" id="myFile" name="myFile"/&gt;
    	        &lt;/div&gt;
    	        &lt;iframe name="frmUpload" id="frmUpload" style="display:none"&gt;&lt;/iframe&gt;
                &lt;div id="fileStatus"&gt;&lt;/div&gt;
            &lt;/form&gt;
        </pre>
    </div>



    <div class="strong">js 脚本：</div>
    <div>
        <pre class="brush: javascript;">
        _run(function () {

            $('#myFile').change(function () {

                $('#myForm').attr( 'target', 'frmUpload'); // 关键：将 form 的 target 属性指向 iframe
                $('#fileStatus').html('&lt;img src="~img/ajax/ajaxloading.gif" /&gt;');
                $('#myForm').submit();
            });

        });

        // 上传完毕，供 iframe 调用
        function showPic(picUrl) {
            $('#fileStatus').html('&lt;img src="' + picUrl + '" /&gt;');
            $('#myForm')[0].reset();

            // 如果这个表单还要提交内容，那么最好重置它的target
            $('#myForm').attr('target', '_self');
        }
        </pre>
    </div>

    <div class="strong">服务端：</div>
    <div>
        <pre class="brush: c-sharp;">
        public void FrameUpload() {
            set( "uploadLink", to( FrameSave ) ); // 接受上传的网址
        }

        public void FrameSave() {

            logger.Info( "iframe upload" );

            Result result = Uploader.SaveImg( ctx.GetFileSingle() );
            String picName = result.Info.ToString(); // 获取图片名称
            String picUrl = strUtil.Join( sys.Path.Photo, picName ); // 获取图片完整路径
            picUrl = Img.GetThumbPath( picUrl, ThumbnailType.Medium ); // 获取中等缩略图

            // 这里的内容返回给 iframe
            StringBuilder sb = new StringBuilder();
            sb.Append( "&lt;script&gt;" );
            // 这段内容在iframe中，所以通过 parent 来调用主页面的方法
            sb.Append( "parent.showPic('" + picUrl + "')" );
            sb.Append( "&lt;/script&gt;" );

            echoText( sb.ToString() );
        }

        </pre>
    </div>
        

</div>