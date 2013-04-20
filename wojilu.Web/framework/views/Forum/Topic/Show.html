<link type="text/css" rel="stylesheet" href="~js/lib/syntaxhighlighter/styles/sh.css"/>
<script type="text/javascript">
    _run( function() {
        require(['lib/syntaxhighlighter/scripts/sh'], function () {
            SyntaxHighlighter.config.clipboardSwf = "~js/lib/syntaxhighlighter/scripts/clipboard.swf";
            SyntaxHighlighter.all();
        });
    });
</script>

<div class="row" style="margin-bottom:15px;"><div class="span12">#{location}</div></div>

<div class="forum-topic-prenext">
    <div class="forum-prenext-link">
        <div class="pull-left forum-prenext-spl"></div>
        <div class="pull-left forum-prenext-pre"><a href="?pre">:{prevTopic}</a></div>
        <div class="pull-left forum-prenext-sp">|</div>
        <div class="pull-left forum-prenext-next"><a href="?next">:{nextTopic}</a></div>
        <div class="pull-left forum-prenext-spr"></div>
    </div>    
    <div class="forum-prenext-pager">#{page}</div>    
    <div class="forum-prenext-cmd">                 
        <a href="#{newReplyUrl}" class="cmdreply btn btn-info" rel="nofollow" style="color:#fff;"><i class="icon-share-alt icon-white"></i> 回复</a>
        <a href="#{newTopicUrl}" class="cmdnew btn btn-success btn-white" rel="nofollow" style="color:#fff;"><i class="icon-plus icon-white"></i> 发布主题</a>
    </div>
</div>

#{postBlock}

<!-- copy from above -->
<div class="forum-topic-prenext">
    <div class="forum-prenext-link">
        <div class="pull-left forum-prenext-spl"></div>
        <div class="pull-left forum-prenext-pre"><a href="?pre">:{prevTopic}</a></div>
        <div class="pull-left forum-prenext-sp">|</div>
        <div class="pull-left forum-prenext-next"><a href="?next">:{nextTopic}</a></div>
        <div class="pull-left forum-prenext-spr"></div>
    </div>    
    <div class="forum-prenext-pager">#{page}</div>    
    <div class="forum-prenext-cmd">                 
        <a href="#{newReplyUrl}" class="cmdreply btn btn-info" rel="nofollow" style="color:#fff;"><i class="icon-share-alt icon-white"></i> 回复</a>
        <a href="#{newTopicUrl}" class="cmdnew btn btn-success btn-white" rel="nofollow" style="color:#fff;"><i class="icon-plus icon-white"></i> 发布主题</a>
    </div>
</div>

<!-- BEGIN form -->

<div class="forum-form-container" id="topicForm">

    <div class="row">
        <div style="padding-left:40px;"><h3>:{replyTopic}</h3></div>
    </div>

    <div class="clearfix" style="margin-top:10px;">

        <form method="post" action="#{post.ReplyActionUrl}" class="form-horizontal autoSubmitForm" id="pubTopicForm">

        <div class="pull-left" id="pubUserPic" style=" text-align:right;width:124px;margin-left:20px;margin-top:10px;">#{currentUser}</div>

        <div class="pull-left forum-form-quick">

            <div class="control-group">
                  <label class="control-label" for="txtTitle">_{title}</label>
                  <div class="controls">
                    <input id="txtTitle" name="Title" type="text" placeholder="请填写标题" value="#{post.ReplyTitle}" style="width:600px;">
                    
                  </div>
                  <div class="hide"><span class="valid" to="Title" msg="_{exTitle}" mode="border"></span></div>
            </div>

            <div class="control-group">
                  <label class="control-label">_{content}</label>
                  <div class="controls" style="width:610px;">
                    #{Editor}
                  </div>
                  <div class="hide"><span class="valid" to="Content" mode="border"></span></div>
            </div>

            <div class="control-group">    
                <label class="control-label"></label>

                <!-- Button --> 
                <div class="controls">

                    <button id="btnForumReply" name="btnSubmit" class="btn btn-primary" type="submit">
                        <i class="icon-pencil icon-white"></i>
                        :{submitPost}
                    </button>

                    <input type="hidden" name="forumId" value="#{forumBoard.Id}" />
                    <input type="hidden" name="topicId" value="#{post.TopicId}" />
                    <input type="hidden" name="parentId" value="#{post.ParentId}" />
                    <input type="hidden" name="currentPageId" value="#{currentPageId}" />
                    <input type="hidden" name="__ajaxUpdate" value="1" />
                </div>
            </div>

        </div>

        </form>
    </div>

</div>
<!-- END form -->

<div id="tagBox" class="ebox" style="width:280px; padding:10px; display:none;">
    <div>请输入tag(多个tag之间用逗号分隔)</div>
    <div><input id="tagValue" type="text" style="width:250px;" /></div>
    <div><input id="btnSaveTag" type="button" value="_{submit}" class="btn btn-primary" />
        <input id="tagCancel" type="button" value="_{cancel}" class="btn btnCancel" />
        <input id="tagId" type="hidden" />
    </div>
</div>


<script>
_run( function() {
    wojilu.ui.valid();

    var isTopicLocked = #{isTopicLocked};
    if( isTopicLocked ) $('#topicForm').hide();

    var appendHtml = function(data) {
        $('.postOne').last().after( data.Info );
        $('.editCmd', $('.postOne').last()).show(); // 显示编辑命令
        
        // 清空编辑器内容 
        wojilu.editor.clear('Content');
    };

    $('#btnForumReply').click( function() {

        var thisForm = $('#pubTopicForm');

        // disable & loading
        var btnSubmit = $(thisForm).find("[type='submit']");
        btnSubmit.attr( 'disabled', 'disabled' );
        btnSubmit.after( ' <span class="loadingInfo"><img src="'+wojilu.path.img+'/ajax/loading.gif"/>loading...</span>' );

        // 提交之前先同步编辑器内容
        wojilu.editor.sync('Content');

        $.post( thisForm.attr('action').toAjax(), thisForm.serializeArray(), function(data) {

            $( '.loadingInfo', thisForm ).html('');
            btnSubmit.attr( 'disabled', false );

            if( wojilu.str.isJson( data ) == false ) {
                alert(data);
                return;
            }

            if( !data.IsValid) {
                alert( data.Msg );
                return;
            }

            if( data.Msg =='ajax' ) {
                appendHtml(data);
            }
            else {
                window.location.href = data.Info;
            }

        });

        return false;
    });

});
</script>


<script>
    _run(function () {
        require(['wojilu.app.forum.edit'], function (x) {
            x.check( #{moderatorJson}, #{creatorId}, '#{tagAction}' );
        });
    });
</script>


