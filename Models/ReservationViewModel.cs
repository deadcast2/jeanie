using jeanie.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace jeanie.Models
{
    public class ReservationViewModel
    {
        public ReservationViewModel() { }

        public ReservationViewModel(Reservation reservation)
        {
            Id = reservation.Id;
            Name = reservation.Name;
            Email = reservation.Email;
            Grade = reservation.Grade;
            Notes = reservation.Notes;
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
            Status = reservation.Status;
            Source = reservation;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Grade { get; set; }

        public string Notes { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? Date
        {
            set
            {
                StartDate = EndDate = value;
                StartDate = StartDate?.Date.AddHours(StartTime?.Hour ?? 0);
                EndDate = EndDate?.Date.AddHours(EndTime?.Hour ?? 0);
            }
            get
            {
                return StartDate?.Date;
            }
        }

        public ReservationStatus Status { get; private set; }

        public string StatusText => Status.Text();

        public string TimeSlot { get; set; }

        public DateTime? StartDateFromTimeSlot => Date?.AddHours(SplitTimeSlot[0]);

        public DateTime? EndDateFromTimeSlot => Date?.AddHours(SplitTimeSlot[1]);

        public Reservation Source { get; private set; }

        public string FormattedTimeSlot => 
            Status >= ReservationStatus.Complete ? $"{StartDate.Value.ToLocalTime().ToShortDateString()} " +
            $"{StartDate.Value.ToLocalTime().ToShortTimeString()} - " +
            $"{EndDate.Value.ToLocalTime().ToShortTimeString()}" : "TBD";

        public string Url => $"{BaseUrl}/Reservations/Edit/{Id}";

        public List<string> Errors { get; private set; } = new List<string>();

        public bool IsValid(bool strict = false)
        {
            Errors.Clear();

            if (string.IsNullOrWhiteSpace(Name))
            {
                Errors.Add("Name cannot be blank.");
            }

            if (strict)
            {
                // Presence
                if (!Date.HasValue) Errors.Add("A date must be selected.");
                if (string.IsNullOrWhiteSpace(TimeSlot)) Errors.Add("A time slot must be selected.");
                if (string.IsNullOrWhiteSpace(Grade)) Errors.Add("A grade must be specified.");
                if (!new EmailAddressAttribute().IsValid(Email)) Errors.Add("A valid email must be specified.");

                // Range
                if (Date.HasValue && !ReservationHelper.IsValidDate(Date.Value))
                    Errors.Add($"The date selected must be {ReservationHelper.HoursInAdvance} hours in advance.");

                // Length
                if (Name.Length > Reservation.MaxNameLength)
                    Errors.Add($"Name must be less than {Reservation.MaxNameLength} characters.");
                if ((Email ?? "").Length > Reservation.MaxEmailLength)
                    Errors.Add($"Email must be less than {Reservation.MaxEmailLength} characters.");
                if ((Grade ?? "").Length > Reservation.MaxGradeLength)
                    Errors.Add($"Grade must be less than {Reservation.MaxGradeLength} characters.");
                if ((Notes ?? "").Length > Reservation.MaxNotesLength)
                    Errors.Add($"Notes must be less than {Reservation.MaxNotesLength} characters.");
            }

            return Errors.Count == 0;
        }

        public bool IsComplete => Status == ReservationStatus.Complete;

        public bool IsConfirmed => Status == ReservationStatus.Confirmed;

        public bool IsCancelled => Status == ReservationStatus.Cancelled;

        public string BaseUrl
        {
            get
            {
#if DEBUG
                return ConfigurationManager.AppSettings["DevBaseUrl"];
#else
                return ConfigurationManager.AppSettings["ProdBaseUrl"];
#endif
            }
        }

        private int[] SplitTimeSlot
        {
            get
            {
                var hourRange = (TimeSlot ?? "").Split('-');
                if (hourRange.Length != 2) return new[] { 0, 0 };

                int.TryParse(hourRange[0], out int startHour);
                int.TryParse(hourRange[1], out int endHour);
                return new[] { startHour, endHour };
            }
        }
    }
}
