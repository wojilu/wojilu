<table  class="sectionToolbar">
    <tr><td class="cmdAdmin"><a href="#{addUrl}" class="frmBox" xwidth="800">+:{addPost}</a> 
    <a href="#{listUrl}" class="frmBox" xwidth="800">_{all}</a></td></tr>
</table>

<table style="margin:5px;">
    <tr>
    <td class="">
    <!-- BEGIN list -->
    <div class="summary-item">
    <h3><a href="#{post.Url}" title="#{post.TitleFull}" style="#{post.TitleCss}" class="frmBox" xwidth="800">#{post.TitleShow}</a></h3>
    <div class="note font12">#{post.AuthorShow} 发布于 #{post.Created} <span class="left10">#{post.HitsShow}</span> <span class="left10">#{post.RepliesShow}</span></div>
    <div style=" " class="clearfix summary-body">
        <a href="#{post.Url}" class="summary-pic">#{post.PicSHtml}</a>
        #{post.SummaryShow} <a href="#{post.Url}" class="frmBox left15 edit" xwidth="800"><img src="~img/edit.gif" />_{edit}</a>
    </div>
    </div>
    <!-- END list -->
    </td>
    </tr>
</table>
