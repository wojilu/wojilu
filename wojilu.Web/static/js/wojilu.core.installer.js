require(['lib/jquery.easing.1.3'], function () {

    var slidePageNext = function (me, callback) {
        $(".step").each(function () {
            var nleft = $(this).position().left - 800;
            $(this).animate({ left: nleft }, 750, 'easeInOutExpo', callback);
        });
        $('.stepTitleItem').removeClass('stepTitleItemCurrent');
        var nextId = parseInt( $(me).attr('data-id') )+1;
        $('#stepTitle'+nextId).addClass('stepTitleItemCurrent');
    };

    var slidePagePrev = function () {
        $(".step").each(function () {
            var nleft = $(this).position().left + 800;
            $(this).animate({ left: nleft }, 750, 'easeInOutExpo');
        });
        $('.stepTitleItem').removeClass('stepTitleItemCurrent');
        var prevId = parseInt( $(this).attr('data-id') )-1;
        $('#stepTitle'+prevId).addClass('stepTitleItemCurrent');
    };
    
    $('.btnPrev').click(slidePagePrev);

    // 第一步处理：设置数据库
    //-----------------------------------------------------------------------------------------------------
    $('#btnStep1').click(function () {
        var me = this;
        
        var dbType = $('input[name=dbType]:checked').val();
        var dbName;
        var connectionStr;

        if (dbType == 'access') {
            dbName = $('input[name=accessName]').val();
            if (!dbName) {
                alert('请填写数据库名称'); $('input[name=accessName]').focus(); return;
            }
        }
        else if (dbType == 'sqlserver' || dbType == 'sqlserver2000' ) {
            connectionStr = $('#sqlStr').val();
            if( !connectionStr ) {
                alert('请填写数据库连接字符串');$('#sqlStr').focus(); return;
            }
        }
        else if( dbType == 'mysql' ) {
            connectionStr = $('#mysqlStr').val();
            if( !connectionStr ) {
                alert('请填写数据库连接字符串');$('#mysqlStr').focus(); return;
            }
        }
        else {
            alert('请选择数据库类型 或 点击"直接进入下一步"'); return;
        }

        var postData = {
            dbType: dbType,
            dbName: dbName,
            connectionStr: connectionStr
        };
        
        var btn = $(this);
        btn.attr('disabled', true );
        $('#loadingDb').css('visibility','visible');
        $.post(setConfigLink.toAjax(), postData, function (data) {
            btn.attr('disabled', false );
            $('#loadingDb').css('visibility','hidden');
            if ('ok' == data) {
                slidePageNext(me, function() {$('#adminName').focus();});
            }
            else {
                alert(data);
            }
        });

    });
    
    // 第二步处理：创建用户
    //-----------------------------------------------------------------------------------------------------
    $('#btnStep2').click(function () {
        var me = this;
        var adminName = $('#adminName').val();
        var adminEmail = $('#adminEmail').val();
        var adminPwd = $('#adminPwd').val();
        var adminPwd2 = $('#adminPwd2').val();
        var adminUrl = $('#adminUrl').val();

        window.objUser = {
            name: adminName,
            email: adminEmail,
            pwd : adminPwd,
            url : adminUrl
        };

        // 管理员具有至高权限，所以随便填写什么都可以，不做格式限制。
        if (!adminName) {
            alert('请填写用户名');
            $('#adminName').focus();
            return false;
        }
        
        if( adminUrl && adminUrl.length>0 ) {
            var isError = adminUrl.search(/^[a-zA-Z]{1}([a-zA-Z0-9]|[_]){2,19}$/ )==-1;
            if( isError ) {
                alert( '个性网址只能填写英文和数字，英文开头，3-20个字符' );
                $('#adminUrl').focus();
                return false;
            }
        }
        
        if (!adminEmail) {
            alert('请填写email');
            $('#adminEmail').focus();
            return false;
        }

        if (!adminPwd) {
            alert('请填写密码');
            $('#adminPwd').focus();
            return false;
        }

        if (!adminPwd2) {
            alert('请重复填写密码');
            $('#adminPwd2').focus();
            return false;
        }

        if (adminPwd != adminPwd2) {
            alert('两次密码不一致');
            $('#adminPwd2').focus();
            return false;
        }
        
     

        slidePageNext(me);
    });
    
    // 第三步处理：选择网站类型
    //-----------------------------------------------------------------------------------------------------
    $('#btnStep3').click(function () {
        var me = this;
        window.objUser.siteType = $('input[name=siteType]:checked').val();
        window.objUser.siteName = $('input[name=siteName]').val();

        if( confirm( '确认提交？' )==false ) {
            return false;
        }
        
        $('#initInfo').show();
        
        var btn = $(this);
        btn.attr('disabled', true );
        var btnPrev = btn.prev();
        btnPrev.attr('disabled', true );        

        $.post(setUserLink.toAjax(), window.objUser, function (data) {
            $('#initInfo').hide();
            btn.attr('disabled', false );
            btnPrev.attr('disabled', false );
            if( data=='ok' ) {
                slidePageNext(me);
            }
            else {
                alert( data );
            }
        });
    });

    // 第四步处理：安装完成
    //-----------------------------------------------------------------------------------------------------
    $('#btnDone').click(function () {
        $.post(doneLink.toAjax(), function (data) {
            if ('ok' == data) {
                window.location.href = '/';
            }
            else {
                alert('发生错误=' + data);
            }
        });
    });
    
    //--------------------------数据库类型-----------------------------------------

    $('#lnkNext').click(function () {
        $('#rdoAccess').click();
        $('#btnStep1').click();
        return false;
    });
    
    $('#rdoAccess').click(function () {
        $('.accessInfo').slideDown();
        $('.sqlserverInfo').slideUp();
        $('.mysqlInfo').slideUp();
    });

    $('#rdoSqlServer').click(function () {
        $('.accessInfo').slideUp();
        $('.sqlserverInfo').hide();
        $(this).parent().parent().after($('#sqlserverInfo'));
        $('.sqlserverInfo').slideDown();
        $('.mysqlInfo').slideUp();
    });

    $('#rdoSqlServer2000').click(function () {
        $('.accessInfo').slideUp();
        $('.sqlserverInfo').hide();
        $(this).parent().parent().after($('#sqlserverInfo'));
        $('.sqlserverInfo').slideDown();
        $('.mysqlInfo').slideUp();
    });

    $('#rdoMysql').click(function () {
        $('.accessInfo').slideUp();
        $('.sqlserverInfo').slideUp();
        $('.mysqlInfo').slideDown();
    });

});