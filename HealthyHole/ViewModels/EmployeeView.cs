using BuisnessLogicLayer.Models;
using System.Collections.Generic;

namespace HealthyHole.ViewModels
{
    public class EmployeeView
    {  
        public int Id { get; set; }        
        public string Surname { get; set; }     
        public string Name { get; set; }     
        public string Fatherhood { get; set; }     
        public string Title { get; set; }
        public int Strikes { get; set; }        
    }
}
