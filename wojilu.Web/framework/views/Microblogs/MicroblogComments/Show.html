
<div id="commentsWrap" style="margin-left:20px; background:#f2f2f2; border:1px #ccc solid;">

<style type="text/css">
body { background:#fff;} 

.commentCmd {cursor:pointer; padding:2px 6px 1px 6px; }
#commentForm table td { padding:5px;}
.commentItem { margin:5px 0px 0px 20px;}

.mblogOne {border-bottom:1px #ccc dotted; padding-top:10xp; }
.mblogOne table {width:100%;margin:10px 0px 5px 0px;}
.mblogOne table img {width2:32px;}
.mblogOne table td {vertical-align:top;}
</style>


    <div id="commentForm" style="margin:5px 10px 5px 0px;">
    <form method="post" action="#{ActionLink}" class="ajaxUpdateForm">
    <table style="width:100%;">
        <tr>
            <td style="vertical-align:top; text-align:center;width:60px;"><img src="#{viewer.PicSmall}" /></td>
            <td style="text-align:left;">
                <div><textarea name="content" style="width:100%; height:40px;"></textarea><span class="valid hide" mode="border"></span></div>
                <div style="margin-top:5px;">
                <label><input id="Checkbox1" name="isRepost" type="checkbox" />同时转发到我的微博</label>
                <input type="submit" value="_{comment}" class="btn btns" id="btnComment" /> 
                <input name="rootId" type="hidden" value="#{c.RootId}" />
                <input name="parentId" type="hidden" value="#{c.ParentId}" />
            </td>
        </tr>
    </table>
    </form>
    </div>

    
	<div style="margin:5px 10px 5px 10px; padding-bottom:10px;">
	
	    <div id="blogComments#{blog.Id}"></div>
	    
        <!-- BEGIN comments -->
        <div class="mblogOne">
        <table class="mblogOneTbl">
            <tr>
                <td style="width:60px;"><a href="#{user.Link}" target="_blank"><img src="#{user.Face}" /></a></td>
                <td>
                <div>
                    <a href="#{user.Link}" target="_blank">#{user.Name}</a> 
                    <span class="note">_{postedAt} #{comment.Created}</span>                    
                </div>
                <div style="margin-top:5px; color:#666;" id="commentContent#{comment.Id}">
                    <div style="float:left;">#{comment.Content}</div>
                    <div class="commentCmd link left5" style="float:right;" parentId="#{comment.Id}" rootId="#{comment.RootId}">_{reply}</div>
                    <div class="clear"></div>
                </div>
                </td>
            </tr>
        </table>
        </div>
        <!-- END comments -->
        <div style="margin:15px 10px 10px 0px; text-align:right;">#{moreLink}</div>
    </div>    
<script>

_run( function() {

    $('.commentCmd').click( function() {
        var form = $('#commentForm');
        $(this).after( form );
        form.show();
        $('textarea[name=content]', form).val('');
        $('input[name=rootId]', form).val( $(this).attr( 'rootId') );
        $('input[name=parentId]', form).val( $(this).attr( 'parentId') );
    });
    
    $('.btnOther').click( function() {
        $('#commentForm').hide();
    });
    
    function resizeParent() {

        var clist =$('#commentList#{parentPanelId}', window.parent.document );
        var frm = $('iframe', clist);
        frm.height( frm.height()+60 );
        
    };
    
    wojilu.ui.ajaxUpdateForm( resizeParent );

    $('#moreLink').click( function() {
        var url = $(this).attr( 'href' );
        $(this).attr( 'href', $(this).attr( 'to' ) );
        return true;
    });
    
});
</script>
    
</div>