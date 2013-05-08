define( [], function() {

    var loadingInfo = ' <span class="loadingInfo"><img src="' + wojilu.path.img + '/ajax/loading.gif"/> loading...</span>';

    // 一般评论后的“回复”命令
    function bindReplyEvent(objX) {
        $('.lnkReply').click(function () {

            var rformWrap = $('.replyFormWrap', $(this).parent().parent());
            var formInner = $('.commentFormInner', rformWrap);
            if (formInner.length > 0) {
                formInner.toggle();
            }
            else {
                var clonedForm = $('#commentFormWrap').clone();
                rformWrap.append(clonedForm);
                $('.btnComment', clonedForm).after('<input name="parentId" type="hidden" value="' + $(this).attr('data-ParentId') + '" />');
                bindSubmitEvent(objX);
            }
            return false;

        });
    }

    // 子评论中的“回复”命令
    function bindSubReplyEvent(objX) {
        
        $('.lnkSubReply').unbind('click').click(function () {

            var rformWrap = $('.replyFormWrap', $(this).parentsUntil('.replySubList').parent().parent());
            var formInner = $('.commentFormInner', rformWrap);

            var parentId = $(this).attr('data-ParentId');
            var atId = $(this).attr('data-AtId');
            var atAuthor = $(this).attr('data-Author');
            var atAuthorInfo = '';
            if (atAuthor != '') {
                atAuthorInfo = '@' + atAuthor + ': ';
            }

            var btnComment, txtBody;
            if (formInner.length <= 0) {
                var clonedForm = $('#commentFormWrap').clone();
                rformWrap.append(clonedForm);

                txtBody = $('textarea[name=commentBody]', clonedForm);            
                btnComment = $('.btnComment', clonedForm);
            }
            else {
                txtBody = $('textarea[name=commentBody]', formInner);
                btnComment = $('.btnComment', formInner);
                formInner.show();
            }

            btnComment
                .after('<input name="parentId" type="hidden" value="' + parentId + '" />')
                .after('<input name="atId" type="hidden" value="' + atId + '" />')
                .after('<input name="atAuthorInfo" type="hidden" value="' + atAuthorInfo + '" />');

            txtBody.focus().val(atAuthorInfo);

            bindSubmitEvent(objX);

            return false;
        });

    }


    // “更多评论” 的视图逻辑处理
    function bindMoreLinkEvent( objX ) {

        // 是否有更多评论(初始状态)
        var isMoreLink = false;

        // 处理每条评论
        $('.replyCount').each(function () {

            // 显示子回复数量
            var replies = $(this).attr('data-ReplyCount');
            if (replies > 0) $(this).text('(共' + replies + '条回复)');

            if (replies <= objX.subCacheSize) return;

            // 显示更多评论按钮
            var moreCount = replies - objX.subCacheSize;
            var parentId = $(this).attr('data-ParentId');
            var startId = $(this).attr('data-StartId');
            isMoreLink = true;

            $('#' + $(this).attr('data-MoreId'))
                .attr('data-MoreLink', objX.moreLink )
                .attr('data-ParentId', parentId)
                .attr('data-StartId', startId)
                .attr('data-MoreCount', moreCount)
                .text('显示更多回复(还有' + moreCount + '条) ?');            
        });

        if (isMoreLink == false) return;

        // 绑定更多回复点击事件
        $('.moreLink').unbind('click').click(function () {

            var thisCmd = $(this);

            var moreLink = thisCmd.attr('data-MoreLink');
            var parentId = thisCmd.attr('data-ParentId');
            var startId = thisCmd.attr('data-StartId');
            var moreCount = thisCmd.attr('data-MoreCount');

            var replyWrap = $('#replyMoreWrap' + parentId);
            replyWrap.append('<div style="padding:10px 50px;" class="loadingInfoWrap">' + loadingInfo + '</div>');

            $.get(moreLink, { 'parentId': parentId, 'startId': startId }, function (data) {

                var newStartId = appendMore(replyWrap, data, parentId);
                var restCount = moreCount - data.length;

                if (restCount <= 0) {
                    thisCmd.hide();
                }
                else {
                    thisCmd.attr('data-StartId', newStartId)
                        .attr('data-MoreCount', restCount)
                        .text('显示更多回复(还有' + restCount + '条) ?');                    
                }

                $('.loadingInfoWrap', replyWrap).hide();

                bindSubReplyEvent(objX);

            });
        });
    }

    // 显示更多回复
    function appendMore(replyWrap, data, parentId) {

        var moreItems = '';
        var newStartId = 0;
        for (var i = 0; i < data.length; i++) {

            moreItems += '    <table class="commentItem"><tr>' +
                '<td class="cmUserFace avs">' + data[i].UserFace + '</td>' +
                '<td class="cmItemMain">' +
                '    <div><span class="strong">' + data[i].UserName + '</span></div>' +
                '   <div>' + data[i].Content + '</div>' +
                '   <div><span class="note">' + data[i].Created + '</span><span class="link lnkSubReply left5" data-ParentId="'+parentId+'" data-AtId="'+data[i].Id+'" data-Author="'+data[i].AuthorText+'">回复</span></div>' +
                '   <div class="clear"></div>' +
                '</td></tr>' +
                '</table>';

            newStartId = data[i].Id;
        }

        replyWrap.append(moreItems);

        return newStartId;
    }

    // 添加回复之后，局部刷新到页面
    function appendComment (cmContent, parentId, objX) {

        var item = '    <table class="commentItem"><tr>' +
            '<td class="cmUserFace avs">' + objX.userFace + '</td>' +
            '<td class="cmItemMain">' +
            '    <div><span class="strong">' + objX.userName + '</span></div>' +
            '   <div>' + cmContent + '</div>' +
            '   <div class="note">(刚刚发布) </div>' +
            '   <div class="clear"></div>' +
            '</td></tr>' +
            '</table>';

        if (parentId > 0) {
            $('#replyListLine' + parentId).prepend(item);
        }
        else {
            $('#commentListWrap').prepend(item);
        }
    };

    // loading 方法
    function loadBegin (ctxForm) {
        var btnSubmit = $('.btnComment', ctxForm);
        btnSubmit.attr('disabled', 'disabled').after(loadingInfo);
        return btnSubmit;
    };

    function loadEnd (btnSubmit) {
        $('.loadingInfo').html('');
        btnSubmit.attr('disabled', false);
    };

    // 提交评论到服务器
    var postComment = function (btnSubmit, objX) {

        var ctxForm = btnSubmit.parentsUntil('.commentPublishTable');

        var vCode = $('input[name=ValidationText]', ctxForm).val();
        var userName = $('input[name=UserName]', ctxForm).val();
        if (userName) objX.userName = userName;
        var userEmail = $('input[name=UserEmail]', ctxForm).val();

        var txtCommentBody = $('textarea[name=commentBody]', ctxForm);
        var cmContent = txtCommentBody.val();

        var atAuthorInfo = $('input[name=atAuthorInfo]', ctxForm).val();
        if (cmContent == atAuthorInfo) {
            alert('请填写内容');
            return;
        }

        var parentId = $('input[name=parentId]', ctxForm).val();
        var atId = $('input[name=atId]', ctxForm).val();

        var postData = {
            UserName: userName,
            UserEmail: userEmail,
            Content: cmContent,
            ValidationText: vCode,
            ParentId: parentId,
            AtId: atId,

            url: objX.thisUrl,
            dataTitle: objX.thisDataTitle,
            dataType: objX.thisDataType,
            dataUserId: objX.thisDataUserId,
            dataId: objX.thisDataId,
            appId: objX.thisAppId
        };

        loadBegin(ctxForm);

        $.post( objX.createLink.toAjax(), postData, function (data) {
            loadEnd(btnSubmit);
            if ('ok' == data) {
                appendComment(cmContent, parentId, objX);
                txtCommentBody.val('');
                addReplyCount();
            }
            else {
                alert(data.Msg);
            }
        });

    };

    // 验证码
    function showValidationImg() {
        var ctxForm = $(this).parentsUntil('.commentPublishTable');
        $('.validationRow', ctxForm).show();
    }

    // 提交评论
    function bindSubmitEvent( objX ) {
        $('.btnComment').unbind('click').click(function () { postComment($(this), objX); return false; });
        $('.commentBody').focus(showValidationImg);
    }

    // 绑定所有评论数量(在当前页和父页面)
    function bindReplyCount( replies ) {
        $('#replies').text( replies );
        $('#contentReplies', window.parent.document ).text( replies );
    }

    function addReplyCount() {
        var replies = parseInt( $('#replies').text() ); 
        bindReplyCount( replies + 1 );
    }

    function bindUserInfo( objX ) {
        $('.cmUserFaceInfo').html( objX.userFace );
    }


    //---------------------------------------------------------------------------------------------------------

    // 绑定提交事件；处理子评论列表；显示用户头像
    function bindCommentEvent( objX ) {

        bindUserInfo( objX );
        bindReplyCount( objX.replies );

        bindReplyEvent( objX );
        bindSubReplyEvent( objX );
        bindSubmitEvent( objX );
        bindMoreLinkEvent( objX );
    }

    return { bindEvent : bindCommentEvent };

});
