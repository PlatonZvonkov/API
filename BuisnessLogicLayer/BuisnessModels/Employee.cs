using System.Collections.Generic;

namespace BuisnessLogicLayer.Models
{
    public class Employee
    {  
        public int Id { get; set; }        
        public string Surname { get; set; }     
        public string Name { get; set; }     
        public string Fatherhood { get; set; }     
        public string Title { get; set; }
        public int Strikes { get; set; }
        public List<Shift> Shifts { get; set; }

    }
}
