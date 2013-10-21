<style>
.blog-pick-wrap { margin-bottom:20px;}
.blog-pick-wrap li {margin-bottom:3px;}
.blog-pick-wrap li div {width:200px;overflow: hidden; white-space:nowrap;text-overflow:ellipsis; }

.blog-pick-left {float:left;width:280px; background1:#ccc; margin-right:1px red solid; padding-top:10px; margin-left:0px;}
    .blog-pick-slider { height:220px;}
    .post-index {display:none;}
    
.blog-pick-right {float:left;width:300px;background1:#f2f2f2; margin-left:10px;}

    #blog-pick-hot {margin:5px; margin-left:0px;}
    #blog-pick-hot-title {font-size:16px; font-weight:bold; margin-bottom:px; margin-top:10px; width:300px; overflow: hidden; text-overflow:clip; white-space:nowrap;}
    #blog-pick-hot-summary {padding:3px 10px 3px 3px; color:#666;}
    
    #blog-pick-list-wrap {margin:0px 10px 10px 0px;}
    #blog-pick-list {margin-left:3px;}
    #blog-pick-list li div.pick-list-title {width:280px;overflow: hidden;white-space:nowrap;text-overflow:clip; }
    #blog-pick-list li.pick-item {width:280px; padding:1px 0px 2px 0px; list-style:none; }

</style>

<div class="blog-pick-wrap">

    <div class="blog-pick-left">
        <div class="blog-pick-slider">
        <script> _run(function() {wojilu.ui.slider();}); </script>
        <div class="slideWrap" style="width: 280px; height:220px;margin:0px; "> 
            <!-- BEGIN pickedImg --><a href="#{f.Url}"><img alt="#{f.Title}" src="#{f.ImgUrl}" /></a><!-- END pickedImg -->
        </div>
        <div>&nbsp;</div>
        </div>
    </div>

    <div class="blog-pick-right">

        <div id="blog-pick-hot">
            <!-- BEGIN hotPick -->
            <div id="blog-pick-hot-title"><a href="#{x.LinkShow}">#{x.Title}</a></div>
            <div id="blog-pick-hot-summary">#{x.Summary}</div>
            <!-- END hotPick -->
        </div>

        <div id="blog-pick-list-wrap">
            <ul id="blog-pick-list">
                <!-- BEGIN pickList -->
                <li class="pick-item">
                    <div class="pick-list-title"><a href="#{x.LinkShow}">#{x.Title}</a></div>
                </li>
                <!-- END pickList -->
            </ul>
        </div>
    
    </div>

</div>
<div class="clear"></div>
