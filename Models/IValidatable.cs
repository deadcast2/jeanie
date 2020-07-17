using System.Collections.Generic;

namespace jeanie.Models
{
    public interface IValidatable
    {
        IList<string> Errors { get; set; }
        bool IsValid(bool partial = false);
    }
}
