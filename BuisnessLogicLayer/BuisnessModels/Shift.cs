using System;

namespace BuisnessLogicLayer.Models
{
    public class Shift
    {        
        public int? Id { get; set; }        
        public DateTime ShiftStarts { get; set; }        
        public DateTime? ShiftEnds { get; set; }
        public int? Hours { get; set; }
        public int EmployeeId { get; set; } 

    }
}
