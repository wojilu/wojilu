<h4>#{section.Name}</h4>

<ul class="ulposts unstyled">
<!-- BEGIN list -->
<li style=" margin-bottom:8px;">
    <span class="list-info">[#{post.Created} ]</span>
    <div style=" display:inline-block; width:400px;" class="overflow"><a href="#{post.Url}" class="list-title" style="#{post.TitleCss}">#{post.Title}</a><span class="note">_{view}:#{post.Hits}</span></div>
    </li>
<!-- END list -->
<div class="clear"></div>
</ul>

<div class="left10">#{page}</div>
<style>.currentPageNo{ background:#c00;}</style>