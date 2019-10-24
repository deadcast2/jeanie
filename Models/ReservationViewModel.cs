using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jeanie.Models
{
    public class ReservationViewModel
    {
        public string Name { get; set; }

        public List<string> Errors { get; private set; } = new List<string>();

        public bool IsValid
        {
            get
            {
                Errors.Clear();

                if (string.IsNullOrWhiteSpace(Name))
                {
                    Errors.Add("Name cannot be blank");
                }

                return Errors.Count == 0;
            }
        }
    }
}