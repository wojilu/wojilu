<div style="padding:20px; background:#fff;">

<style>
.formTitle {font-weight:bold; padding-left:80px;}
.tdRule {width:90px;}
.tdLeft { vertical-align:top;width:60px; text-align:right}
</style>
    <div></div>

    <div class="formPanel">
    <form method="post" action="#{savePostLink}">    
        <table style="width:99%;">
            <tr><td>规则说明</td><td colspan="2" class="formTitle">内置验证规则</td></tr>
            
                
            <tr><td class="tdRule">rule="email"</td><td class="tdLeft">请输入</td><td>
                <input name="myTitle3" type="text" /> <span class="valid" rule="email" msg="请输入email"></span></td></tr>
                
            <tr><td class="tdRule">rule="int"</td><td class="tdLeft">请输入</td><td>
                <input name="myTitle4" type="text" /> <span class="valid" rule="int" msg="请输入正整数int"></span></td></tr>
                
            <tr><td class="tdRule">rule="tel"</td><td class="tdLeft">请输入</td><td>
                <input name="myTitle5" type="text" /> <span class="valid" rule="tel" msg="请输入电话tel"></span></td></tr>
                
            <tr><td class="tdRule">rule="mobile"</td><td class="tdLeft">请输入</td><td>
                <input name="myTitle6" type="text" /> <span class="valid" rule="mobile" msg="请输入手机mobile"></span></td></tr>
                
            <tr><td class="tdRule">rule="url"</td><td class="tdLeft">请输入</td><td>
                <input name="myTitle7" type="text" /> <span class="valid" rule="url" msg="请输入网址url，必须以http开头"></span></td></tr>
                
            <tr><td class="tdRule">rule="zip"</td><td class="tdLeft">请输入</td><td>
                <input name="myTitle8" type="text" /> <span class="valid" rule="zip" msg="请输入邮政编码zip"></span></td></tr>
                
            <tr><td class="tdRule">rule="qq"</td><td class="tdLeft">请输入</td><td>
                <input name="myTitle9" type="text" /> <span class="valid" rule="qq" msg="请输入qq号码"></span></td></tr>

            <tr><td>&nbsp;</td><td></td><td><input id="Submit1" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>


    
</div>

