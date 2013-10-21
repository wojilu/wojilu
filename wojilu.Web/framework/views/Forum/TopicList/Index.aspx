#{forumPath}

 <div class="board-info">
    <div class="board-info-title clearfix">
        <div class="span10">
            <span class="board-info-name">#{forumBoard.Title}</span> 
            今日: <span class="board-info-num-today">#{forumBoard.TodayPosts}</span>
            <span class="board-info-sp">|</span>
            主题: <span class="board-info-num">#{forumBoard.Topics}</span>
            <span class="board-info-sp">|</span>
            帖子: <span class="board-info-num">#{forumBoard.Posts}</span>
            <span class="board-info-moderator">#{forumBoard.Moderator}</span>
        </div>
    </div>
    #{forumBoard.Notice}
 </div>
 <script>
     _run(function () {
         $('.board-info-num-today').each(function () {
             if (parseInt($(this).text()) > 0) $(this).css('color', 'red');
         });
     });
 </script>

#{toolbar}
<!-- BEGIN categoryPanel -->
<div class="board-cat">
<a href="#{boardLink}"><strong>_{all}</strong></a><!-- BEGIN categories --><a href="#{c.LinkShow}" style="color:#{c.NameColor}">#{c.Name}</a><!-- END categories -->
</div>
<!-- END categoryPanel -->

<table id="forumPostList" class="table table-condensed forum-topic-list">

	<tr class="forum-topic-list-header">
		<th style="width:5%">&nbsp;</th>
		<th style="width:55%; ">
            <div class="pull-left right10"><strong>_{title}</strong></div>

            <div class="pull-left"><a href="#{forumBoard.PollUrl}" class="left20 right10">_{poll}</a></div>
            <div class="pull-left forum-cmd-sp">|</div>

            <div class="pull-left"><a href="#{forumBoard.PickedUrl}" class="right10">:{picked}</a></div>
            <div class="pull-left forum-cmd-sp">|</div>

            <div class="pull-left dropdown right10">
                <a class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" href="#">
                    :{sort} <span class="caret"></span>
                </a>
			    <ul class="dropdown-menu" id="sortCmds"  style="min-width:100px;">
				    <li><a href="#{slinkReplied}"><i class="icon-th-list"></i> :{sortReplied}</a></li>
				    <li><a href="#{slinkCreated}"><i class="icon-th-list"></i> :{sortCreated}</a></li>
                    <li class="divider"></li>
				    <li><a href="#{slinkReplies}"><i class="icon-th-list"></i> :{sortReplies}</a></li>
				    <li><a href="#{slinkViews}"><i class="icon-th-list"></i> :{sortViews}</a></li>
			    </ul>
            </div>
            <div class="pull-left forum-cmd-sp">|</div>

            <div class="dropdown pull-left">
                <a class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" href="#">
                    :{timeFilter} <span class="caret"></span>
                </a>
			    <ul class="dropdown-menu" id="timeFilter" style="min-width:100px;">
				    <li><a href="#{slinkAll}"><i class="icon-hand-right"></i> :{tAll}</a></li>
                    <li class="divider"></li>
				    <li><a href="#{slinkDay}"><i class="icon-hand-right"></i> :{tDay}</a></li>
				    <li><a href="#{slinkDayTwo}"><i class="icon-hand-right"></i> :{tDay2}</a></li>
				    <li><a href="#{slinkWeek}"><i class="icon-hand-right"></i> :{tWeek}</a></li>
                    <li class="divider"></li>
				    <li><a href="#{slinkMonth}"><i class="icon-hand-right"></i> :{tMonth}</a></li>
				    <li><a href="#{slinkMonthThree}"><i class="icon-hand-right"></i> :{tMonth3}</a></li>
				    <li><a href="#{slinkMonthSix}"><i class="icon-hand-right"></i> :{tMonth6}</a></li>
			    </ul>
            </div> 
        
        </th>
		<th style="width:15%" class="center">_{author}</th>
		<th style="width:10%" class="center">_{reply}/_{view}</th>
		<th style="width:15%" class="center">:{lastPost}</th>
	</tr>
	
	<!-- BEGIN stickyList -->
	<tr class="psSticky">
		<td class="psStatus"><img src="~skin/apps/forum/#{stickyIconName}.gif" /></td>
		<td class="psTitle">
		<div><span class="chkAdmin">#{p.CheckBox}</span>
		<a href="#{p.Url}" style="#{p.TitleStyle}" target="_blank">#{p.Titile}</a> 
		<span style="vertical-align:middle">#{p.Attachments}</span> #{p.Pages}</div>
		</td>
		<td class="psAuthor"><a href="#{p.MemberUrl}">#{p.MemberName}</a><br/><span class="gray">#{p.CreateTime}</span></td>
		<td align="center"><span class="orange">#{p.ReplyCount}</span><span class="gray">/</span><span class="note">#{p.Hits}</span></td>
		<td class="psAuthor"><a href="#{p.LastReplyUrl}">#{p.LastReplyName}</a><br/><span class="gray">#{p.LastUpdate}</span></td>
	</tr>	
	<!-- END stickyList -->
	
	<tr class="psTopicType psTopicSep"><td colspan="5"><div>#{topicTypeName}</div></td></tr>
	
	<!-- BEGIN postlist -->
	<tr>
		<td class="psStatus"><img src="#{postStatusImage}"></td>
		<td class="psTitle">
		    <div><span class="chkAdmin">#{p.CheckBox}</span>#{p.TypeImg}#{p.Category} <a href="#{p.Url}" style="#{p.TitleStyle}" target="_blank">#{p.Titile}</a>
		    <span class="note">#{p.Reward}#{p.Price}#{p.ReadPermission}</span>
		    <span style="vertical-align:middle">#{p.Attachments}</span> #{p.Pages} #{p.LabelNew}
		    </div>
		</td>
		<td class="psAuthor"><a href="#{p.MemberUrl}">#{p.MemberName}</a><br/><span class="gray">#{p.CreateTime}</span></td>
		<td align="center"><span class="orange">#{p.ReplyCount}</span><span class="gray">/</span><span class="note">#{p.Hits}</span></td>
		<td class="psAuthor"><a href="#{p.LastReplyUrl}">#{p.LastReplyName}</a><br/><span class="gray">#{p.LastUpdate}</span></td>
	</tr>
	<!-- END postlist -->	
