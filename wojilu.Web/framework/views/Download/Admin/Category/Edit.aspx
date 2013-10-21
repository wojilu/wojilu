<div>

	<div style="margin:20px;">
		
		
<form method="post" action="#{ActionLink}" class="">

    <table style="width:300px; margin:10px auto;">
        <tr><td style=" text-align:right;">_{name}: </td><td><input name="Name" type="text" value="#{Name}" /></td></tr>
        <tr><td></td><td><input name="IsThumbView" type="checkbox" #{checked} />按缩略图模式浏览</td></tr>
        <tr><td></td><td><div style="margin-top:10px;"><input id="Submit2" type="submit" value="_{submit}"  class="btn" /></div></td></tr>
    
    </table>    

</form>


	</div>
</div>