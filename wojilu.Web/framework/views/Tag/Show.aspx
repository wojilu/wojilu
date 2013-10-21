<style>
#tagResultsPanel { padding:10px 10px 15px 15px; margin:0px 10px 10px 0px; border-radius:5px;}
#tagResultsTitle { font-size:14px; font-weight:bold; margin:10px 0px 10px 5px;}

#tagDataList {margin:5px;}
#tagDataList li { margin:15px 5px 10px 15px; border-bottom:5px solid #eee; padding-bottom:15px;}
    .tag-item-time {color:#666; margin:5px 0px 0px 0px;}
    .tag-item-summary {margin-top:5px; color:#333;}

.tagListTitle {height:28px;margin:0px 0px 10px 0px; background:#f2f2f2; padding:3px 10px;border-radius:5px;}

.tagListTitle div { float:left; font-size:14px; font-weight:bold; margin-top:5px;}
.tagListTitle div.tagMore { float:right; font-size:12px; font-weight:normal;}

.tagListMain { padding:0px 10px 10px 10px;}
#tagList span{  margin:5px 10px 5px 0px; height:25px; vertical-align:middle}

</style>

<div class="row" style="padding-top:20px;">

    <div id="tagMain" class="span8">
    
    <div id="tagResultsPanel">

        <form id="tagForm" class="well form-search">
            <img src="~img/m/search.png" /> <input id="txtTag" type="text" value="#{tagName}" /> 

            <button id="btnTagSearch" class="btn btn-primary" type="submit">
                <i class="icon-search icon-white"></i>
                _{search}tag
            </button>
        </form>

    
        <ul id="tagDataList" class="unstyled">
            <!-- BEGIN list -->
            <li>
                <div class="font14 strong"><a href="#{data.Link}">#{data.Title}</a></div>
                <div class="tag-item-time">
                    <span class="right10">时间：#{data.Created}</span>
                    <span class="right10">#{data.Author}</span>
                    <span class="right10">#{data.TypeName}</span>
                </div>
                <div class="tag-item-summary">#{data.Summary}</div>
            </li>
            <!-- END list -->
        </ul>

        <div style="margin:15px 10px;">#{page}</div>
    </div>
        
    </div>
    
    <div id="tagList" class="span4" maxCount="#{maxCount}">
        <div style="padding:0px 10px 10px 10px;">
        <div class="tagListTitle">
            <div><img src="~img/s/tag_g.png" /> 常用 tag</div>
            <div class="tagMore"><a href="#{allLink}">_{more}...</a></div>
            <div class="clear"></div>
        </div>
        <div class="tagListMain">
            <div style="line-height:250%;">
                <!-- BEGIN maxTag -->
                <span dataCount="#{tag.DataCount}"><a href="#{tag.Link}" style="font-size:#{tag.FontSize}em;">#{tag.Name}</a></span><!-- END maxTag -->
            </div>
            <div class="clear"></div>
        </div>

        <div class="tagListTitle" style="margin-top:15px;">
            <div><img src="~img/s/tag.png" /> 最新 tag</div>
            <div class="tagMore"><a href="#{allLink}">_{more}...</a></div>
            <div class="clear"></div>
        </div>
        <div class="tagListMain">
            <div style="line-height:250%;">
                <!-- BEGIN recentTag -->
                <span dataCount="#{tag.DataCount}"><a href="#{tag.Link}" style="font-size:#{tag.FontSize}em;">#{tag.Name}</a></span><!-- END recentTag -->
            </div>
            <div class="clear"></div>
        </div>        
        
        </div>
    </div>
    


</div>




