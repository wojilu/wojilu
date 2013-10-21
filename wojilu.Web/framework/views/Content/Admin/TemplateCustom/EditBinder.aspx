<div style="width:800px; height:480px;">
	
<style>
.tabList {border-bottom: 1px solid #B8D5FF;}
.tabList li { width:130px;}
</style>
	
    <ul class="tabList" style="margin:20px;">
        <li class="firstTab currentTab"><a href="#">自定义模板</a><span></span></li>
        <li><a href="#{selectLink}">选择已有模板</a><span></span></li>
        <div class="clear"></div>
    </ul>
	
	<div id="content1" class="tabMain" style="width:780px;">
        <div style="padding:10px;">
        
            <form method="post" action="#{ActionLink}">
            <div style="margin:10px;">
                <div><textarea id="TextArea1" name="Content" style="width:99%; height:350px; line-height:150%; font-family:Consolas;">#{templateContent}</textarea></div>
                <div style="margin-top:5px;">
                    <input id="Submit1" class="btn" type="submit" value="_{submit}"  style="float:left;"/>&nbsp;
                    <input id="Button1" class="btnCancel" type="button" value="_{cancel}" style="float:left;margin-right:20px;" />&nbsp;
                    
                </div>
            </div>
            </form>
            
            <div><div style="float:left;margin-top:-28px;margin-left:200px;"><a class="putCmd cmd strong" href="#{resetLink}"><span><img src="~img/back.gif" /> 恢复到默认模板</span></a></div></div>


        </div>
    </div>

</div>
