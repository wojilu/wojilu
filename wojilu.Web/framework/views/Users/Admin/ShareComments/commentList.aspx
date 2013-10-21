
<div style="margin:5px 5px 5px 10px; " id="shareComments#{share.Id}">

<!-- BEGIN comments -->                 	
<table style="width: 95%; margin:5px 0px 5px 10px;background:#ebf3f7; margin-left:#{c.Indent}px;">
    <tr>
        <td style="width:38px; vertical-align:top;"><a href="#{c.UserLink}"><img src="#{c.UserPic}" style="width:32px;"/></a></td>
        <td style="vertical-align:top;">
            <div><a href="#{c.UserLink}">#{c.UserName}</a> <span class="note">_{postedAt} #{c.Created}</span></div>
            <div style="margin-top:5px;" id="commentContent#{c.Id}">#{c.Content}
            <span class="commentCmd link left5" parentId="#{c.Id}" rootId="#{c.RootId}">_{reply}</span>
            </div>
        </td>
    </tr>
</table>                    	
<!-- END comments -->

</div>    
