using jeanie.Lib;
using System;
using System.Collections.Generic;
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
            Grade = reservation.Grade;
            Notes = reservation.Notes;
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

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

        public string TimeSlot { get; set; }

        public string FormattedTimeSlot => IsBooked ? $"{StartDate.Value.ToShortDateString()} " +
            $"{StartDate.Value.ToShortTimeString()} - {EndDate.Value.ToShortTimeString()}" : "TBD";

        public string Url => $"{BaseUrl}/Reservations/Edit/{Id}";

        public List<string> Errors { get; private set; } = new List<string>();

        public bool IsValid(bool strict = false)
        {
            Errors.Clear();

            if (string.IsNullOrWhiteSpace(Name))
            {
                Errors.Add("Name cannot be blank");
            }

            if (strict)
            {
                if (!Date.HasValue) Errors.Add("A date must be select");
                if (string.IsNullOrWhiteSpace(TimeSlot)) Errors.Add("A time slot must be select");
                if (string.IsNullOrWhiteSpace(Grade)) Errors.Add("A grade must be specified");
            }

            return Errors.Count == 0;
        }

        public bool IsBooked => StartDate.HasValue && EndDate.HasValue;

        private string BaseUrl
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
    }
}