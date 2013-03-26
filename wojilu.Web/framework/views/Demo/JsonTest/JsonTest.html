<div style="padding:20px; background:#fff;">

<style>
.formPanel {margin:10px; padding:10px; background:#eee; line-height:200%;}
.formTitle {font-weight:bold; padding-left:80px;}
.tdLeft { vertical-align:top;width:80px;}
</style>
    <div></div>

    <div class="formPanel">
        <div style="margin:10px 0px;">当你选择了下面一项，js将向服务端发送请求，服务端返回json数据。</div>
        <div style="margin-bottom:20px;">
        <select id="dropItem" href="#{jsonLink}">
            <option value=0>请选择……</option>
            <option value=1>1</option>
            <option value=2>2</option>
            <option value=3>3</option>
        </select>
        </div>
    </div>

    <div style="border-bottom:1px #ccc solid;margin:0px 0px 20px 0px;">&nbsp;</div>
    
    <div>
    <div class="strong">客户端代码</div>
    <pre class="brush: c-sharp;">
    
&lt;select id="Select1" href="&#35;{jsonLink}"&gt;
    &lt;option value=0&gt;请选择……&lt;/option&gt;
    &lt;option value=1&gt;1&lt;/option&gt;
    &lt;option value=2&gt;2&lt;/option&gt;
    &lt;option value=3&gt;3&lt;/option&gt;
&lt;/select&gt;

&lt;script&gt;
_run( function() {
    $('#dropItem').change( function() {    
        $.post( $(this).attr( 'href'), {'Id':$(this).val()}, function( data ) {
        
            // 因为服务端通过echoJson直接返回json对象，所以这里可以直接通过Name属性获值
            // 如果服务端echoText返回普通字符串，那么客户端需要使用 eval('('+data+')') 额外解析    
            alert( data.Name );
             
        });        
        
    });
});
&lt;/script&gt;
            
    </pre>
    </div>
    
    <div>
    <div class="strong">服务端代码</div>
    
    <div style="margin-left:5px;">
    特别说明：如果自己拼接字符串，请<br />
        1）给 <strong style="color:red;">json属性名加上双引号</strong>，否则客户端Jquery无法正常解析。<br />
        2）其他字符串的引号必须使用双引号，如果使用单引号也不能正常解析。<br />
        举例： <strong style="color:Blue;">{Name:&#39;张三&#39;}</strong> 需要改成 <strong style="color:red;">{"Name":"张三"}</strong> 
        。</div>
    <pre class="brush: c-sharp;">
    
public void JsonTest() {
    set( "jsonLink", to( JsonResult ) );
}

public void JsonResult() {

    // 获取客户端提交的值
    int id = ctx.PostInt( "Id" );

    // 使用dic存放数据，可以避免手工拼接的错误
    Dictionary&lt;String, String&gt; dic = new Dictionary&lt;String, String&gt;();
    dic.Add( "Name", "你选择了" + id  );

    // 使用 echoJson 方法返回 json 字符串，客户端jquery可以直接使用
    echoJson( dic );
}
    
    </pre>
    </div>
</div>

<script>
_run( function() {
    $('#dropItem').change( function() {    
        $.post( $(this).attr( 'href'), {'Id':$(this).val()}, function( data ) {            
            alert( data.Name );        
        });        
        
    });
});
</script>