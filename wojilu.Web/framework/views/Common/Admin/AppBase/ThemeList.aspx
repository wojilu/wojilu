
<div id="typePanel">


    <!-- BEGIN list -->
    <div class="typeItem">
        <div class="typePic">#{x.PicOrDesc}</div>
        <div><input class="rdoType" name="themeId" id="themeId#{x.Id}" type="radio" value="#{x.Id}" />
        <label for="themeId#{x.Id}">#{x.Name}</label>
        </div>

    </div>
    <!-- END list -->

    <div class="clear"></div>
</div>

<style>
#typePanel { font-size:12px; }
.typeItem {float:left; margin:5px;}
.typePic {width:152px; height:102px; border:1px solid #ccc; color:#666; font-size:14px; line-height:150%;overflow:hidden;}
.typePic .desc { margin:1px; height:100px; background:#f2f2f2;}
.desc-inner { padding:12px; overflow:hidden;}
.typeItem img{width:150px;height:100px;}
</style>

<script>
    _run(function () {

        $('.rdoType').click(function () {
            $('#themeId', window.parent.document).val($(this).val());
        });

        $($('.rdoType')[0]).click();
    });
</script>