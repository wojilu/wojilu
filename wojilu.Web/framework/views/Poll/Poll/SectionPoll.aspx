
<div style="margin:3px;" id="pollItem#{poll.Id}">

    <div class="pollTitle strong" style="margin:5px 0px 5px 0px;">#{poll.Title}</div>
    <div>
        <span class="note">#{poll.Created}</span>
        <span class="note">#{poll.Replies}</span>
        <a href="#{poll.ShowLink}" class="left10">_{detail}</a>
    </div>
    <div class="pollQuestion" style="margin:5px;font-size:12px;color:#666;">#{poll.Question}</div>

    <div class="pollMain" style="margin:0px;font-size:12px;">
        <div id="pollContainer">
            <table class="pollOptions" cellpadding="0" cellspacing="0" style="width:95%;margin-left:5px;">
            <!-- BEGIN options -->
            <tr>
                <td class="optionItemString">#{op.SelectControl} #{op.Text}</td>
            </tr><!-- END options -->
            </table>

            <div class="note">
                _{pollCount}: #{poll.Voters}。#{poll.ExpiryInfo}
            </div>

            <!-- BEGIN cmdVote -->
            <div class="pollSubmit">                
                <input id="btnPoll#{poll.Id}" class="btn" type="button" value="_{pollVote}"  />
                <span id="lnkVote" class="frmBox" title="投票" href="#"></span>
<script>

_run( function() {

    $('#btnPoll#{poll.Id}').click( function() {
        var ctx = $( '#pollItem' + #{poll.Id} );
        
        var selectedControl = $( 'input[name=pollOption]:checked', ctx );
        var pValue = selectedControl.serialize();
        if( wojilu.str.hasText( pValue )==false ) {
            alert( '_{pollSelectRequire}' );
            return false;
        }
        
        $(this).attr( 'disabled', 'disabled' );
        $(this).val( '_{pollVoted}' );
        var voteLink = '#{poll.VoteLink}';
        var url = voteLink+'?'+pValue;
        
        $('#lnkVote').attr( 'href', url ).click();
    });

});

</script>
            </div><!-- END cmdVote -->
            
            <!-- BEGIN plsVote --><div style="margin:0px 0px 10px 0px; color:#666;">_{pollLoginRequire}</div><!-- END plsVote -->
            
        </div>
    </div>
</div>





