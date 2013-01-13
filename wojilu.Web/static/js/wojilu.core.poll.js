define( [], function() {

	function initPoll(xTopicId, xVoteLink, xResultLink) {

		var ctxPoll = $('#pollWrap'+xTopicId);
		
		// 查看投票结果
		function bindViewResult() {
			$('.cmdViewResult',ctxPoll).click(function () {
				$('#pollFormWrap' + xTopicId).hide();
				$('#pollResultWrap' + xTopicId).show();
			});
		}

		// 进入投票表单
		function bindViewForm() {			
			$('.cmdViewForm',ctxPoll).unbind('click').click(function () {
				$('#pollFormWrap' + xTopicId).show();
				$('#pollResultWrap' + xTopicId).hide();
			});
		}
		
		bindViewResult();
		bindViewForm();

		function refreshResult() {
			var resultLink = xResultLink.toAjax();
			$.post(resultLink, function (data) {
				$('#pollResultWrap'+xTopicId).html(data);
				bindViewForm();
			});
		}

		$('.btnPoll', ctxPoll).click(function () {

			var pctx = $('#pollOption'+xTopicId);

			var selectedControl = $('input[name=pollOption]:checked', pctx);
			var pValue = selectedControl.serialize();
			if (wojilu.str.hasText(pValue) == false) {
				alert('请先选择');
				return false;
			}

			$(this).attr('disabled', 'disabled');
			$(this).val('你已经投过票');
			var voteLink = xVoteLink.toAjax();

			$.post(voteLink, pValue, function (data) {

				if ('ok' == data) {
					refreshResult();
					$('.cmdViewResult',ctxPoll ).click();
				} else {
					alert(data.Msg);
				}
			});
		});
	
	}
	
	return {
		init : initPoll
	};

});