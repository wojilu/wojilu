<style>
#tagList {  }
.tagListTitle { font-size:14px; font-weight:bold; margin:0px 0px 10px 0px; text-align:left;}
.tagListMain {}
#tagList .tagListMain span { margin:5px 10px 5px 0px; height:25px; vertical-align:middle}
</style>

<div id="tagList" style="padding:10px 10px 10px 10px;">
    
    <div style="padding-left:5px; padding-bottom:10px;">
        
        <div>#{toolbar}</div>
           
        <div class="tagListMain">
            <div style="line-height:250%;">
                <!-- BEGIN tags -->
                <span style="margin-right:20px;" id="tag#{tag.Id}">
                <a href="#{tag.Link}" style="font-size:#{tag.FontSize}em; ">#{tag.Name}<span style="font-size:11px; color:#aaa;">(#{tag.DataCount})</span></a><a href="#{tag.DeleteLink}" class="ajaxDeleteCmd" removeId="tag#{tag.Id}">×</a>
                </span>
                <!-- END tags -->
            </div>
                
        </div>
            
    </div>    
        
    <div>#{page}</div>   
        
</div>    



