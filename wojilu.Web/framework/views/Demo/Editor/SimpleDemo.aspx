
<div style="margin:30px;">
    <h1 style=" margin-bottom:20px;">UEditor简单功能</h1>

     <form id="form" method="post" target="_blank" action="#{ActionLink}">

    <script type="text/plain" id="post.Content" name="post.Content">
        <p>这是<strong>编辑器</strong>内容</p>
    </script>
    <script>_run(function () { wojilu.editor.bind('post.Content').height(220).line(2).show() })</script>


    <div class="top10"><button type="submit" class="btn btn-primary"><i class="icon-plus icon-white"></i> 发布数据</button> </div>
    </form>

    <h2>代码</h2>
    <pre class="brush: c-sharp;">
    &lt;script type="text/plain" id="post.Content" name="post.Content"&gt;
        &lt;p&gt;这是&lt;strong&gt;编辑器&lt;/strong&gt;内容&lt;/p&gt;
    &lt;/script&gt;
    &lt;script&gt;
    _run(function () { 
        wojilu.editor.bind('post.Content').height(220).line(2).show();
    });
    &lt;/script&gt;
    </pre>
    <div>注意：编辑器的id和name尽量保持一致，否则无法使用客户端验证。</div>

</div>