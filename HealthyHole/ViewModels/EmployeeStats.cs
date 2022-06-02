using BuisnessLogicLayer.Models;
using System.Collections.Generic;


namespace HealthyHole.ViewModels
{
    public class EmployeeStats
    {
        public string FullName { get; set; }
        public ICollection<Shift> Shifts { get; set; }
    }
}
