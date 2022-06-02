using DataAccessLayer.DBContext;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DataAccess : IDataAccess, IShiftAccess
    {
        private Context context;
        public DataAccess(Context context)
        {
            this.context = context;
        }
        #region EmployeeMethods
        public async Task<T> AddEmployeeAsync<T>(T entity) where T : class
        {
            context.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<List<string>> GetAllTitlesAsync()
        {
            var result = await context.Employees.Select(m => m.Title).Distinct().ToListAsync();
            return result;
        }
        public EmployeeDAO UpdateEmployee(EmployeeDAO entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            var result = context.Employees.Find(entity.Id);
            return result;
        }
        public void DeleteEmployee(EmployeeDAO entity)
        {            
            context.Employees.Remove(entity);
            context.SaveChangesAsync(); 
        }
       

        public EmployeeDAO GetEmployee(int id)
        {
            var found = context.Employees.Find(id);
            return found;
        }

        public async Task<ICollection<EmployeeDAO>> GetEmployeesAsync()
        {
            ICollection<EmployeeDAO> result;
            result = await context.Employees.OrderBy(x=>x).ToListAsync();
            return result;
        }
        public IEnumerable<EmployeeDAO> GetEmployees(string title)
        {
            IQueryable<EmployeeDAO> group;            
            group = context.Employees.Where(x=>x.Title==title).Select(grp=>grp);
            var result = group.AsEnumerable();
            return result;
        }

        #endregion
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
            var result = context.Shifts.OrderBy(x=>x).Last(x=>x.EmployeeId == id);
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
