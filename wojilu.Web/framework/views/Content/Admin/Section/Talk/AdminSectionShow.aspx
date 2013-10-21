<table class="sectionToolbar">
<tr><td class="cmdAdmin"><a href="#{addUrl}" class="frmBox" xwidth="750">+:{addTalk}</a> 
<a href="#{listUrl}" class="frmBox" xwidth="800">_{all}</a></td></tr>
</table>

<div style="margin:0px 5px;">
    <table style="height:120px; width:100%;">
    <tr>
    <td>
	    <!-- BEGIN list -->
	    <div class="index_content_excerpt" style="margin-bottom:10px;">
		    <div>#{post.Content}</div>		
		    <div style="text-align:right;">
                -- #{post.Author}
                <a href="#{post.EditUrl}" class="frmBox left5 edit" xwidth="750"><img src="~img/edit.gif" /> _{edit}</a>
            </div>
	    </div>
	    <!-- END list -->
    </td>
    </tr>

    </table>
</div>