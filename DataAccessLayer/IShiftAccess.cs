using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IShiftAccess
    {
        ShiftDAO GetShift(int id);
        void AddNewShift(ShiftDAO shift);
        void UpdateShift(ShiftDAO shift);
        Task<ICollection<ShiftDAO>> GetAllShiftsAsync(int id);
    }
}
