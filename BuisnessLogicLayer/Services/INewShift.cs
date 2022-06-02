using BuisnessLogicLayer.Models;

namespace BuisnessLogicLayer.Services
{
    public interface INewShift
    {
        public void ShiftStarts(int id);
        public void ShiftEnds(int id);        
        public Shift GetShift(int id);
    }
}
