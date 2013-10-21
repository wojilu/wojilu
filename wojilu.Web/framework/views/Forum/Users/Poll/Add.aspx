<style>
input.width300 {width:450px;}
</style>

<script>
    _run(function () {

        wojilu.ui.valid();
        wojilu.ui.ajaxPostForm();

        $('#pollOptionAdd').click(function () {
            var optionCount = parseInt($('#optionCount').text()) + 1;
            if (optionCount > 20) { alert('_{exPollMaxTwoOptions}'); return; }
            $(this).parent().parent().before('<div class="control-group"><label class="control-label">_{option}' + optionCount + '</label><div class="controls"><input name="Answer" type="text" class="width300" maxlength="50" /></div></div>');
            $('#optionCount').text(optionCount);
        });

        $('#pollOptionRemove').click(function () {
            var optionCount = parseInt($('#optionCount').text());
            if (optionCount <= 2) { alert('_{exPollMinTwoOptions}'); return; }

            $(this).parent().parent().prev().remove();

            optionCount -= 1;
            $('#optionCount').text(optionCount);
        });

        $('#btnMore').click(function () {
            $('#moreOption').toggle();
        });


    });	

</script>

#{location}

<div class="row">
<div class="span12">
<div class="forum-form-container" style="padding-top:10px;">

	<form action="#{ActionLink}" method="post" id="forumPollForm" class="ajaxPostForm form-horizontal forum-form-normal">

        <div class="control-group">
            <label class="control-label">_{pollTitle}</label>
            <div class="controls">
                <input name="Title" style="width: 450px" type="text" >
            </div>
            <div class="hide"><span class="valid" mode="border" to="Title"></span></div>
        </div>

        <div class="control-group">
            <label class="control-label">_{pollType}</label>
            <div class="controls">
                <label class="radio inline" style="width:30px;">
                    <input id="ptype1" name="PollType" type="radio" value="0" #{singleCheck} /> _{singleChoice}
                </label>

                <label class="radio inline" style="width:30px;">
                    <input id="ptype2" name="PollType" type="radio" value="1" #{multiCheck} /> _{multiChoice}
                </label>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label">_{pollNote}<br /><span class="label label-info">_{optional}</span></label>
            <div class="controls"><div  style="width:90%;">
<script type="text/plain" id="Question" name="Question"></script>
<script>
    _run(function () {
        wojilu.editor.bind('Question').height(100).line(1).show();
    });
</script>
            
            </div></div>
        </div>

        <div class="hide" id="optionCount">#{optionCount}</div>

        <div class="control-group">
            <label class="control-label">_{option}1</label>
            <div class="controls"><input name="Answer" type="text" class="width300" maxlength="50" /></div>
        </div>
        <div class="control-group">
            <label class="control-label">_{option}2</label>
            <div class="controls"><input name="Answer" type="text" class="width300" maxlength="50" /></div>
        </div>
        <div class="control-group">
            <label class="control-label">_{option}3</label>
            <div class="controls"><input name="Answer" type="text" class="width300" maxlength="50" /></div>
        </div>
        <div class="control-group">
            <label class="control-label">_{option}4</label>
            <div class="controls"><input name="Answer" type="text" class="width300" maxlength="50" /></div>
        </div>
        <div class="control-group">
            <label class="control-label">_{option}5</label>
            <div class="controls"><input name="Answer" type="text" class="width300" maxlength="50" /></div>
        </div>

        <div class="control-group">
            <label class="control-label"></label>
            <div class="controls">
                <span class="btn btn-mini" id="pollOptionAdd"><i class="icon-plus-sign"></i> _{addOneOption}</span>
                <span class="btn btn-mini left10" id="pollOptionRemove"><i class="icon-minus-sign"></i> _{deleteOneOption}</span>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label"></label>
            <div class="controls">

                <button name="btnSubmit" type="submit" class="btn btn-primary">
                    <i class="icon-signal icon-white"></i>
                    _{pubPoll}
                </button>

                <span id="btnMore" class="btn btn-mini left20"><i class="icon-hand-right"></i> 其他设置...</span>

                <input name="forumId" type="hidden" value="#{forumId}" />
            </div>
        </div>

        <div id="moreOption" style="display:none;">

            <div class="control-group">
                <label class="control-label">_{effectiveDay}</label>
                <div class="controls">
                    <input name="Days" type="text" value="0" style="width:30px;" />
                    <span class="help-inline">_{dayTip}</span>
                </div>
            </div>

            <div class="control-group">
                <label class="control-label">_{otherRequirements}</label>
                <div class="controls">
                    <label class="checkbox inline">
                        <input name="IsVisible" id="pIsVisible" type="checkbox" /> _{isVisible}
                    </label>
                    <label class="checkbox inline">
                        <input name="OpenVoter" id="pOpenVoter" type="checkbox" />_{isVoterOpen}
                    </label>
                </div>
            </div>

        </div>

	</form>

</div>
</div>
</div>
