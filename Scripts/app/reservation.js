window.JRS = window.JRS || {};
window.JRS.Reservation = {};

(function ($, self, undefined) {
    var confirmed = false;
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

        $('#reservation_form').submit(function (e) {
            if (!confirmed) {
                e.preventDefault();

                if (validate()) {
                    var tmpl = $.templates('#recap_template');
                    $('#recap').html(tmpl.render({
                        date: $('#date').val(),
                        time: $('#time_slot option:selected').text()
                    }));
                    $('#confirm_modal').modal('show');
                }
            }
        });

        $('#confirm').click(function () {
            confirmed = true;
            $('#reservation_form').submit();
        });
    };

    var validate = function () {
        check($('#date'));
        check($('#time_slot'));
        check($('#grade'));
        return $('.has-error').length === 0;
    };

    var check = function ($el) {
        $el.closest('.form-group').toggleClass('has-error', $el.val() == '');
    };
})(jQuery, window.JRS.Reservation);
