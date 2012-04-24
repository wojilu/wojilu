wojilu.ui.slideToggleForm = function () {
  
    $(".fbCategoryPointer").each(function () {
        var self = $(this);
        self.click(function () {
            self.parent().parent().parent().find(".forumBoard").slideToggle("fast");
        }).mouseover(function () {
            var $tip = $('<div id="tip"></div>');
            self.append($tip);
            $('#tip').show('fast');
        }).mouseout(function () {
            $('#tip').remove();
        }).mousemove(function (e) {
            $('#tip').css({ "top": (e.pageY - 60) + "px", "left": (e.pageX + 30) + "px" })     
    });

    });
}
