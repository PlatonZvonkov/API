using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class EmployeeDAO
    {        
        [Key]
        public int Id { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Fatherhood { get; set; }
        [Required]
        public string Title { get; set; }
        public int Strikes { get; set; }
        public List<ShiftDAO> Shifts { get; set; }

    }
}
