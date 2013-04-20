



<script>
    _run(function () {
        require(["http://ajax.aspnetcdn.com/ajax/jquery.validate/1.10.0/jquery.validate.min.js"], function () {

            wojilu.editor.bind('content').show(function (x) {
                x.addListener('contentchange', function () {
                    this.sync();
                    //1.2.4+以后可以直接给textarea的id名字就行了
                    $('textarea').valid();
                });

                var validator = $("#myform").submit(function () {
                    x.sync();
                }).validate({
                    ignore: "",
                    rules: {
                        title: "required",
                        content: "required"
                    },
                    errorPlacement: function (label, element) {
                        label.insertAfter(element.is("textarea") ? element.next() : element);
                    }
                });
                validator.focusInvalid = function () {
                    if (this.settings.focusInvalid) {
                        try {
                            var toFocus = $(this.findLastActive() || this.errorList.length && this.errorList[0].element || []);
                            if (toFocus.is("textarea")) {
                                x.focus()
                            } else {
                                toFocus.filter(":visible").focus();
                            }
                        } catch (e) {
                        }
                    }
                }

            });


        });
    });
</script>

<div style="margin:30px;">

<form id="myform" action="">
    <h3>Ueditor在jquery validation下的验证</h3>

    <label>其他内容</label>
    <input name="title" />

    <br/>

    <label>编辑器</label>
    <textarea id="content" name="content" rows="15" cols="80" style="width: 80%"></textarea>

    <br />
    <input type="submit" name="save" value="Submit" />
</form>
</div>