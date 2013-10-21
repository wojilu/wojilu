<div style="">
	<form action="#{ActionLink}" method="post" class="">
	
        <style>
        .tabList li{ width:130px;}
        </style>	
            
	    <ul class="tabList clearfix" style="margin:5px 10px 5px 20px;">
	    <li class="currentTab firstTab">:{picDescription}<span></span></li>
	    <li><a href="#{post.ImgListUrl}">:{uploadPics}</a><span></span></li>
	    </ul>

	    <div class="tabMain" style="width:95%; " >
	    <div style="padding:10px">
		    <table style="width: 680px"  cellpadding="3" cellspacing="0">
			    <tr>
				    <td>_{title}</td>
				    <td><input name="Title" style="width: 350px" type="text" value="#{post.Title}" class="tipInput" tip="_{exTitle}"><span class="valid" mode="border"></span>
                    <span class="left10"></span>
                    #{post.IsCloseComment}
                    
                    </td>
			    </tr>
			    <tr>
				    <td>_{titleHome}</td>
				    <td><input name="TitleHome" style="width: 350px" type="text" value="#{post.TitleHome}" ></td>
			    </tr>

	<tr>
		<td style="width:100px">SEO Keywords</td>
		<td><input name="MetaKeywords" type="text" style="width: 99%" value="#{post.MetaKeywords}" /></td>
	</tr>
	<tr>
		<td style="width:100px;vertical-align:top">SEO Description</td>
		<td style=""><textarea name="MetaDescription" style="width: 99%; height:30px;">#{post.MetaDescription}</textarea></td>
	</tr>
			    <tr>
				    <td style="vertical-align:top">_{description}</td>
				    <td>
<script type="text/plain" id="Content" name="Content"></script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(250).line(1).show();
    });
</script>
                    
                    </td>
			    </tr>
			    <tr class="hide">
				    <td>_{orderId}</td>
				    <td>
				    <input name="OrderId" size="20" style="width: 30px" type="text" value="#{post.OrderId}"></td>
			    </tr>


			    <tr>
				    <td>&nbsp;</td>
				    <td>
				        <input name="Submit1" type="submit" value=":{editPicsInfo}" class="btn">
				        <input type="button" value="_{cancel}" class="btnCancel" />
				    </td>
			    </tr>
		    </table>
	    </div>
	    </div>
    </form>


</div>
