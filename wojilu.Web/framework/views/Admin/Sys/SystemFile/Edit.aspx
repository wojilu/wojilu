
<div class="adminMainPanel">
	<div id="listWrap">
	    <div style="margin-bottom:5px;"><strong style="margin-right:10px;">_{editFile}</strong>_{currentPath}: #{ViewDir}</div>
	    <div class="orange strong">#{editTip}</div>

	    <form action="#{ActionLink}" method="post" class="ajaxPostForm">
	    <div>
            <textarea name="TemplateContent" style="width:99%; height:480px; font-family:Consolas">#{templateContent}</textarea></div>
        #{backupCmd}
        <div style="margin-top:5px;">
            <input id="Submit1" type="submit" value="_{submit}" class="btn" #{disabled} />
            <input id="Button1" type="button" value="_{return}" class="btnReturn" />
            <input id="Hidden1" name="file" type="hidden" value="#{fileName}" />
            
        </div>
        </form>
        <div style="margin:15px 0px;">#{restoreCmd}</div>
	</div>	

</div>

<style>
#listWrap {padding:5px 10px;}


</style>