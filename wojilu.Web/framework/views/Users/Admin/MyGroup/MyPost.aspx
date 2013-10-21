<style>

.panelTitle { padding:5px 10px; font-size:14px;}

#postsWarp {border:1px #ccc solid; width:95%; margin:auto; }
#postsTitle {border-bottom:1px #ccc solid; padding:5px 10px; background:#eee;}
.currentTitle {font-weight:bold; font-size:14px;}

.postList {width:98%; margin:auto; margin-top:5px;}
.postList td { padding-left:5px;border:0px; border-bottom:1px #ccc solid;}

.psHeader td{padding:10px 5px;}

td.postTitle div {max-width:600px;_width:600px; text-overflow: ellipsis;white-space:nowrap;-o-text-overflow:ellipsis;-moz-text-overflow:ellipsis;overflow: hidden; font-size:14px;}
.psTime {color:#666;}

.pageWrap {margin:auto; margin-top:10px; margin-bottom:10px; width:95%;}

</style>

<div id="postsWarp">

    <div id="postsTitle"><span class="currentTitle right20">我参加的小组的帖子</span></div>

    <table class="postList" border="0" cellpadding="0" cellspacing="0">
    	
	    <tr class="psHeader">
		    <td style="width:15%" >_{created}</td>
		    <td style="width:45%">_{title}</td>
		    <td style="width:10%">版块</td>
		    <td style="width:15%" class="center">_{reply}/_{view}</td>
		    <td style="width:15%">最后发帖</td>
	    </tr>
    	
	    <!-- BEGIN list -->
	    <tr class="dataRow">
		    <td>#{p.CreateTime}</td>
		    <td style="" class="postTitle">
		        <div>
		        #{p.TypeImg} <a href="#{p.Url}" style="#{p.TitleStyle}">#{p.Titile}</a>
		        <span style="vertical-align:middle">#{p.Attachments}</span>
		        </div>
		    </td>
		        <td><a href="#{p.OwnerLink}" style="color:#666;" target="_blank">#{p.OwnerName}</a></td>
		    <td align="center"><span class="orange">#{p.ReplyCount}</span><span class="gray">/</span><span class="note">#{p.Hits}</span></td>
		    <td style="padding:3px 0px 3px 0px;">
		        <div>#{p.RepliedUserName}</div>
		        <div class="psTime">#{p.Replied}</div>		        
		    </td>
	    </tr>
	    <!-- END list -->	

    </table>

    <div class="pageWrap">#{page}</div>

</div>



