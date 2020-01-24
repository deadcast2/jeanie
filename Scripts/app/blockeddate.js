window.JRS = window.JRS || {};
window.JRS.BlockedDate = {};

(function ($, self, undefined) {
    var calendar;
    var blockedDates;

    self.init = function () {
        loadBlockedDates(loadCalendar);

        $('#time_slots_modal').on('click', 'button.time-slot', function () {
            var start = $(this).data('start');
            $.post($(this).closest('form').attr('action'), {
                Start: start,
                End: $(this).data('end'),
                Action: $(this).data('action')
            }, function () {
                showTimeSlotModal(new Date(start));
                loadBlockedDates(function () { calendar.render(); });
            });
        });

        $('#time_slots_modal').on('click', '#block_all', function () {
            $.post($(this).data('path'), function () {
                loadBlockedDates(function () { calendar.render(); });
                $('#time_slots_modal').modal('hide');
            });
        });
    };

    var loadBlockedDates = function (onComplete) {
        $.get('/blockeddates/dates', function (dates) {
            blockedDates = {};
            $.each(dates, function (i, date) {
                blockedDates[new Date(date.Date).getTime()] = date.IsDayFullyBooked
                    ? 'fully-blocked-date' : 'partially-blocked-date';
            });
            if (typeof onComplete === "function") {
                onComplete();
            }
        });
    };

    var showTimeSlotModal = function (date) {
        $.get($('#time_slots_modal').data('path') + '?date=' + date.toLocaleDateString(), function (response) {
            $('#time_slots').html(response);
        });
        $('#time_slots_modal .selected-date').text(date.toLocaleDateString());
        $('#time_slots_modal').modal('show');
    };

    var loadCalendar = function () {
        calendar = new FullCalendar.Calendar($('#calendar').get(0), {
            plugins: ['dayGrid', 'interaction'],
            aspectRatio: 2,
            dateClick: function (info) {
                showTimeSlotModal(info.date);
            },
            dayRender: function (info) {
                var time = info.date.getTime();
                if (blockedDates.hasOwnProperty(time)) {
                    $(info.el).addClass(blockedDates[time]);
                }
            }
        });
        calendar.render();
    };
})(jQuery, window.JRS.BlockedDate);
