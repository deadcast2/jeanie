using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace jeanie.Models
{
    public class FormattedReservation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public string TimeSlot { get; set; }
        public string Status { get; set; }
        public string CreatedAt { get; set; }

        [NotMapped]
        public string Url => new ReservationViewModel { Id = Id }.Url;
    }
}
