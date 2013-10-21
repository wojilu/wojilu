<form action="#{ActionLink}" method="post">

<table style="width:80%; margin:20px auto;">

<tr>
<td colspan="2">
<div class="warning" style="width:100%; margin-bottom:20px;">说明：<strong>将当前的布局和样式，导出为主题，方便下次重复使用。</strong>所谓主题，就是一个设计好的界面，包括样式、布局和初始化数据。</div>

</td>

</tr>


<tr>
<td style="width:60px;">主题名称</td><td><input type="text" name="Name" style="width:200px;" placeholder="请填写名称" />
<span class="valid border"></span>
 </td>
</tr>

<tr>
<td>简介</td><td><textarea name="Description" style="width:98%; height:60px;"></textarea></td>
</tr>


<tr>
<td></td><td>
<input type="submit" value="_{submit}" class="btn" />
<input type="button" class="btnCancel btn" value="_{cancel}" />

</td>
</tr>

</table>

</form>