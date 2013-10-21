<style>
input.width300 {width:450px;}
.tdL { text-align:right;width:60px;}

#titleTd {width:480px;}
#title {width: 460px}
#tagList {width:200px}
#srcLink {width:250px;}

.tabItem { cursor:pointer;}
.tabMain {padding:10px 0px;}

td.pubLeft { text-align:right; width:80px; background:#eceff5; font-weight:bold; vertical-align:top;}

.postSection label {margin-right:10px;}
label.clickLabel { background:#ffe45c; }

.tabList {border-bottom: 1px solid #B8D5FF;}
</style>

<script>
    _run(function () {

        $('.tabList li a').click(function () {
            var tabId = wojilu.str.trimStart($(this).parent().attr('id'), 'tab');
            $('.tabMain').hide();
            $('#content' + tabId).show();
        });

        wojilu.ui.valid();

        $('#pollOptionAdd').click(function () {
            var optionCount = parseInt($('#optionCount').text()) + 1;
            if (optionCount > 20) { alert('_{exPollMaxTwoOptions}'); return; }

            $(this).parent().parent().before(' <tr><td class="tdL">_{option}' + optionCount + '</td><td><input name="Answer" type="text" class="width300" maxlength="50" /></td></tr>');
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

<div style="padding-top:10px;height:450px;width:650px;">

	<form action="#{ActionLink}" method="post" id="pollPollForm" class="form-horizontal">

    <div style="margin:0px 10px;">
	<ul class="tabList">
	<li id="tab1" class="firstTab currentTab" style="margin-left:30px; width:130px;"><a href="#">:{general}</a><span></span></li>
	<li id="tab2" style="width:130px;"><a href="#">:{advanced}</a><span></span></li>
    <li id="tab3" style="width:130px;"><a href="#">SEO</a><span></span></li>
	</ul>




	<div id="content1" class="tabMain" style="width:98%;">
	<table style="width: 99%;margin:auto;" border="0">

        <tr>
            <td class="tdL">_{pollTitle}</td>
            <td><input name="Title" style="width: 450px" type="text" ><span class="valid" mode="border" to="Title"></span></td>    
        </tr>

        <tr>
            <td class="tdL">_{pollType}</td>
            <td><label class="radio inline">
                <input id="ptype1" name="PollType" type="radio" value="0" #{singleCheck} /> _{singleChoice}</label>
                <label class="radio inline">
                <input id="ptype2" name="PollType" type="radio" value="1" #{multiCheck} /> _{multiChoice}</label>
            </td>
        </tr>

        <tr>
            <td class="tdL" style=" vertical-align:top;">_{pollNote}<br /><span class="label label-info note">(_{optional})</span></td>
            <td><div  style="width:520px;">
<script type="text/plain" id="Question" name="Question"></script>
<script>
    _run(function () {
        wojilu.editor.bind('Question').height(80).line(1).show();
    });
</script>
            
            </div></td>
        </tr>

        <tr>
            <td class="tdL">_{option}1</td>
            <td><input name="Answer" type="text" class="width300" maxlength="50" /></td>
        </tr>

        <tr>
            <td class="tdL">_{option}2</td>
            <td><input name="Answer" type="text" class="width300" maxlength="50" /></td>
        </tr>

        <tr>
            <td class="tdL">_{option}3</td>
            <td><input name="Answer" type="text" class="width300" maxlength="50" /></td>
        </tr>

        <tr>
            <td class="tdL">_{option}4</td>
            <td><input name="Answer" type="text" class="width300" maxlength="50" /></td>
        </tr>

        <tr>
            <td></td>
            <td style="padding-left:5px;">
                    <span class="btn btn-mini" id="pollOptionAdd">+ _{addOneOption}</span>
                    <span class="btn btn-mini left10" id="pollOptionRemove">- _{deleteOneOption}</span>
            </td>
        </tr>

        <tr>
            <td></td>
            <td style="padding-top:10px;">
            
                <button name="btnSubmit" type="submit" class="btn btn-primary">
                    <img src="~img/poll.gif">
                    _{pubPoll}
                </button>


                <input name="appId" type="hidden" value="#{appId}" />
                <div class="hide" id="optionCount">4</div>

            </td>
        </tr>

        </table>
    </div>


	<div id="content2" class="tabMain" style="display:none;width:98%;">
	<table style="margin:auto;" class="pubTable">

        <tr>
            <td class="pubLeft">_{effectiveDay}</td>
            <td>
                <input name="Days" type="text" value="0" style="width:30px;" />
                <span class="help-inline note">_{dayTip}</span>
            </td>
        </tr>

        <tr">
            <td class="pubLeft">_{otherRequirements}</td>
            <td>
                <label class="checkbox inline">
                    <input name="IsVisible" id="pIsVisible" type="checkbox" /> _{isVisible}
                </label>
                <label class="checkbox inline">
                    <input name="OpenVoter" id="pOpenVoter" type="checkbox" />_{isVoterOpen}
                </label>
            </td>
        </tr>

        <tr><td></td><td>&nbsp;</td></tr>
        <tr>
            <td class="pubLeft">查看次数</td>
            <td><input name="Hits" type="text" style="width:30px" value="0" /></td>
        </tr>
	    <tr><td class="pubLeft">_{closeComment}</td><td>
        
        <label class="checkbox"><input type="checkbox" name="IsCloseComment" value="1"> 关闭评论</label>   	    
	    </td></tr>

        <tr><td></td><td>&nbsp;</td></tr>
	    <tr>
	        <td class="pubLeft">_{tag}</td>
	        <td><input name="TagList" id="tagList" type="text" style="width:300px;"><span class="note left5" title="">_{tagTip}</span></td>
	    </tr>

		<tr >
			<td class="pubLeft">_{summary}</td>
			<td>
			<textarea name="Summary" style="width: 450px; height: 90px"></textarea></td>
		</tr>
		
	</table>
	</div>

    <div id="content3" class="tabMain" style="display:none;width:98%;">
	<table style="margin:auto;" class="pubTable">

	    <tr>
	        <td class="pubLeft">SEO<br />keywords</td>
	        <td><input name="MetaKeywords" type="text" style="width:450px;"><br />
	        <span class="note">如果不填写，则使用tag</span>
	        </td>
	    </tr>

		<tr >
			<td class="pubLeft">SEO<br />description</td>
			<td>
			<textarea name="MetaDescription" style="width: 450px; height: 90px"></textarea><br />
			<span class="note">如果不填写，则使用摘要</span>
			</td>
		</tr>
        </table>
    </div>
	
    </div>
	</form>

</div>

