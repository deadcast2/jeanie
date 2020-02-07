window.JRS = window.JRS || {};
window.JRS.Admin = window.JRS.Admin || {};
window.JRS.Admin.Reservation = {};

(function ($, self, undefined) {
    self.init = function () {
        $('#reservations_grid').DataTable();
    };
})(jQuery, window.JRS.Admin.Reservation);
