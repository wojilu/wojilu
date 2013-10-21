<div style="margin:30px;">
<div>
    <script  id="editor" type="text/plain">这里可以书写，编辑器的初始内容</script>
</div>


<div class="clear"></div>
<div id="btns">
    <div >
        <button  onclick="getAllHtml()">获得整个html的内容</button>
        <button  onclick="getContent()">获得内容</button>
        <button onclick="setContent()">写入内容</button>
        <button onclick="setContent(true)">追加内容</button>
        <button onclick="getContentTxt()">获得纯文本</button>
        <button  onclick="getPlainTxt()">获得带格式的纯文本</button>
        <button  onclick="hasContent()">判断是否有内容</button>
        <button  onclick="setFocus()">使编辑器获得焦点</button>
    </div>
    <div >
        <button  onclick="getText()">获得当前选中的文本</button>
        <button  onclick="insertHtml()">插入给定的内容</button>
        <button  id="enable" onclick="setEnabled()">可以编辑</button>
        <button onclick="setDisabled()">不可编辑</button>
        <button onclick=" UE.getEditor('editor').setHide()">隐藏编辑器</button>
        <button onclick=" UE.getEditor('editor').setShow()">显示编辑器</button>
    </div>

</div>
<div >
    <button onclick="createEditor()"/>创建编辑器</button>
    <button onclick="deleteEditor()"/>删除编辑器</button>
</div>


<script type="text/javascript">
_run(function () {

    wojilu.editor.bind('editor').show(function (ue) {



        ue.addListener('ready',function(){
            this.focus()
        });

        window.insertHtml = function(){
            var value = prompt('插入html代码','');
            ue.execCommand('insertHtml',value)
        }

        window.createEditor = function(){
            enableBtn();
            UE.getEditor('editor')
        }
        window.getAllHtml = function() {
            alert( UE.getEditor('editor').getAllHtml() )
        }
        window.getContent = function() {
            var arr = [];
            arr.push( "使用editor.getContent()方法可以获得编辑器的内容" );
            arr.push( "内容为：" );
            arr.push(  UE.getEditor('editor').getContent() );
            alert( arr.join( "\n" ) );
        }
        window.getPlainTxt = function() {
            var arr = [];
            arr.push( "使用editor.getPlainTxt()方法可以获得编辑器的带格式的纯文本内容" );
            arr.push( "内容为：" );
            arr.push(  UE.getEditor('editor').getPlainTxt() );
            alert( arr.join( '\n' ) )
        }
        window.setContent = function (isAppendTo) {
            var arr = [];
            arr.push( "使用editor.setContent('欢迎使用ueditor')方法可以设置编辑器的内容" );
            UE.getEditor('editor').setContent( '欢迎使用ueditor',isAppendTo );
            alert( arr.join( "\n" ) );
        }
        window.setDisabled = function() {
            UE.getEditor('editor').setDisabled( 'fullscreen' );
            disableBtn( "enable" );
        }

        window.setEnabled = function() {
            UE.getEditor('editor').setEnabled();
            enableBtn();
        }

        window.getText = function() {
            //当你点击按钮时编辑区域已经失去了焦点，如果直接用getText将不会得到内容，所以要在选回来，然后取得内容
            var range =  UE.getEditor('editor').selection.getRange();
            range.select();
            var txt =  UE.getEditor('editor').selection.getText();
            alert( txt )
        }

        window.getContentTxt = function() {
            var arr = [];
            arr.push( "使用editor.getContentTxt()方法可以获得编辑器的纯文本内容" );
            arr.push( "编辑器的纯文本内容为：" );
            arr.push(  UE.getEditor('editor').getContentTxt() );
            alert( arr.join( "\n" ) );
        }
        window.hasContent = function() {
            var arr = [];
            arr.push( "使用editor.hasContents()方法判断编辑器里是否有内容" );
            arr.push( "判断结果为：" );
            arr.push(  UE.getEditor('editor').hasContents() );
            alert( arr.join( "\n" ) );
        }
        window.setFocus = function() {
            UE.getEditor('editor').focus();
        }
        window.deleteEditor = function() {
            disableBtn();
            UE.getEditor('editor').destroy();
        }
        window.disableBtn = function (str) {
            var div = document.getElementById( 'btns' );
            var btns = domUtils.getElementsByTagName( div, "button" );
            for ( var i = 0, btn; btn = btns[i++]; ) {
                if ( btn.id == str ) {
                    domUtils.removeAttributes( btn, ["disabled"] );
                } else {
                    btn.setAttribute( "disabled", "true" );
                }
            }
        }
        window.enableBtn = function() {
            var div = document.getElementById( 'btns' );
            var btns = domUtils.getElementsByTagName( div, "button" );
            for ( var i = 0, btn; btn = btns[i++]; ) {
                domUtils.removeAttributes( btn, ["disabled"] );
            }
        }

    });
});
</script>
</div>