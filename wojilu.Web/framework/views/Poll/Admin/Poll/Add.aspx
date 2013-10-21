<style>
input.width300 {width:450px;}
</style>


<script type="text/javascript">
<!--
_run( function(){

    //wojilu.ui.valid();

	$("#pollViewButton").click( function() {
		$( ".onePollResultNotShow " ).css( "display", "block" );
	});

	$( '#pollOptionAdd' ).click( function() {
		var optionCount = parseInt( $( '#optionCount' ).text() ) + 1;
		if( optionCount>20 ) { alert( '_{exPollMaxTwoOptions}' );return;}
		$(this).parent().before( '<div>_{option}'+optionCount+'：<input name="Answer" type="text" class="width300" maxlength="50" /></div>' );
		$( '#optionCount' ).text(optionCount)
	});

	$( '#pollOptionRemove' ).click( function() {
		var optionCount = parseInt( $( '#optionCount' ).text() );
		if( optionCount<=2 ) { alert( '_{exPollMinTwoOptions}' );return;}
		var optionAll = $(this).parent().siblings();
		$(optionAll[ optionAll.length-1 ]).remove();
		optionCount  -= 1;
		$( '#optionCount' ).text(optionCount);
	});
    

});	

//-->
</script>


<div class="forumForm addPollPanel">

	<form action="#{ActionLink}" method="post" id="forumPollForm">
	<table class="forumFormMainTable" style="margin:20px 10px 50px 30px" >
		<tr>
			<td class="tdL"><strong>_{pollTitle}</strong></td>
			<td>
			<input name="Title" style="width: 450px" type="text" ><span class="valid" mode="border"></span></td>
		</tr>
		<tr>
			<td class="tdL" valign="top">_{pollNote}<br>
			<span class="note">(_{optional})</span></td>
			<td>
            
<script type="text/plain" id="Question" name="Question"></script>
<script>
    _run(function () {
        wojilu.editor.bind('Question').height(80).line(1).show();
    });
</script>

            </td>
		</tr>
		<tr>
			<td valign="top" colspan="2" style=" line-height:200%; padding:10px 0px 10px 13px"><div style="display:none;" id="optionCount">#{optionCount}</div>
			<div>_{option}1：<input name="Answer" type="text" class="width300" maxlength="50"/><span class="valid" mode="border"></span></div>
			<div>_{option}2：<input name="Answer" type="text" class="width300" maxlength="50" /><span class="valid" mode="border"></span></div>
			<div>_{option}3：<input name="Answer" type="text" class="width300" maxlength="50" /></div>
			<div>_{option}4：<input name="Answer" type="text" class="width300" maxlength="50" /></div>
			<div>_{option}5：<input name="Answer" type="text" class="width300" maxlength="50" /></div>
			<div style="margin:5px 50px;"><span class="font12 cmd" id="pollOptionAdd">+ _{addOneOption}</span>
			<span class="left10 font12 cmd" id="pollOptionRemove">- _{deleteOneOption}</span></div>
			</td>
		</tr>
		
		<tr>
			<td class="tdL"><strong>_{pollType}</strong></td>
			<td><input id="ptype1" name="PollType" type="radio" value="0" checked="checked" /><label for="ptype1">_{singleChoice}</label> <input id
			="ptype2" name="PollType" type="radio" class="left10" value="1" /><label for="ptype2">_{multiChoice}</label>
			</td>
		</tr>
		<tr>
			<td colspan="2" style="padding: 5px 10px 30px 80px;">
			<input name="btnSubmit" type="submit" value="_{pubPoll}" class="btn"></td>
		</tr>
		<tr>
			<td class="tdL"><strong>_{effectiveDay}</strong></td>
			<td>
			<input name="Days" style="width: 30px" type="text" value="0">  <span class="red font12">_{dayTip}</span></td>
		</tr>
		<tr class="hide">
			<td class="tdL" valign="top"><strong>_{otherRequirements}</strong></td>
			<td><input name="IsVisible" id="pIsVisible" type="checkbox"><label for="pIsVisible">_{isVisible}</label> 
			<input class="left20" name="OpenVoter" id="pOpenVoter" type="checkbox" /><label for="pOpenVoter">_{isVoterOpen}</label></span>
			</td>
		</tr>
	</table>
	</form>

</div>




