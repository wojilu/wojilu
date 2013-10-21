<div id="pollWrap#{topicId}">


    <div id="pollFormWrap#{topicId}" style="#{formStyle}">
        #{pollForm}      
    </div>

    <div id="pollResultWrap#{topicId}" style="#{resultStype}">
        #{pollResult}
    </div>

</div>

<script>
    _run(function () {

        require(['wojilu.core.poll'], function (x) {

            var topicId = #{topicId};
            var voteLink = '#{poll.VoteLink}';
            var resultLink = '#{resultLink}';

            x.init(topicId, voteLink,  resultLink);
        });
    });
</script>
