<div style="padding:20px; background:#fff;">

<style>
.formTitle {font-weight:bold; padding-left:80px;}
.tdLeft { vertical-align:top;width:80px;}
</style>
    <div></div>

    <div class="formPanel">
    <form method="post" action="#{savePostLink}">    
        <table  class="table">
            <tr><td colspan="2" class="formTitle">多选框验证</td></tr>
            
            <tr>
                <td class="tdLeft">普通模式</td>
                <td>
                    #{book}
                    <span class="valid" msg="请填写内容" to="book"></span>
                </td>
            </tr>
            
            <tr><td></td><td><input id="Submit1" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>

    <div class="formPanel">
    <form method="post" action="#{savePostLink}">    
        <table  class="table">
            <tr><td colspan="2" class="formTitle">多选框验证</td></tr>
            
            <tr>
                <td class="tdLeft">边框模式</td>
                <td>            
                    #{music}                
                    <span class="valid" mode="border" to="music"></span>
                </td>
            </tr>
            
            <tr><td></td><td><input id="Submit2" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>
        
        
    <div style="border-bottom:1px #ccc solid;margin:0px 0px 20px 0px;">&nbsp;</div>
    
    <div>
        <div class="strong">服务端代码</div>
        <pre class="brush: c-sharp;">

        public void Checkbox() {

            Dictionary&lt;string, string&gt; book = new Dictionary&lt;string, string&gt;();
            book.Add( "小说", "1" );
            book.Add( "诗歌", "2" );
            book.Add( "散文", "3" );
            book.Add( "戏剧", "4" );

            checkboxList( "book", book, null );

            Dictionary&lt;string, string&gt; music = new Dictionary&lt;string, string&gt;();
            music.Add( "民谣", "1" );
            music.Add( "流行", "2" );
            music.Add( "摇滚", "3" );
            music.Add( "电子", "4" );

            checkboxList( "music", music, null );
        }
        </pre>
        <div class="strong">前端代码</div>
        <div style="margin:5px; color:Red;">注意：因为多选框呈现出来的是多个checkbox，所以验证控件必须加上 to="控件name" 属性</div>
        <pre class="brush: c-sharp;">
        
            &lt;tr&gt;
                &lt;td class="tdLeft"&gt;普通模式&lt;/td&gt;
                &lt;td&gt;
                    &#35;{book}
                    &lt;span class="valid" msg="请填写内容" to="book"&gt;&lt;/span&gt;
                &lt;/td&gt;
            &lt;/tr&gt;
            
            
            &lt;tr&gt;
                &lt;td class="tdLeft"&gt;边框模式&lt;/td&gt;
                &lt;td&gt;            
                    &#35;{music}                
                    &lt;span class="valid" mode="border" to="music"&gt;&lt;/span&gt;
                &lt;/td&gt;
            &lt;/tr&gt;
            
        </pre>

    
    </div>
        
    
</div>

