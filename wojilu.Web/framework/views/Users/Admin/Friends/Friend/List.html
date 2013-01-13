

<style>
.fItem {width: 285px; height:138px; float:left; display:inline; margin:6px; border-radius:3px; box-shadow:2px 2px 10px #aaa; overflow:hidden;}
.fPic {border-radius:4px; width:100px; height:100px;}
</style>
    				
		    <!-- BEGIN list -->
			<table class="fItem" cellpadding="4">
			    <tr>
				    <td style="width:100px; text-align:center; vertical-align:top; padding-top:8px;">
				        <div><a href="#{m.UrlFull}" target="_blank"><img src="#{m.FaceFull}" class="fPic"></a></div>
				        <div><span href="#{m.DeleteUrl}" class="deleteCmd"><img src="~img/delete.gif" /> _{canelFriend}</span></div>
				    </td>
				    <td style="vertical-align:top;">
				        <div class="friendName"><a href="#{m.UrlFull}" target="_blank">#{m.Name}</a></div>
					    <div>_{regTime}: #{m.CreateTime}</div>
					    <div>_{lastLoginTime}: #{m.LastLoginTime}</div>
					    <div>
					        <span class="lblCategory">_{friendCategory}:<span id="lblCat#{m.Id}">#{m.CategoryName}</span></span>
					        <span class="categoryEditCmd left5 link #{m.HideClass}" categoryId="#{m.CategoryId}" friendId="#{m.Id}">_{edit}</span>
					    </div>
					    <div class="note">_{description}:<span id="lblDescription#{m.Id}">#{m.Description}</span></div>
				    </td>
			    </tr>
		    </table>
		    <!-- END list -->
		    <div class="clear"></div>
		    <div class="page" style="padding-left:20px;">#{page}</div>
    				

    <div id="editForm" class="ebox" style="width:350px;">
    <div style="padding:10px 5px 10px 10px;">

        <table style="width:100%;">
            <tr>
                <td>_{category}</td>
                <td>#{FriendCategory} <a href="#{categoryLink}">_{categoryAdmin}</a></td>
            </tr>
            <tr>
                <td>_{description}</td>
                <td><textarea id="txtDescription" style="width:280px; height:45px;"></textarea></td>
            </tr>
            <tr>
                <td></td>
                <td><input id="btnEdit" type="button" class="btn" value="_{edit}" />
                    <input id="btnCancel" type="button" class="btnOther" value="_{cancel}" />
                    <input id="friendId" type="hidden" />
                </td>
            </tr>
        </table>

    </div>
    </div>

    <script>
    _run( function() {
        //$("#editForm").draggable();
        $('.categoryEditCmd').click( function() {
            var ps = $(this).position();
            
            var editForm = $('#editForm');
            var fWidth = editForm.width();
            var formX = ps.left;
            if( formX+ fWidth > document.body.clientWidth ) formX = document.body.clientWidth-fWidth-25;
            
            editForm.css( 'top', ps.top+20 ).css( 'left', formX ).slideToggle(130);
            
            var friendId = $(this).attr( 'friendId' );
            $('#FriendCategory').val( $(this).attr( 'CategoryId' ) );
            $('#friendId').val( friendId );
            $('#txtDescription').val( $('#lblDescription'+friendId).text() );
        });
        
        $('#btnCancel').click( function() {
            $('#editForm').slideToggle(130);
        });
        
        $('#btnEdit').click( function() {
            var categoryId = $('#FriendCategory').val();
            var friendDescription = $('#txtDescription').val();
            var friendId = $('#friendId').val();
            
            $.post( '#{saveCategoryLink}'.toAjax(), {'friendId':friendId, 'categoryId':categoryId, 'friendDescription':friendDescription}, function(data) {
            
                if( data.IsValid ==false ) {
                    alert( data.Msg );
                    return false;
                }                
                
                $('#lblCat'+data.FriendId).html( data.Name );
                $('#lblDescription'+data.FriendId).html( data.Description );
                
                $('#FriendCategory').val('');
                $('#txtDescription').val('');
                $('#friendId').val('');
                $('#editForm').slideToggle(130);
            });
            
        });
        
    });
    </script>

</div>

