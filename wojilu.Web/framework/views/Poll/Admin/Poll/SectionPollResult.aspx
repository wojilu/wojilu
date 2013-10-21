<div>

    <div class="pollTitle strong" style="margin:5px 0px 5px 0px;">#{poll.Title}</div>
    <div>
        <span class="note">#{poll.Created}</span>
        <span class="note">#{poll.Replies}</span>
        <a href="#{poll.ShowLink}" class="left10">_{detail}</a>
    </div>
    <div class="pollQuestion" style="margin:5px;font-size:12px;color:#666;">#{poll.Question}</div>

    <div class="pollMain" style="margin:0px;font-size:12px;">
        <div id="pollContainer">
            <table class="pollOptions" cellpadding="3" cellspacing="0" style="width:95%;margin-left:5px; border-collapse:collapse; border:1px solid #ccc;" border=1>
            <!-- BEGIN options -->
            <tr>
                <td class="optionItemString"><label for="pollOption#{op.Id}">#{op.Text}</label></td>
                <td id="onePollResult#{op.Id}" class="onePollResult">
                    <img src="~img/vote.gif" style="width:#{op.ImgWidth}px;height:9px;" />
                    <span class="pollOptionVotes">#{op.Percent}</span>
                </td>
            </tr><!-- END options -->
            </table>

            <div class="note" style="margin-top:10px;">
                _{pollVoted}_{pollCount}: <a href="#{poll.ResultLink}" class="frmBox">#{poll.Voters}</a> 。#{poll.ExpiryInfo}
            </div>
            
        </div>
    </div>
</div>