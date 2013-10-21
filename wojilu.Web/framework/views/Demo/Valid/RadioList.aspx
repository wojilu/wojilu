<div style="padding:20px; background:#fff;">

<style>
.formTitle {font-weight:bold; padding-left:80px;}
.tdLeft { vertical-align:top;width:80px;}

.radio {width:30px;}
</style>
    <div></div>

    <div class="formPanel">
    <form method="post" action="#{savePostLink}">    
        <table  class="table">
            <tr><td colspan="2" class="formTitle">单选框验证</td></tr>
            <tr><td class="tdLeft">普通模式</td><td>
            #{book}            
            <span class="valid" msg="请填写内容" to="book"></span>
            </div>
            </td></tr>
            <tr><td></td><td><input id="Submit1" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>

    <div class="formPanel">
    <form method="post" action="#{savePostLink}">    
        <table  class="table">
            <tr><td colspan="2" class="formTitle">单选框验证</td></tr>
            <tr><td class="tdLeft">边框模式</td><td>            
            <div style="margin:5px 10px; width:300px; padding:2px 10px 5px 10px;">
            #{music}            
            <span class="valid border" to="music"></span>
            </div>
            </td></tr>
            <tr><td></td><td><input id="Submit2" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>
        
        
    <div style="border-bottom:1px #ccc solid;margin:0px 0px 20px 0px;">&nbsp;</div>
    
    <div>
    <div class="strong">服务端代码</div>
    <pre class="brush: c-sharp;">

    public void RadioList() {

        Dictionary&lt;string, string&gt; book = new Dictionary&lt;string, string&gt;();
        book.Add( "小说", "1" );
        book.Add( "诗歌", "2" );
        book.Add( "散文", "3" );
        book.Add( "戏剧", "4" );

        radioList( "book", book, null );

        Dictionary&lt;string, string&gt; music = new Dictionary&lt;string, string&gt;();
        music.Add( "民谣", "1" );
        music.Add( "流行", "2" );
        music.Add( "摇滚", "3" );
        music.Add( "电子", "4" );

        radioList( "music", music, null );
    }
    </pre>
    </div>
        
    
</div>

