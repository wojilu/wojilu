<style>
.blog-category-name { font-size:14px; margin:10px 0px 10px 0px; padding:10px; padding-left:0px;}

.blog-item {width:98%; margin-top:20px; margin-bottom:10px; border-bottom1:1px #eee solid; background:#f2f2f2; border-radius:4px;}
.blog-item-user {width:80px;padding:10px 5px 5px 5px;vertical-align:top; text-align:center;}
.blog-item-user img { border-radius:4px;}

.blog-item-post { padding-top:10px;}
    .blog-item-title { font-size:14px; font-weight:bold;}
    .blog-item-created { color:#aaa; padding-top:5px;}
    .blog-item-abstract { color:#666; padding:5px; padding-left:0px; padding-bottom:10px;}
</style>

	
<div class="blog-category-name">当前路径：<a href="#{blogLink}">博客频道</a> &raquo; #{listName}</div>

<!-- BEGIN list -->	
<table class="blog-item">
	<tr>
		<td class="blog-item-user">#{post.Face}</td>
		<td class="blog-item-post">
		    <div class="blog-item-title"><a href="#{post.LinkShow}">#{post.Title}</a></div>
		    <div class="blog-item-created">
                #{post.Created}
                <span class="left10 right10">点击:#{post.Hits}</span>
                <span>回复:#{post.Replies}</span>
            </div>
		    <div class="blog-item-abstract">#{post.Abstract}</div>
		</td>
	</tr>
</table>	
<!-- END list -->

<div style="margin-top:20px">#{page}</div>
