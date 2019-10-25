using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace jeanie.Models
{
    public class Reservation
    {
        public const int MaxNameLength = 100;
        public const int MaxGradeLength = 100;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, StringLength(MaxNameLength)]
        public string Name { get; set; }

        [StringLength(MaxGradeLength)]
        public string Grade { get; set; }

        public string Notes { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
