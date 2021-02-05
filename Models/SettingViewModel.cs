using System.Collections.Generic;
using System.Web.Mvc;

namespace jeanie.Models
{
    public class SettingViewModel : IValidatable
    {
        public int? DailyReservationLimit { get; set; }

        [AllowHtml]
        public string EmailTemplate { get; set; }

        public IList<string> Errors { get; set; } = new List<string>();

        public SettingViewModel() { }

        public SettingViewModel(Setting setting)
        {
            DailyReservationLimit = setting.DailyReservationLimit;
            EmailTemplate = setting.EmailTemplate;
        }

        public bool IsValid(bool partial = false)
        {
            Errors.Clear();

            if (DailyReservationLimit < 1)
            {
                Errors.Add("Daily reservation limit must be greater than 0");
            }

            return Errors.Count == 0;
        }
    }
}