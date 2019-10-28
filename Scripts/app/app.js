$(function () {
    new ClipboardJS('.copy');

    $('a.copy').click(function (e) {
        e.preventDefault();
        var $this = $(this);
        $this.tooltip('show');
        setTimeout(function () {
            $this.tooltip('hide');
        }, 1500);
    });

    $('[data-toggle="tooltip"]').tooltip({
        trigger: 'manual'
    });

    $('.delete-confirm').click(function (e) {
        e.preventDefault();
        if (confirm('Are you sure?')) {
            $(this).closest('form').submit();
        }
    });

    $('.pickadate').pickadate({
        format: 'mm/dd/yyyy',
        min: 3 // 72 Hours notice
    });

    $('.pickatime').pickatime({
        interval: 60,
        min: [9, 0],
        max: [21, 0]
    });
});
