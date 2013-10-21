

<div class="adminMainPanel">
	<div id="listWrap">
	
	    <div style="margin-bottom:5px;">
            <form method="post" action="#{ActionLink}">
            <div style="margin-bottom:5px; color:#666;">
            <span class="strong orange">根据网址，获取此网址用到的所有模板</span>
            </div>
            <div>
                请输入页面网址 <input name="webAddress" style="width:380px;" type="text" /> 
                <input class="btn btns" type="submit" value="_{search}" /></div>
            </form>
	    </div>
	    
	    <div>
	        #{dirAndFiles}
	    </div>

    </div>
</div>

<style>
#listWrap {padding:5px 10px;}

div.line {padding:2px 10px; margin:0px 3px;}
div.dir { background:url("~img/folder.gif") no-repeat 5px center;padding-left:30px;}
div.file { background:url("~img/doc.gif") no-repeat 5px center;padding-left:30px;}
div.pic { background:url("~img/img.gif") no-repeat 5px center;padding-left:30px;}

</style>