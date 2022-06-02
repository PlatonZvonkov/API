using BuisnessLogicLayer.Models;

namespace BuisnessLogicLayer.Services
{
    public interface INewShift
    {
        public string ShiftStarts(Employee item);
        public string ShiftEnds(Employee item);        
        public Shift GetShift(int id);
    }
}
