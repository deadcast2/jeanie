using System.Collections.Generic;
using System.Web.Mvc;

namespace jeanie.Models
{
    public class SettingViewModel : IValidatable
    {
        public int? DailyReservationLimit { get; set; }

        public string EmailTemplateSubject { get; set; }

        [AllowHtml]
        public string EmailTemplateBody { get; set; }

        public IList<string> Errors { get; set; } = new List<string>();

        public SettingViewModel() { }

        public SettingViewModel(Setting setting)
        {
            DailyReservationLimit = setting.DailyReservationLimit;
            EmailTemplateSubject = setting.EmailTemplateSubject;
            EmailTemplateBody = setting.EmailTemplateBody;
        }

        public bool IsValid(bool partial = false)
        {
            Errors.Clear();

            if (DailyReservationLimit < 1)
            {
                Errors.Add("Daily reservation limit must be greater than 0.");
            }

            if ((EmailTemplateSubject ?? "").Length > Setting.MaxEmailTemplateSubjectLength)
            {
                Errors.Add($"Subject must be less than {Setting.MaxEmailTemplateSubjectLength + 1} characters.");
            }

            return Errors.Count == 0;
        }
    }
}