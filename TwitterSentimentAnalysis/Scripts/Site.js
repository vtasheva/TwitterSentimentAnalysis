$(function () {
    $('#searchForm').submit(function (e) {
        if ($(this).valid()) {
            $('.validation-summary-errors').remove();
        }
    });
});