using System;

namespace HealthyHole.ViewModels
{
    public class ShiftView
    {  
        public DateTime ShiftStarts { get; set; }        
        public DateTime? ShiftEnds { get; set; }
        public int? Hours { get; set; }

    }
}
