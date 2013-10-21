<div style="margin:30px;">
<h1>UEditor自定义插件</h1>

<!--style给定宽度可以影响编辑器的最终宽度-->
<script type="text/plain" id="myEditor">
    <p><img src="http://ueditor.baidu.com/website/images/banner-dl.png" alt=""></p>
    <p>插件描述：选中图片，在其上单击，会改变图片的边框！</p>
</script>
<script type="text/javascript">


    //创建一个在选中的图片单击时添加边框的插件，其实质就是在baidu.editor.plugins塞进一个闭包

    var myPlugin = function () {

        var me = this;
        //创建一个改变图片边框的命令
        // 注意：命令名称必须全部小写，不能使用驼峰格式
        me.commands["addborder"] = {
            execCommand: function () {

                //获取当前选区
                var range = me.selection.getRange();
                //选区没闭合的情况下操作
                if (!range.collapsed) {
                    //图片判断
                    var img = range.getClosedNode();
                    if (img && img.tagName == "IMG") {
                        //点击切换图片边框
                        img.style.border = img.style.borderWidth == "5px" ? "1px" : "5px solid red";
                    }
                }
            }
        };
        //注册一个触发命令的事件，同学们可以在任意地放绑定触发此命令的事件
        me.addListener('click', function () {
            me.execCommand("addborder");
        });

    };


    var xPlugin = function () {
        var me = this;

        // 注意：命令名称必须全部小写，不能使用驼峰格式
        me.commands["showtext"] = {
            execCommand: function () {
                alert('begin showtext');
            }
        };

        me.addListener('click', function () {
            me.execCommand("showtext");
        });
    };


    // 加载编辑器
    _run(function () {

        wojilu.editor.bind('myEditor')
            .plugin('addborder', myPlugin)
            .plugin('xPlugin', xPlugin)
            .show();

    });

</script>

</div>