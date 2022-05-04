$(function () {
    new ClipboardJS('.copy');

    $('.container').on('click', 'a.copy', function (e) {
        e.preventDefault();
        var $this = $(this);
        $this.tooltip('show');
        setTimeout(function () {
            $this.tooltip('hide');
        }, 1500);
    });

    $('.container').on('click', '.delete-confirm', function (e) {
        e.preventDefault();
        if (confirm('Are you sure?')) {
            $(this).closest('form').submit();
        }
    });

    $('.pickadate').pickadate({
        format: 'mm/dd/yyyy',
        min: 3, // 72 Hours notice,
        max: '07/31/2022',
        today: ''
    });

    $('.pickatime').pickatime({
        interval: 30,
        min: [9, 0],
        max: [21, 0]
    });
});
