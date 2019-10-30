using jeanie.Lib;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace jeanie.Models
{
    public class ReservationViewModel
    {
        private static PhoneNumberUtil PhoneUtil = null;

        public ReservationViewModel() 
        {
            PhoneUtil = PhoneNumberUtil.GetInstance();
        }

        public ReservationViewModel(Reservation reservation) : base()
        {
            Id = reservation.Id;
            Name = reservation.Name;
            Email = reservation.Email;
            PhoneNumber = reservation.PhoneNumber;
            Grade = reservation.Grade;
            LicensePlateNumber = reservation.LicensePlateNumber;
            MakeAndModel = reservation.MakeAndModel;
            Notes = reservation.Notes;
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
            Status = reservation.Status;
            Source = reservation;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FormattedPhoneNumber => 
            ParsedPhoneNumber == null ? null : PhoneUtil.Format(ParsedPhoneNumber, PhoneNumberFormat.NATIONAL);
        private PhoneNumber ParsedPhoneNumber =>
            string.IsNullOrWhiteSpace(PhoneNumber) ? null : PhoneUtil.ParseAndKeepRawInput(PhoneNumber, "US");
        public string Grade { get; set; }
        public string LicensePlateNumber { get; set; }
        public string MakeAndModel { get; set; }
        public string Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? EndTime { get; set; }
        public string TimeSlot { get; set; }
        public Reservation Source { get; private set; }

        public DateTime? Date
        {
            set
            {
                StartDate = EndDate = value;
                StartDate = StartDate?.Date.AddHours(StartTime?.Hour ?? 0).AddMinutes(StartTime?.Minute ?? 0);
                EndDate = EndDate?.Date.AddHours(EndTime?.Hour ?? 0).AddMinutes(EndTime?.Minute ?? 0);
            }
            get
            {
                return StartDate?.Date;
            }
        }

        public ReservationStatus Status { get; private set; }

        public string StatusText => Status.Text();

        public string StatusClass
        {
            get
            {
                switch (Status)
                {
                    case ReservationStatus.Confirmed: return "success";
                    case ReservationStatus.Cancelled: return "danger";
                    default: return "";
                }
            }
        }

        public DateTime? StartDateFromTimeSlot => Date?.AddHours(SplitTimeSlot[0]);

        public DateTime? EndDateFromTimeSlot => Date?.AddHours(SplitTimeSlot[1]);

        public string FormattedTimeSlot =>
            Status >= ReservationStatus.Complete ? $"{StartDate.Value.ToShortDateString()} " +
            $"{StartDate.Value.ToShortTimeString()} - {EndDate.Value.ToShortTimeString()}" : "TBD";

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
                if (!new EmailAddressAttribute().IsValid(Email)) Errors.Add("A valid email must be specified.");
                if (ParsedPhoneNumber == null || !PhoneUtil.IsValidNumberForRegion(ParsedPhoneNumber, "US")) 
                    Errors.Add("A valid phone # must be specified.");
                if (string.IsNullOrWhiteSpace(Grade)) Errors.Add("A grade must be specified.");
                if (string.IsNullOrWhiteSpace(LicensePlateNumber)) Errors.Add("A license plate # must be specified.");
                if (string.IsNullOrWhiteSpace(MakeAndModel)) Errors.Add("A make & model must be specified.");

                // Range
                if (Date.HasValue && !ReservationHelper.IsValidDate(Date.Value))
                    Errors.Add($"The date selected must be {ReservationHelper.HoursInAdvance} hours in advance.");

                // Length
                if (Name.Length > Reservation.MaxNameLength)
                    Errors.Add($"Name must be less than {Reservation.MaxNameLength} characters.");
                if ((Email ?? "").Length > Reservation.MaxEmailLength)
                    Errors.Add($"Email must be less than {Reservation.MaxEmailLength} characters.");
                if ((PhoneNumber ?? "").Length > Reservation.MaxPhoneNumberLength)
                    Errors.Add($"Phone # must be less than {Reservation.MaxPhoneNumberLength} characters.");
                if ((Grade ?? "").Length > Reservation.MaxGradeLength)
                    Errors.Add($"Grade must be less than {Reservation.MaxGradeLength} characters.");
                if ((LicensePlateNumber ?? "").Length > Reservation.MaxLicensePlateNumberLength)
                    Errors.Add($"License plate # must be less than {Reservation.MaxLicensePlateNumberLength} characters.");
                if ((MakeAndModel ?? "").Length > Reservation.MaxMakeAndModelLength)
                    Errors.Add($"Make & model must be less than {Reservation.MaxMakeAndModelLength} characters.");
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

        private double[] SplitTimeSlot
        {
            get
            {
                var hourRange = (TimeSlot ?? "").Split('-');
                if (hourRange.Length != 2) return new[] { 0.0, 0.0 };

                double.TryParse(hourRange[0], out var startHour);
                double.TryParse(hourRange[1], out var endHour);
                return new[] { startHour, endHour };
            }
        }
    }
}