</table>

#{toolbarBottom}

<div id="adminToolbar" class="hide">#{adminToolbar}</div>

<!-- BEGIN form -->
<div class="row">
<div class="span12">
<div class="forum-form-container">

    <div class="row">
	    <div style="padding-left:40px;"><h3>:{addTopic}</h3></div>
    </div>
    <div class="row clearfix" style="margin-top:10px;">

        <div class="span2" id="pubUserPic" style=" text-align:left;margin-left:20px;padding-left:20px;margin-top:10px;width:110px;"></div>

        <div class="span9 pull-left1 forum-form-quick" style=" ">
        <form method="post" action="#{ActionLink}" class="form-horizontal ajaxPostForm" id="pubTopicForm">

            <div class="control-group">
                  <label class="control-label" for="txtTitle">_{title}</label>
                  <div class="controls">
                      #{Category}<input id="txtTitle" name="Title" type="text" placeholder="请填写标题" style="float:left;"> 
                      <div style="clear:both;"></div>
                  </div>
                  <div class="hide"><span class="valid" msg="_{exTitle}" to="Title" mode="border"></span></div>
            </div>

            <div class="control-group">
                  <label class="control-label">_{content}</label>
                  <div class="controls" style="width:610px;">
                        <script type="text/plain" id="Content" name="Content"></script>
                        <script>_run(function () {wojilu.editor.bind('Content').height(150).line(1).show();})</script>
                        <div class="valid border" to="Content"></div>
                  </div>
            </div>

            <div class="control-group">    
                <label class="control-label"></label>

                <div class="controls">
                    <button type="submit" class="btn btn-primary" name="btnSubmit" >
                        <i class="icon-pencil icon-white"></i> :{submitPost}
                    </button>
                </div>
            </div>
        </form>
        </div>


	    
    </div>


</div>
</div>
</div>
<!-- END form -->


<script>
_run( function() {
    wojilu.ui.valid();
    wojilu.ui.ajaxPostForm();
    
    var moderators = #{moderatorJson};
    
    var isModerator = function(name) {
        for( var i=0;i<moderators.length;i++ ) {
            if( moderators[i] == name ) return true;
        }
        return false;
    };
    
    var showMTool = function() {

        if( ctx.viewer.IsAdministrator || isModerator(ctx.viewer.obj.Name) ) {
            $('#adminToolbar').show();
            $('.chkAdmin').show();
        }
        
        if( ctx.viewer.IsAdministrator ) {
            $('#forumAdminGlobalSticky').show();
            $('#forumAdminGlobalStickyUndo').show();
            $('#forumAdminMove').show();
        }
        
        if( ctx.viewer.IsLogin==false ) {
            $('#pubUserPic').html( '<img src="~static/upload/face/guest_m.jpg" />' );
        }
        else {

            if( ctx.viewer.HasPic ) {
                $('#pubUserPic').html( '<div style="text-align:center;"><img src="'+ctx.viewer.obj.PicMedium+'" /></div>' );
            }
            else {
                $('#pubUserPic').html( '<div style="text-align:center;font-size:20px;font-weight:bold;">'+ctx.viewer.obj.Name+'</div>'+
                '<div style="text-align:center;"><img src="~static/upload/face/guest_m.jpg" /></div>' );
            }
        }
        
    };
    
    wojilu.site.load( showMTool );
    
});
</script>
