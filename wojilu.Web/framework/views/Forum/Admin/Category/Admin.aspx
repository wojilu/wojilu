<div style="margin:10px">
    <table style="width: 100%;margin-bottom:10px;" id="dataAdminList" data-sortAction="#{sortAction}">
	    <tr class="tableHeader">
		    <td style="width:120px;text-align:center">_{orderId}</td>
		    <td>_{name}/:{topicCount}</td>
		    <td>_{admin}</td>
	    </tr>	

        <!-- BEGIN list -->
        <tr class="tableItems">
	        <td class="sort" style="text-align:center">
		        <img src="~img/up.gif" class="cmdUp right10" data-id="#{category.Id}"/>
		        <img src="~img/dn.gif" class="cmdDown" data-id="#{category.Id}"/>
	        </td>
	        <td><span style="color:#{category.NameColor}">#{category.Name}</span>(#{category.TopicCount})</td>
	        <td> <a href="#{category.EditUrl}" class="editCmd">_{edit}</a> <span href="#{category.DeleteUrl}" class="deleteCmd">_{delete}</span></td>
        </tr>
        <!-- END list -->

    </table>

    <div class="formPanel">		
        <div class="strong" style="margin:5px 5px 5px 3px;">:{addCategoryToBoard}:<span class="red">#{forumBoard.Name}</span></div>
        <form method="post" action="#{ActionLink}">
        <table style="width: 520px">
	        <tr>
		        <td>:{newCategoryName}</td>
		        <td><input name="Name" type="text" class="tipInput" tip="_{exName}" /><span class="valid" mode="border"></span></td>
	        </tr>
	        <tr>
		        <td>:{nameColorStyle}</td>
		        <td><input name="NameColor" type="text" value="" /><span class="note">:{nameColorTip}</span></td>
	        </tr>
	        <tr><td></td>
		        <td>
		        <input name="Submit1" type="submit" value=":{addCategory}" class="btn" />
		        <input type="button" value="_{cancel}" class="btnCancel" />
		        <input type="hidden" name="BoardId" value="#{BoardId}" /></td>
	        </tr>
        </table>
        </form>

    </div>
</div>