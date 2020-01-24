using System;
using System.Collections.Generic;

namespace jeanie.Areas.Admin.Models
{
    public class BlockedDatesViewModel
    {
        public List<(DateTime start, DateTime end)> AvailableSlots { get; set; }

        public List<(DateTime start, DateTime end)> BookedSlots { get; set; }
    }
}
