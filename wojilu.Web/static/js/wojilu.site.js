wojilu.ui.slideToggleForm = function () {

    $(".fbCategoryPointer").each(function () {
        var self = $(this);
        self.click(function () {
            self.parent().parent().parent().find(".forumBoard").slideToggle("fast");
        });
    });
};