window.JRS = window.JRS || {};
window.JRS.Reservation = {};

(function ($, self, undefined) {
    var previousDisabledDays = [];

    self.init = function (timeSlot) {
        var picker = $('.pickadate').change(function () {
            $.get('/timeslots/show?day=' + $(this).val(), function (timeSlots) {
                var $timeSlot = $('#time_slot');
                $timeSlot.empty();
                $.each(timeSlots, function (i, el) {
                    $timeSlot.append('<option value="' + el.value + '">' + el.text + '</option>');
                });

                if (timeSlot != '') {
                    $timeSlot.val(timeSlot);
                }
            });
        }).change().pickadate('picker');

        var disableDates = function (e) {
            if (e == null || e.highlight) {
                $.get('/disableddates/show?day=' + picker.get('view', 'yyyy/mm/dd'), function (disabled) {
                    picker.set('enable', previousDisabledDays);
                    previousDisabledDays = JSON.parse(disabled);
                    picker.set('disable', previousDisabledDays);
                });
            }
        };

        picker.on({
            open: disableDates,
            set: disableDates
        });
    };
})(jQuery, window.JRS.Reservation);
