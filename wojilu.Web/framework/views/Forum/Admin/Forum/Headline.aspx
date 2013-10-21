
	
<div class="" style="height:450px;">

	<form method="post" action="#{ActionLink}" class="ajaxPostForm">
	<table style="width: 98%; margin:5px auto;" >
		<tr>
			<td>
<script type="text/plain" id="Notice" name="Notice">#{Notice}</script>
<script>
_run(function () {
    wojilu.editor.bind('Notice').height(380).line(1).show();
});
</script>

			</td>
		</tr>
		<tr>
			<td>
			<input type="submit" value="_{submit}"  class="btn" />
			</td>
		</tr>
	</table>
	</form>

</div>


