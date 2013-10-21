
<div style="padding:20px;">

<form method="post" action="#{ActionLink}" class="ajaxPostForm">


    <table style="width:90%;">
        <tr>
            <td style="width:220px;">
                必须经过审核的用户</td>
            <td>
                <input name="contentSubmitterRole.NeedApproval" type="text" value="#{r.NeedApproval}" /></td>
        </tr>
        <tr>
            <td>
                不用审核直接发送</td>
            <td>
                <input name="contentSubmitterRole.Submitter" type="text" value="#{r.Submitter}" /></td>
        </tr>
        <tr>
            <td>
                不用审核直接发送（荣誉称号）</td>
            <td>
                <input name="contentSubmitterRole.AdvancedSubmitter" type="text" value="#{r.AdvancedSubmitter}" /></td>
        </tr>
        <tr>
            <td>
                直接发送（频道拥有者及相关人员）</td>
            <td>
                <input name="contentSubmitterRole.Editor" type="text" value="#{r.Editor}" /></td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <input id="Submit1" type="submit" class="btn" value="_{submit}" />
                &nbsp;</td>
        </tr>
    </table>
</form>
</div>