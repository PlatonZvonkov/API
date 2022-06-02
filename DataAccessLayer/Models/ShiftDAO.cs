using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class ShiftDAO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime ShiftStarts { get; set; }
        
        public DateTime ShiftEnds { get; set; }
        public int Hours { get; set; }

        public int EmployeeId { get; set; }
        
        public EmployeeDAO Employee { get; set; }

    }
}
