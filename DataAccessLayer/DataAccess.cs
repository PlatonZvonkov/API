using DataAccessLayer.DBContext;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DataAccess : IDataAccess
    {
        private SQLiteContext context;
        public DataAccess(SQLiteContext context)
        {
            this.context = context;
        }
        /**
        * I added only one generic method "AddEmployeeAsync" just for demonstration, its easier and faster to hardcode others
        * Names of models end with DAO (data access object) for convinience to distinct them from modelson other layer
        */       
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
            if (found != null)
            {
                  context.Entry(found).State = EntityState.Detached;
            }          
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
    }
}
