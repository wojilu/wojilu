<link href="~css/wojilu.core.space.css?v=#{cssVersion}" rel="stylesheet" />

<div style="margin:20px 0px 0px 25px;">

    <div style="width:190px; float:left; border:1px solid #ccc; background:#fff;border-radius:4px;">

        <div style="margin:10px 0px; text-align:center;">

		    <div class="userFace" style="text-align:center;margin-top:20px;">
            <a href="#{u.Link}"><img src="#{u.PicMedium}"  style="width:100px;border-radius:4px; box-shadow:0px 1px 6px #aaa;" /></a>
            </div>		
		
		    <div class="sideBar_Panel" style="border:0px;">
                <div style="margin:3px 0px;"><a href="#{u.Link}">#{u.Name}</a></div>
                <div class="note">#{u.Created} 加入</div>
                <div style="margin-top:10px;">#{followCmd}</div>
                <div style="margin-top:5px;">
                    <a href="#{sendMsgLink}" class="btn" id="sendMsgCmd"><i class="icon-envelope"></i> 消息</a>
					    <a href="#{shareLink}" class="btn frmBox" xwidth="500" title="_{shareUser}"><i class="icon-share-alt"></i> _{share}</a>
                </div>

		    </div>

            
        </div>

        <div style="margin:30px 10px 30px 10px;">

            <div style="margin:10px 5px; "><strong>性别</strong> #{u.Gender}</div>
        
            <div style="margin:10px 5px 5px 5px; font-weight:bold;">兴趣爱好</div>
            <div style="margin-left:5px; color:#666;">#{u.Hobby}</div>
        </div>
    
    </div>

    <style>
    .wfUserNavWrap {width:880px;  border:1px solid #dedede;border-radius:4px;margin:0px auto 20px auto;background:#fff; }
    .wfUserNav {width:100%; }
    .wfUserNav td { padding:8px 0px; text-align:center; font-size:14px;}
    .wfUserNav a { text-decoration:none;}

    .wfNavItem:hover {background:#efefef;}    
    .wfNavCurrent { background:#61c0d6; color:#fff;}
    .wfNavCurrent a {color:#fff;}
    .wfNavCurrent:hover {background:#61c0d6;}
    
    </style>

    <div style=" float:left; width:930px;">

        <div class="wfUserNavWrap">
        <table class="wfUserNav">
            <tr>
                <td id="tIndex" style="width:120px;" class="wfNavItem"><a href="#{u.Link}">收集 <span>#{u.Pins}</span></a></td>
                <td id="tLike" style="width:120px;" class="wfNavItem"><a href="#{u.LikeLink}">喜欢 <span>#{u.Likes}</span></a></td>
                <td id="tAlbum" style="width:120px;" class="wfNavItem"><a href="#{u.AlbumLink}">专辑 <span>#{u.Albums}</span></a></td>
                <td id="tFollower" style="width:120px;display:none;" class="wfNavItem"><a href="#{u.FollowerLink}" target="_blank">粉丝 <span>#{u.Followers}</span></a></td>
                <td style=" text-align:right;">                
                
                </td>
            </tr>
        </table>
        </div>

<script>
_run(function () {
    var href = window.location.href;

    if (href.indexOf('like') > -1) {
        $('#tLike').addClass('wfNavCurrent');
    }
    else if (href.indexOf('album') > -1) {
        $('#tAlbum').addClass('wfNavCurrent');
    }
    else if (href.indexOf('follower') > -1) {
        $('#tFollower').addClass('wfNavCurrent');
    }
    else {
        $('#tIndex').addClass('wfNavCurrent');
    }

});
</script>
    

    
    #{layout_content}

    </div>

    <div style=" clear:both;"></div>



</div>


