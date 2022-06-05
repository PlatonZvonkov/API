using BuisnessLogicLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuisnessLogicLayer.Services
{
    public interface IShift
    {
        public string ShiftStarts(Employee item);
        public string ShiftEnds(Employee item);        
        public Shift GetShift(int id);
        public Task<ICollection<Shift>> GetAllShiftsAsync(int id);
    }
}
