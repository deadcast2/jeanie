using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jeanie.Models
{
    public class BlockedDate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Index("IX_StartDate_EndDate", IsUnique = true, Order = 0)]
        public DateTime StartDate { get; set; }

        [Index("IX_StartDate_EndDate", IsUnique = true, Order = 1)]
        public DateTime EndDate { get; set; }
    }
}
