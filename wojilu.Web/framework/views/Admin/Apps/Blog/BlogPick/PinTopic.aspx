<div style="width:530px;">
<form method="post" action="#{ActionLink}">

<table style="width:500px; margin:20px auto;">
<tr>
    <td style="width:80px; vertical-align:top;  text-align:right; font-weight:bold;">标题</td><td>
        <div>#{x.Title}</div>  
    </td>
</tr>
<tr>
    <td style="text-align:right; font-weight:bold;">网址</td><td>
        <div>#{x.Link}</div> 
    </td>
</tr>
<tr><td></td><td style="height:30px;">&nbsp;</td></tr>
<tr>
    <td style="  vertical-align:top;  text-align:right; font-weight:bold;">固定到位置</td><td>
        <div>

        <div style="width:330px;height:20px; background:#eee; text-align:center;margin:2px 5px ;">
            <label style="cursor:pointer;"><input id="idx1" type="radio" name="Index" value="1" checked="checked" />1号位(头条)</label>            
        </div>

        <script>
            for (var i = 2; i < 12; i++) {

                document.write('<div style="float:left;width:160px;height:20px; background:#eee; text-align:;margin:2px 5px ;"><label style="cursor:pointer;"><input id="idx'+i+'" type="radio" name="Index" value="' + i + '" /> <span>' + i + '号位</span></label></div>');

            }
            
            var ids = [#{indexIds}];
            for (var i = 0; i < ids.length; i++) {
                $('#idx'+ids[i]).attr('disabled', 'disabled')
                var span = $('#idx'+ids[i]).next();
                span.html( '<span style="color:#aaa;">'+span.text()+ '(已占用)</span>');
            }
        </script>

        </div> 
    </td>
</tr>
<tr>
    <td>&nbsp;</td><td>
        <input id="Submit1" type="submit" value="固定帖子" class="btn" />
        <input id="Button1" type="button" value="_{cancel}" class="btnCancel" />
    </td>
</tr>
</table>
</form>
</div>
