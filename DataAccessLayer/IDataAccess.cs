using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IDataAccess
    {        
        Task<T> AddEmployeeAsync<T>(T entity)where T : class;
        EmployeeDAO UpdateEmployee(EmployeeDAO entity);
        EmployeeDAO GetEmployee(int id);
        Task<ICollection<EmployeeDAO>> GetEmployeesAsync();
        IEnumerable<EmployeeDAO> GetEmployees(string title);
        Task<List<string>> GetAllTitlesAsync();
        void DeleteEmployee(EmployeeDAO entity);
    }
}