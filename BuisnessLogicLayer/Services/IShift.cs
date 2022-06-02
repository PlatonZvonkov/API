using BuisnessLogicLayer.Models;

namespace BuisnessLogicLayer.Services
{
    public interface IShift
    {
        public string ShiftStarts(Employee item);
        public string ShiftEnds(Employee item);        
        public Shift GetShift(int id);
    }
}
