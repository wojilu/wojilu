
<div style="width:500px;height:260px;margin:15px;">


	<form method="post" action="#{ActionLink}">
	<div>_{shareNote}:</div>
	<textarea name="Comment" style="width:93%;height:80px;"></textarea>
	<div><input name="Submit1" type="submit" value="_{submit}" class="btn" /></div>
	<input name="name" type="hidden" value="#{name}" />
	<input name="dataType" type="hidden" value="#{dataType}" />
	<input name="dataLink" type="hidden" value="#{dataLink}" />	
	</form>
	
	<div style="margin:10px 10px 10px 0px;padding:10px;background:#f2f2f2; width:90%;">#{shareContent}</div>
</div>