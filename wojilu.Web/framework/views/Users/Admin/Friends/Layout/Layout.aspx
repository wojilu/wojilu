<style>

.tabList li{ width:100px;}
</style>

<div style="margin:15px 0px 0px 20px; font-size:14px; font-weight:bold;"><img src="~img/s/users.png" /> _{friend}</div>

<div style="width:95%;margin:15px 0px 0px 20px;">
<ul class="tabList clearfix">
<li class="firstTab"><a href="#{f.ListLink}" class="frmLink" loadTo="tabMain" nolayout=4>_{myFriends}</a><span></span></li>
<li><a href="#{f.FollowingLink}" class="frmLink" loadTo="tabMain" nolayout=5>_{myFollowing}</a><span></span></li>
<li><a href="#{f.MoreLink}" class="frmLink" loadTo="tabMain" nolayout=5>_{suggestPeople}</a><span></span></li>
<li><a href="#{f.SearchLink}" target="_blank"><img src="~img/search.gif" /> _{advancedUserSearch}</a><span></span></li>
<li><a href="#{f.InviteLink}" class="frmLink" loadTo="tabMain" nolayout=5>好友邀请</a><span></span></li>
<li><a href="#{f.Rank}" target="_blank">用户排行</a><span></span></li>

</ul>
</div>

<div class="tabMain" style="width:96%">
<div style="padding:10px;" id="tabMain">#{layout_content}</div>
</div>

<script>
_run( function() {
    wojilu.tool.makeTab( '.tabList', 'currentTab', '' );
});
</script>

