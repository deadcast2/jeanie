window.JRS = window.JRS || {};
window.JRS.Admin = window.JRS.Admin || {};
window.JRS.Admin.Reservation = {};

(function ($, self, undefined) {
    self.init = function () {
        $('#reservations_grid').DataTable({
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
            dom: 'ft<"pull-left"l>p',
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
    };
})(jQuery, window.JRS.Admin.Reservation);
