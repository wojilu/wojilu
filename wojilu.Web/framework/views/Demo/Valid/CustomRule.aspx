<div style="padding:20px; background:#fff;">

<style>

.tdRule {width:90px;}
.tdLeft { vertical-align:top;width:60px; text-align:right}
</style>



    <div class="formPanel">
        <h2 style=" margin-bottom:20px;">自定义规则验证</h2>


    <form method="post" action="#{savePostLink}">    
        <table class="table">
            
            <tr><td class="tdRule">rule="^.{5,}$"</td><td>
                <input name="myTitle1" type="text" /> <span class="valid" rule="^.{5,}$" msg="至少5个字符"></span></td></tr>
                
            <tr><td class="tdRule">rule="^.{3,8}$"</td><td>
                <input name="myTitle2" type="text" /> <span class="valid" rule="^.{3,8}$" msg="3-8个字符之间"></span></td></tr>
                

            <tr><td>&nbsp;</td><td><input id="Submit1" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>


    
</div>

