using AutoMapper;
using BuisnessLogicLayer.Models;
using DataAccessLayer;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BuisnessLogicLayer.Services
{
    public class HumanResourcesService : IHumanResources
    {
        private readonly IDataAccess dataAccess;        
        MapperConfiguration config; 
        IMapper _mapper;
        /**
        * Names of models end with DAO (data access object) for convinience to distinct them from models on this layer
        * and mapping convinience
        */
        public HumanResourcesService(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;            
            config = new MapperConfiguration(cfg =>
            { 
                cfg.CreateMap<Employee, EmployeeDAO>();
                cfg.CreateMap<EmployeeDAO, Employee>();
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
            var result = dataAccess.GetEmployee(id);            
            return _mapper.Map<Employee>(result);
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
