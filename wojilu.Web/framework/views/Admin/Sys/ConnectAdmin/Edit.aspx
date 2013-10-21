<div style="width:550px; height:200px;">

<style>
.tdLeft { vertical-align:top; width:90px;}
</style>

<div style="margin:20px;">
<form method="post" action="#{ActionLink}">


    <table style="width:100%;">

        <tr>
            <td class="tdLeft">名称</td>
            <td><input name="x.Name" type="text" value="#{x.Name}" style="width:150px;" /></td>
        </tr>
        <tr>
            <td class="tdLeft">登录名称</td>
            <td><input name="x.LoginName" type="text" value="#{x.LoginName}" style="width:150px;" /></td>
        </tr>
        <tr>
            <td class="tdLeft">TypeFullName</td>
            <td><input name="x.TypeFullName" type="text" value="#{x.TypeFullName}" style="width:250px;" /></td>
        </tr>
        <tr>
            <td class="tdLeft">ConsumerKey</td>
            <td><input name="x.ConsumerKey" type="text" value="#{x.ConsumerKey}" style="width:250px;" /></td>
        </tr>
        <tr>
            <td class="tdLeft">ConsumerSecret</td>
            <td><input name="x.ConsumerSecret" type="text" value="#{x.ConsumerSecret}" style="width:380px;" /></td>
        </tr>

        <tr>
            <td>&nbsp;</td>
            <td>
                <input id="Submit1" type="submit" value="_{submit}" class="btn" />
                <input id="Button1" type="button" value="_{cancel}" class="btnCancel" />
            </td>
        </tr>

    </table>
</form>
</div>
</div>