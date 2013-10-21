<div style="padding:20px;">

<style>
.tdL {width:70px; background:#f2f2f2; text-align:right; font-weight:bold;}
</style>

<form method="post" action="#{ActionLink}">


<table cellpadding="5" style="border:0px #ccc solid; width:98%; margin:auto;">
    <tr><td class="tdL">名称</td><td><input name="Name" type="text" value="#{installer.Name}" /></td></tr>
    <tr><td class="tdL">简介</td><td><input name="Description" type="text" style="width:320px;" value="#{installer.Description}" /></td></tr>
    <tr><td class="tdL">是否启用</td><td>
        <div>
            <span class="right10" style="color:#666;">在</span>
            <span style="color:#000; font-weight:bold;">#{appCheckboxList}</span>
            <span class="left10" style="color:#666;">中启用本程序</span>
        </div>
    </td></tr>
    <tr><td class="tdL">关闭模式</td><td>如果关闭app，那么<br />#{closeMode}</td></tr>
    <tr><td></td><td style="padding:10px;">
    <input id="Submit1" type="submit" value="_{submit}" class="btn" />
    <input id="Button1" type="button" value="_{cancel}" class="btnCancel" />
    
    
    </td></tr>

</table>


</form>

</div>