using AutoMapper;
using BuisnessLogicLayer.Models;
using DataAccessLayer;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BuisnessLogicLayer.Services
{
    public class HumanResourcesService : IHumanResources
    {
        private readonly IDataAccess dataAccess;
        private readonly IShiftAccess shift;
        MapperConfiguration config; 
        IMapper _mapper;
        /**
        * Names of models end with DAO (data access object) for convinience to distinct them from models on this layer
        * and mapping convinience
        */
        public HumanResourcesService(IDataAccess dataAccess, IShiftAccess shift)
        {
            this.dataAccess = dataAccess;
            this.shift = shift;
            config = new MapperConfiguration(cfg =>
            { 
                cfg.CreateMap<Employee, EmployeeDAO>().ForMember(x=>x.Shifts, opt => opt.Ignore());
                cfg.CreateMap<EmployeeDAO, Employee>().ForMember(x=>x.Shifts, opt => opt.Ignore());

                cfg.CreateMap<Shift, ShiftDAO>().ForMember(x => x.Employee, opt => opt.Ignore());
                cfg.CreateMap<ShiftDAO, Shift>();
            }
            );
            _mapper = new Mapper(config);
        }
        #region CRUD
        public async Task<Employee> AddEmployeeAsync(Employee item)
        {            
            var employeeDao = _mapper.Map<EmployeeDAO>(item);
            var result = await dataAccess.AddEmployeeAsync(employeeDao);
            return _mapper.Map<Employee>(result);
        }

        public async Task<List<string>> GetAllTitlesAsync()
        {
            var result = await dataAccess.GetAllTitlesAsync();
            return result;
        }

        public Employee GetEmployee(int id)
        {
            var employee = dataAccess.GetEmployee(id);
            var shiftsById = shift.GetAllShiftsAsync(id).Result;            
            var result = _mapper.Map<Employee>(employee);
            result.Shifts = _mapper.Map<List<ShiftDAO>, List<Shift>>(shiftsById.ToList());
            return result;
        }

        public async Task<ICollection<Employee>> GetEmployeesAsync(string title)
        {
            IEnumerable<EmployeeDAO> option1;
            if (title != null)
            {
                option1 = dataAccess.GetEmployees(title);
                return (ICollection<Employee>)_mapper.Map<IEnumerable<EmployeeDAO>,IEnumerable<Employee>>(option1);
            }
            var option2 = await dataAccess.GetEmployeesAsync();
            return _mapper.Map<ICollection<EmployeeDAO>,ICollection<Employee>>(option2);
        }

        public void RemoveEmployee(int id)
        {
            var employee = dataAccess.GetEmployee(id);            
            dataAccess.DeleteEmployee(employee);
        }
        
        public Employee UpdateEmployee(Employee item)
        {           
            var result =  dataAccess.UpdateEmployee(_mapper.Map<EmployeeDAO>(item));
            return _mapper.Map<Employee>(result);
        }
        #endregion        
    }
}
