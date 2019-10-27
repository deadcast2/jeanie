window.JRS = window.JRS || {};
window.JRS.BlockedDate = {};

(function ($, self, undefined) {
    var blockedDates = [];

    self.init = function () {
        loadBlockedDates(loadCalendar);
    };

    var loadBlockedDates = function (onComplete) {
        $.get('/blockeddates/dates', function (dates) {
            blockedDates = $.map(dates, function (date) {
                return new Date(date).getTime();
            });

            if (typeof onComplete === "function") {
                onComplete();
            }
        });
    };

    var loadCalendar = function () {
        new FullCalendar.Calendar($('#calendar').get(0), {
            plugins: ['dayGrid', 'interaction'],
            aspectRatio: 2,
            dateClick: function (info) {
                $.post('/blockeddates/update?date=' + info.date.toLocaleDateString(), function (response) {
                    $(info.dayEl).toggleClass('blocked-date', response.enabled);
                    loadBlockedDates();
                });
            },
            dayRender: function (info) {
                if (blockedDates.indexOf(info.date.getTime()) > -1) {
                    $(info.el).addClass('blocked-date');
                }
            }
        }).render();
    };
})(jQuery, window.JRS.BlockedDate);
