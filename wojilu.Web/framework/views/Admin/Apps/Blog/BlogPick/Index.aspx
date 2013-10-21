<style>
#pickList li div.pickListTitle {
     width:190px;
     overflow: hidden;     
     white-space:nowrap;
     text-overflow:clip; 
}

#pickList li.pickItem {
    float:left;width:220px; border-bottom:1px #eee solid; margin:0px 20px 0px 0px; padding:0px 0px 0px 0px;
    height:23px;
}

#pickList li.pickItem:hover { background:#eee;}
</style>

<table style="width:100%;">
    <tr>
    <td style="width:35%; vertical-align:top;">
        <div style="margin:5px 10px 10px 8px; background:#f2f2f2; padding:3px 10px; box-shadow:3px 3px 3px #ddd; border-radius:4px;">
            <a href="#{lnkPicAdd}" class="frmBox"><img src="~img/add.gif" /> 添加图片</a>
            <a href="#{lnkPicAdmin}" class="frmBox left10"><img src="~img/edit.gif" /> 修改下面的图片</a>
        </div>

        <div class="forum-top-section" style="height:250px;">
        <script>_run(function () { wojilu.ui.slider(); }); </script>
        <div class="slideWrap" style="margin:0px;width: 350px; height:230px;margin:10px;"> 
            <!-- BEGIN pickedImg --><a href="#{f.Url}"><img alt="#{f.Title}" src="#{f.ImgUrl}" /></a><!-- END pickedImg -->
        </div>
        <div>&nbsp;</div>
        </div>
    </td>

    <td style="width:65%; vertical-align:top;">

        <div style="margin:5px 10px 20px 8px; background:#f2f2f2; padding:3px 10px; box-shadow:3px 3px 3px #ddd; border-radius:4px;">
            <a href="#{lnkAddPin}" class="frmBox"><img src="~img/add.gif" /> 增加自定义内容</a>
            <a href="#{lnkRestore}" class="frmBox left20" title="恢复被隐藏帖子"><img src="~img/s/restore.png" /> 恢复被隐藏帖子</a>
        </div>

        <div style="margin:10px; margin-bottom:5px;">
            <!-- BEGIN hotPick -->
            <table style="width:100%; margin-bottom:10px;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td style="font-size:18px; font-weight:bold; ">#{x.Icon} #{x.Title}</td>
                <td style="width:50px; vertical-align:top;">
                <div class="menuMore" list="myMenuHot">
                    <img src="~img/edit.gif" /> 修改
                    <ul id="myMenuHot" class="menuItems" style="width:90px;">                        <li><a href="#{x.EditLink}" class="frmBox">编辑此帖</a></li>                        <li><a href="#{x.DeleteLink}" class="frmBox">隐藏此帖</a></li>                        <li><a href="#{x.PinLink}" class="frmBox">固定位置</a></li>                    </ul>                        
                </div>
                </td>            
            </tr>            
            </table>


            <div style="padding:3px 10px 3px 3px; color:#666;">#{x.Summary}</div>
            <!-- END hotPick -->
        </div>



        <div style="margin:0px 10px 10px 10px;">
            <ul id="pickList">
            <!-- BEGIN pickList -->
                <li class="pickItem">

                    <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                    <td>
                        <div class="pickListTitle">
                            <a href="#{x.LinkShow}" target="_blank">#{x.Icon} #{x.Title}</a>
                        </div>
                    </td>
                    <td>
                        <div class="menuMore" list="myMenu#{x.Id}">
                            <img src="~img/edit.gif" />
                            <ul id="myMenu#{x.Id}" class="menuItems" style="width:90px;">                                <li><a href="#{x.EditLink}" class="frmBox">编辑此帖</a></li>                                <li><a href="#{x.DeleteLink}" class="frmBox">隐藏此帖</a></li>                                <li><a href="#{x.PinLink}" class="frmBox">固定位置</a></li>                            </ul>       
                        
                        </div>            

                    </td>
                    </tr>
                    </table>

                </li>
            <!-- END pickList -->
            </ul>
        </div>
    
    </td>


    </tr>
</table>