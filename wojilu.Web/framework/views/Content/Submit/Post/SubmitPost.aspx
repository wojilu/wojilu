<div style="">

    <div style="border-bottom:1px #ccc solid; padding:5px ;">投递新闻到：<strong>#{section.Name}</strong>
            <img src="~img/s/back.png" class="left20" /><a href="javascript:history.back();">_{return}</a>

    </div>

    <div style="margin:20px 5px;">
    
<script type="text/javascript">
<!--
    _run(function () {

        $('.tabList li').click(function () {

            $('.tabList li').removeClass('currentTab');
            $(this).addClass('currentTab');

            var tabId = $(this).attr('data-id');

            $('.tabBody').hide();
            $('#' + tabId).show();
        });

    });
//-->
</script>

<style>
#title {width: 600px}
#tagList {width:200px}
#srcLink {width:350px;}

.tabList  {margin-left:20px;}
.tabList li {width:120px; }
.tabBody {width:95%; padding:10px 0px 10px 20px;}
</style>

	
<form method="post" action="#{ActionLink}" style="margin-bottom:30px;" class="ajaxPostForm">


    <ul class="tabList">
        <li class="firstTab currentTab" data-id="content1"><a href="javascript:void(0);">:{general}</a><span></span></li>
        <li data-id="content2"><a href="javascript:void(0);">:{advanced}</a><span></span></li>
    </ul>    
	
	<div id="content1" class="tabBody">
	<table style="width: 100%;margin:auto;">
		<tr class="hide">
			<td style="width: 80px;">:{itsSection}</td>
			<td style="width: 670px;"><strong>#{section.Title}</strong></td>
		</tr>
		<tr>
			<td>_{title}</td>
			<td><input name="Title" id="title" type="text" class="tipInput" tip="_{exTitle}"><span class="valid" mode="border"></span></td>
		</tr>
		<tr>
			<td>_{author}</td>
			<td><input name="Author" type="text" style="width:165px;">
			<span class="left15">:{srcUrl}</span> <input name="SourceLink" id="srcLink" type="text" size="20">

			</td>
		</tr>	
		<tr>
			<td style=" vertical-align:top;">_{content}</td>
			<td>
<script type="text/plain" id="Content" name="Content"></script>
<script>
    _run(function () {
        wojilu.editor.bind('Content').height(350).line(1).show();
    });
</script>
<span class="valid border" to="Content"></span>
            
            </td>
		</tr>	
		<tr>
			<td>&nbsp;</td>
			<td style="padding-top:10px;">
                <button class="btn btn-primary" type="submit">
                    <i class="icon-plus icon-white"></i>
                    :{submitData}
                </button>
			    <input type="button" value="_{return}" class="btn btnReturn left20" />
			</td>
		</tr>		
		
	</table>
	</div>
	
	<div id="content2" class="tabBody" style="display:none;">
	<table style="width: 100%;margin:auto;">
	    <tr>
	        <td>_{tag}</td>
	        <td><input name="TagList" id="tagList" type="text">
            <span class="note" title="">_{tagTip}</span></td>
	    </tr>

		<tr >
			<td style="vertical-align:top">_{summary}</td>
			<td>
			<textarea name="Summary" style="width: 600px; height: 90px"></textarea></td>
		</tr>
		<tr>
			<td style="width: 80px;vertical-align:top">:{picUrl}</td>
			<td>
			<input name="ImgLink" id="imgLink" type="text" style="width: 450px">
			<span class="note">:{picUrlTip}</span></td>
		</tr>
	</table>
	</div>
	


</form>

		


    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    </div>

</div>

