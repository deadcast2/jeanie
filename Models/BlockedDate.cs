using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jeanie.Models
{
    public class BlockedDate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Index(IsUnique = true)]
        public DateTime Date { get; set; }
    }
}
