    <table style=" margin:10px 0px 5px 0px;background2:#ebf3f7;width:100%; " border="0">
        <tr>
            <td rowspan="2" style="width:60px; vertical-align:top;">
                <img src="#{user.Face}" />
            </td>
            <td style="vertical-align:top;">
            <div>
                <a href="#{user.Link}">#{user.Name}</a> 
                <span class="note">#{created}</span>                
            </div>
            </td>
            <td style="text-align:right;">
            </td>
        </tr>
        <tr>
            <td colspan="2" style="vertical-align:top;">#{content}
            </td>
        </tr>
    </table>
