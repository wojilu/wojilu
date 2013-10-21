<style>
.title div{margin:3px 8px;}

.groupList {width:320px; float:left; margin:10px 20px 20px 10px; border-radius:4px; box-shadow:2px 2px 10px #aaa;}
.groupList img {border-radius:4px; }
.groupList td {padding:5px; font-size:12px;}
.groupTitle{font-size:14px;font-weight:bold;}
.groupTool div{border-bottom:1px #aaa solid;}

.groupCmd div {border-bottom:1px #aaa solid;}
.groupCmd div a {display:block;}
.groupCmd div a:hover{background:#ccc;}
</style>

<div class="clearfix">



<table style="width: 100%" class="groupMy">
	<tr>
		<td >
			<div class="title"><div>#{resultLabel}:</div></div>
			<!-- BEGIN list -->
			<table class="groupList">				
				<tr>
					<td valign="top" align="center" style="width:140px;">
					    <div><a href="#{g.Url}"><img src="#{g.Logo}" /></a></div>
					</td>
					<td valign="top"> 
						<div class="groupTitle"><a href="#{g.Url}">#{g.Name}</a></div>
						<div>#{g.Info}</div>
					</td>
				</tr>	
			</table>
			<!-- END list -->			
		</td>
	</tr>
</table>

</div>
