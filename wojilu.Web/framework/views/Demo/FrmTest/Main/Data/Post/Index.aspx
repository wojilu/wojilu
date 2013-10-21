

<table border="1" style="width:100%; margin:0px auto; border-collapse:collapse; border:1px #000 solid; background:#fff;" cellpadding="10">
    <tr>
        <td colspan="2" style="background:#000;"><div style="font-size:18px; text-align:center; color:#fff;">当前 action</div></td>
    </tr>
    <tr>
        <td>页面跳转</td>
        <td>弹窗跳转</td>
    </tr>
    <tr>
        <td style="width:50%;">
        
            <form method="post" action="#{redirectPage4}"><input id="Submit11" type="submit" value="echoRedirect(msg)" /></form>
            <form method="post" action="#{redirectPage}"><input id="Submit1" type="submit" value="echoRedirect(msg,url)" /></form>
            <form method="post" action="#{redirectPageCurrent}"><input id="Submit2" type="submit" value="echoRedirectPart_0" /></form>
            <form method="post" action="#{redirectPage1}"><input id="Submit3" type="submit" value="echoRedirectPart_1" /></form>
            <form method="post" action="#{redirectPage2}"><input id="Submit4" type="submit" value="echoRedirectPart_2" /></form>
            <form method="post" action="#{redirectPage3}"><input id="Submit5" type="submit" value="echoRedirectPart_3" /></form>

        </td>
        
        <td style="width:50%;">
        
            <div><a href="#{boxPage4}" class="frmBox">echoToParent(msg)</a></div>
            <div><a href="#{boxPage}" class="frmBox">echoToParent(msg,url)</a></div>
            <div><a href="#{boxPageCurrent}" class="frmBox">echoToParentPart_0</a></div>
            <div><a href="#{boxPage1}" class="frmBox">echoToParentPart_1</a></div>
            <div><a href="#{boxPage2}" class="frmBox">echoToParentPart_2</a></div>
            <div><a href="#{boxPage3}" class="frmBox">echoToParentPart_3</a></div>
            

        
        </td>
    </tr>
    
    <tr>
        <td colspan="2" style=" text-align:center;">
            <div>ajaxPostForm</div>
            <form class="ajaxPostForm" method="post" action="#{redirectPage4}"><input id="Submit12" type="submit" value="echoRedirect(msg)" /></form>
            <form class="ajaxPostForm" method="post" action="#{redirectPage}"><input id="Submit6" type="submit" value="echoRedirect(msg,url)" /></form>
            <form class="ajaxPostForm" method="post" action="#{redirectPageCurrent}"><input id="Submit7" type="submit" value="echoRedirectPart_0" /></form>
            <form class="ajaxPostForm" method="post" action="#{redirectPage1}"><input id="Submit8" type="submit" value="echoRedirectPart_1" /></form>
            <form class="ajaxPostForm" method="post" action="#{redirectPage2}"><input id="Submit9" type="submit" value="echoRedirectPart_2" /></form>
            <form class="ajaxPostForm" method="post" action="#{redirectPage3}"><input id="Submit10" type="submit" value="echoRedirectPart_3" /></form>


        </td>
    </tr>

</table>

	<script>
	
	_run( function() {
	    wojilu.ui.ajaxPostForm();
	    wojilu.ui.frmLink();
	});
	</script>

