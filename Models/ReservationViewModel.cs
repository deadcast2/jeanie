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

        public DateTime? EndDate { get; set; }

        public string TimeSlot { get; set; }

        public string Url => $"{BaseUrl}/Reservations/Edit/{Id}";

        public List<string> Errors { get; private set; } = new List<string>();

        public bool IsValid
        {
            get
            {
                Errors.Clear();

                if (string.IsNullOrWhiteSpace(Name))
                {
                    Errors.Add("Name cannot be blank");
                }

                return Errors.Count == 0;
            }
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