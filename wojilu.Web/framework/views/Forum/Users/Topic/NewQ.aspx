
#{location}

<script>
_run(function () {
    wojilu.ui.valid();
    wojilu.ui.ajaxPostForm();
});
</script>

<div class="row">
<div class="span12">
<div class="forum-form-container forum-form-normal-container" style="padding-top:10px;">

	<form action="#{ActionLink}" method="post" class="ajaxPostForm form-horizontal forum-form-normal">

        <div class="control-group">
            <label class="control-label">_{title}</label>
            <div class="controls">
                #{Category}
                <input name="Title" style="width: 420px" type="text" value="#{Title}" />
            </div>
            <div class="hide"><span class="valid" mode="border" to="Title"></span> </div>
        </div>

        <div class="control-group">
            <label class="control-label">:{rewardValue}</label>
            <div class="controls">
                <select name="Reward" style="width:130px;">
				<option value="5">5</option>
				<option value="10">10</option>
				<option selected="selected" value="20">20</option>
				<option value="50">50</option>
				<option value="80">80</option>
				<option value="100">100</option>
				<option value="200">200</option>
				</select>
                <span class="help-inline strong">#{currency.Unit}</span>
                <span class="help-inline">:{rewardTip}</span>
            </div>
        </div>


        <div class="control-group">
            <label class="control-label">_{tag}</label>
            <div class="controls">
                <input name="TagList" style="width: 120px;" type="text" value="#{TagList}" />
                <span class="help-inline">_{tagTip}</span>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label">:{qNote}</label>
            <div class="controls"><div style="width:90%;">
<script type="text/plain" id="Content" name="Content">
    #{Content}
</script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(350).line(1).show();
    });
</script>
            </div></div>
            <div class="hide"><span class="valid" to="Content" mode="border"></span></div>
        </div>

        <div class="control-group">
            <label class="control-label">&nbsp;</label>
            <div class="controls">
                <button type="submit" name="btnSubmit" class="btn btn-primary">
                    <i class="icon-pencil icon-white"></i>
                    :{submitPost}
                </button>

                <img src="~img/s/upload.png" style="margin-left:50px;" />
                <a href="#{uploadLink}" class="frmBox strong" xwidth="500" xheight="200" title=":{uploadAttachment}">:{uploadAttachment}</a>
                
            </div>
        </div>

        <div class="control-group">
            <label class="control-label">&nbsp;</label>
            <div class="controls" id="uploadFiles">
                <input id="uploadFileIds" name="uploadFileIds" type="hidden" />
            </div>
        </div>

	</form>

</div>
</div>
</div>
