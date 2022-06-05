using DataAccessLayer.DBContext;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ShiftAccess : IShiftAccess
    {
        private SQLiteContext context;
        public ShiftAccess(SQLiteContext context)
        {
            this.context = context;
        }
        #region Shifts
        public void AddNewShift(ShiftDAO shift)
        {
            context.Shifts.Add(shift);
            context.SaveChanges();
        }

        public void UpdateShift(ShiftDAO shift)
        {
            context.Shifts.Update(shift);
            context.SaveChanges();
        }

        public ShiftDAO GetShift(int id)
        {
            var result = context.Shifts.OrderBy(x => x).Last(x => x.EmployeeId == id);
            return result;
        }

        public async Task<ICollection<ShiftDAO>> GetAllShiftsAsync(int id)
        {
            var result = await context.Shifts.Where(x => x.EmployeeId == id).ToListAsync();
            return result;
        }        
        #endregion
    }
}
