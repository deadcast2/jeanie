namespace jeanie.Lib
{
    using jeanie.Models;
    using System.Data.Entity;

    public class JeanieContext : DbContext
    {
        public JeanieContext() : base("name=JeanieContext") { }

        public virtual DbSet<Reservation> Reservations { get; set; }

        public virtual DbSet<FormattedReservation> FormattedReservations { get; set; }

        public virtual DbSet<BlockedDate> BlockedDates { get; set; }
    }
}
