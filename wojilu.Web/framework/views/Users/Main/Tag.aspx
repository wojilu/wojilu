<style>

.userItem:hover {background:#f2f2f2;}
.userItem td { vertical-align:top; padding:3px;}
.userItem img { padding:3px; border:1px #ccc solid;}

</style>


<div style="background:#f2f2f2; padding:20px 20px 5px 20px;"><form method="get" action="#{ActionLink}" class="form-inline">
<div>
    <input id="Text1" name="tagName" type="text" value="#{tagName}" />
    <button id="Submit1" type="submit" class="btn btn-primary"><i class="icon-search icon-white"></i> 搜索标签</button>
</div>
</form></div>


<div style="margin:30px 0px 20px 0px;">

<table style="width:100%;" cellpadding="0" cellspacing="0">
<!-- BEGIN users -->
    <tr class="userItem">
        <td style="width:60px;"><a href="#{user.Link}"><img src="#{user.PicSmall}" class="avs"  /></a></td>
        <td>
            <div><span style="font-weight:bold; font-size:14px;">#{user.Name}</span>
                <a href="#{user.Link}" class="left10">空间</a>
                <a href="#{user.LinkT}">微博</a>
            </div>
            <div>简介: #{user.Description}</div>
        </td>
        <td style="width:90px; padding-left:10px; text-align:right;">
            <a href="#{user.LinkT}" class="btn addFollow"><i class="icon-plus"></i> 加关注</a>
        </td>
    </tr>
    <tr><td colspan="3" style="height:10px;"></td></tr>
<!-- END users -->
</table>

<div style="margin-top:15px;">#{page}</div>

</div>
