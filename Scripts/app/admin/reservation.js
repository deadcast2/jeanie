window.JRS = window.JRS || {};
window.JRS.Admin = window.JRS.Admin || {};
window.JRS.Admin.Reservation = {};

(function ($, self, undefined) {
    self.init = function () {
        $('#reservations_grid').DataTable({
            processing: true,
            order: [[4, "desc"]],
            columnDefs: [{
                "targets": 'no-sort',
                "orderable": false
            }],
            columns: [
                { name: 'Name' },
                { name: 'Grade' },
                { name: 'TimeSlot' },
                { name: 'Status' },
                { name: 'CreatedAt' },
                { name: 'Actions' }
            ],
            dom: 'ft<"pull-left"l>pr',
            serverSide: true,
            ajax: {
                url: '/admin/reservations/read',
                type: 'POST'
            },
            createdRow: function (row, data, dataIndex) {
                if (data[3] === 'Confirmed') {
                    $(row).addClass('success');
                } else if (data[3] === 'Cancelled') {
                    $(row).addClass('danger');
                }
            }
        }).on('draw', function () {
            $('[data-toggle="tooltip"]').tooltip({
                trigger: 'manual'
            });
        });

        $('#email_template').summernote({ height: 300 });

        $('#email_modal').on('show.bs.modal', function (e) {
            var id = $(e.relatedTarget).data('id');

            $.get({ cache: false, url: $(this).data('path') + '?id=' + id }, function (response) {
                $('#email_template').summernote('code', response);
            });
        });
    };
})(jQuery, window.JRS.Admin.Reservation);
