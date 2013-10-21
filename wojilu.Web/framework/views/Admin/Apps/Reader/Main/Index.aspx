
<div class="" style="padding:20px;">

    <table style="width: 100%" id="dataAdminList" data-sortAction="#{sortAction}">

        <tr class="adminBar">
	        <td colspan="5"><a href="#{addLink}" class="frmBox"><img src="~img/add.gif"/> :{addChannel}</a></td>
        </tr>

        <tr class="tableHeader">
	        <td><strong>_{order}</strong></td>
	        <td><strong>:{sysCategory}</strong></td>
	        <td><strong>:{customName}</strong></td>
	        <td><strong>:{oName}</strong></td>
	        <td><strong>_{admin}</strong></td>
        </tr>
        <!-- BEGIN list -->
        <tr class="tableItems">
	        <td class="sort">
		        <img src="~img/up.gif" class="cmdUp right10" data-id="#{feed.Id}"/>
		        <img src="~img/dn.gif" class="cmdDown" data-id="#{feed.Id}"/>
	        </td>
	        
	        <td>#{feed.CategoryName}</td>

	        <td>#{feed.Name}</td>
	        <td>#{feed.Title}</td>
	        <td><a href="#{feed.LinkEdit}" class="frmBox">_{edit}</a> <a href="#{feed.LinkDelete}" class="deleteCmd">_{delete}</a></td>
        </tr>
        <!-- END list -->
        
        <tr><td colspan="5">#{page}</td></tr>
    	
    </table>

</div>

