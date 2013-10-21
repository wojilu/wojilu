<h1>简单数据绑定</h1>

<link rel="stylesheet" href="~js/lib/zTree/css/zTreeStyle/zTreeStyle.css" type="text/css">

<div>

    <ul id="treeDemo" class="ztree" style="width:360px; overflow:auto;"></ul>
</div>


<script>


    var setting = {
			callback: {
				onExpand : onExpand,
                onCollapse : onCollapse
			}
    };

    function onExpand(event, treeId, treeNode, clickFlag) {

        /*
        $.post( '#{treeStatusLink}'.toAjax(), {url:treeNode.url}, function(data){
        });*/;
        
    }

    function onCollapse(event, treeId, treeNode, clickFlag) {

        /*
        $.post( '#{treeStatusLink}'.toAjax(), {url:treeNode.url, close:'true'}, function(data){
        });*/
        
    }

    var zNodes = #{jsonData};


    _run(function () {

        require(['lib/zTree/js/jquery.ztree.core-3.5'], function () {


            $.fn.zTree.init($("#treeDemo"), setting, zNodes);

        });



    });
</script>