$(function () {
    $('a.copy').click(function (e) {
        e.preventDefault();
        var $this = $(this);
        navigator.clipboard.writeText($this.attr('href')).then(function () {
            $this.tooltip('show');
            setTimeout(function () {
                $this.tooltip('hide');
            }, 1500);
        });
    });

    $('[data-toggle="tooltip"]').tooltip({
        trigger: 'manual'
    });

    $('.pickadate').pickadate();
});
