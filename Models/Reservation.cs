using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jeanie.Models
{
    public enum ReservationStatus { Uncomplete, Complete, ReminderSent, Confirmed, Cancelled }

    public class Reservation
    {
        public const int MaxNameLength = 100;
        public const int MaxEmailLength = 100;
        public const int MaxGradeLength = 100;
        public const int MaxNotesLength = 1000;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, StringLength(MaxNameLength)]
        public string Name { get; set; }

        [StringLength(MaxEmailLength)]
        public string Email { get; set; }

        [StringLength(MaxGradeLength)]
        public string Grade { get; set; }

        [StringLength(MaxNotesLength)]
        public string Notes { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
