<div style="width:800px;">

<style>
.tabList {border-bottom: 1px solid #B8D5FF;}
.tabList li { width:130px;}
</style>
	
    <ul class="tabList" style="margin:20px;">
        <li class="firstTab"><a href="#{customLink}">自定义模板</a><span></span></li>
        <li class="currentTab"><a href="#">选择已有模板</a><span></span></li>
        <div class="clear"></div>
    </ul>

	
	<div id="content1" class="tabMain" style="width:780px;">

        <div style="padding:20px;">
            <form method="post" action="#{ActionLink}">
            	
            <table class="">
	            <tr>
		            <td>:{templateType}</td>
		            <td>#{templateType}</td>
	            </tr>

	            <tr>
		            <td>&nbsp;</td>
		            <td>
		                <input name="Submit1" type="submit" value=":{editSection}" class="btn">
		                <input type="button" value="_{cancel}" class="btnCancel" />
		            </td>
	            </tr>
            </table>

            </form>
        </div>
    </div>

</div>
