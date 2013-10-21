<link type="text/css" rel="stylesheet" href="~js/lib/syntaxhighlighter/styles/sh.css"/>
<script type="text/javascript">
    _run(function () {
        require(['lib/syntaxhighlighter/scripts/sh'], function () {
            SyntaxHighlighter.config.clipboardSwf = "~js/lib/syntaxhighlighter/scripts/clipboard.swf";
            SyntaxHighlighter.all();
        });
    });
</script>

#{location}

#{currentPost}

<script>
    _run(function () {
        require(['wojilu.app.forum.edit'], function (x) {
            x.check( #{moderatorJson}, #{creatorId}, '#{tagAction}' );
        });
    });
</script>

<div style="margin-top:10px; margin-left:20px;">
	<div><a href="#{topic.Url}">&rsaquo;&rsaquo; :{expandTopic}</a></div>
	<ul class="replyPostList">	
	<!-- BEGIN replypost --><li><a href="#{p.MemberUrl}">#{p.MemberName}</a>: <a href="#{p.Url}" class="#{p.Class}">#{p.Title}</a> <span class="note font12 left10"> #{p.CreateTime} (:{wordLength}:#{p.Length})  _{hits}:#{p.Hits} </span></li>
	<!-- END replypost -->
	</ul>
	<div>#{replyPage}</div>
</div>