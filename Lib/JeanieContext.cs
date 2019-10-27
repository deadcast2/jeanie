namespace jeanie.Lib
{
    using jeanie.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class JeanieContext : DbContext
    {
        public JeanieContext()
            : base("name=JeanieContext")
        {
        }

        public virtual DbSet<Reservation> Reservations { get; set; }

        public virtual DbSet<BlockedDate> BlockedDates { get; set; }
    }
}
