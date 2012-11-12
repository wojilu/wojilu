require(["lib/bootstrap-dropdown"], function() { 
        
    $(function() {
        for( var i=0;i<__funcList.length;i++) {
             __funcList[i]();
        }
    }); 

});
