var Spinner = function (selector) {
    var value = '';
    Spinner.prototype.show = function () {
        var sel = $(selector);
        sel.addClass("spinner");
        value = sel.val();
        sel.val('');
        sel.attr('disabled', 'disabled');
    }
    Spinner.prototype.hide = function () {
        var sel = $(selector);
        sel.removeClass("spinner");
        sel.removeAttr('disabled');
        if (value != '' || value != undefined)
            sel.val(value);
    }
}