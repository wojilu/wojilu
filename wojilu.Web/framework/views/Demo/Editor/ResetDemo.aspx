<div style="margin:30px;">
    <h2>重置编辑器和销毁编辑器示例</h2>
    <div class="content" id="simple" style="width:700px;"></div>
    <p><input type="button" onclick="simple()" value="重置编辑器内部参数"><span id="txt"></span></p>
    <p><input id="destroy" type="button" onclick="destroy()" value="销毁编辑器"></p>
    <script type="text/javascript" charset="utf-8">

        _run(function () {

            wojilu.editor.bind('simple').height(150).show(function (editor) {

                window.simple = function () {
                    if (editor) {
                        editor.setContent("编辑器内部变量已经被重置!");
                        editor.reset();
                    }
                };

                window.destroy = function () {
                    editor.destroy();
                    editor = null;
                    clearInterval(timer);
                    var button = document.getElementById("destroy");

                    button.value = "重新渲染";
                    button.onclick = function () {
                        editor = UE.getEditor('simple');
                        this.value = "销毁编辑器";
                        this.onclick = destroy;
                        timer = setInterval(setMsg, 100);
                    }
                };

                function setMsg() {
                    if (editor && editor.undoManger) {
                        document.getElementById("txt").innerHTML = "编辑器当前保存了 <span style='color: red'> " + editor.undoManger.list.length + " </span>次操作";
                    }
                }
                var timer = setInterval(setMsg, 100);

            });
        });
    </script>
</div>