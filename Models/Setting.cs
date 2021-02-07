using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jeanie.Models
{
    public class Setting
    {
        public const int MaxEmailTemplateSubjectLength = 255;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int? DailyReservationLimit { get; set; }

        [StringLength(MaxEmailTemplateSubjectLength)]
        public string EmailTemplateSubject { get; set; }

        [MaxLength]
        public string EmailTemplateBody { get; set; }
    }
}
