<form method="post" style="margin:30px;">
<div>

    <textarea id="txtMsg" name="txtMsg" style="width:500px; height:80px;"></textarea>
    <span class="valid" to="txtMsg" mode="border" rule="^.{3,}$" type="textarea"></span>
</div>

<div>
    <input id="Submit1" type="submit" value="submit" /></div>


</form>

<script>
_run( function() {
    wojilu.ui.valid();
});
</script>