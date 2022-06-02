using AutoMapper;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Services;
using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BuisnessLogicLayer.Services
{
    public class Logic : IResources,INewShift
    {
        private readonly IDataAccess dataAccess;
        private readonly IShiftAccess shiftAccess;
        MapperConfiguration config; 
        IMapper _mapper;
        /**        
        * Names of models end with DAO (data access object) for convinience to distinct them from models on this layer
        * and mapping convinience
        */
        public Logic(IDataAccess dataAccess, IShiftAccess shiftAccess)
        {
            this.dataAccess = dataAccess;
            this.shiftAccess = shiftAccess;
            config = new MapperConfiguration(cfg =>
            { cfg.CreateMap<Employee, EmployeeDAO>();
                cfg.CreateMap<EmployeeDAO, Employee>();

                cfg.CreateMap<Shift, ShiftDAO>();
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
            var result = dataAccess.GetEmployee(id);            
            return _mapper.Map<Employee>(result);
        }

        public async Task<ICollection<Employee>> GetEmployees(string title)
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
        #region Shifts
        /**        
        * Here we not only creating  new shift but also calculating Strikes of employee
        * if he got on work earlier than 09:00 he get a strike, method ShiftEnd will calculate time for strike if earlier than 18:00
        */
        public string ShiftStarts(Employee employee)
        {
            Shift newShift = new Shift
                {
                ShiftStarts = DateTime.Now,
                EmployeeId = employee.Id,
                };
            if (newShift.ShiftStarts.Hour > 9)
            {
                AddStrike(employee);
                return $"You are Late! Strike added! Current Strikes - {employee.Strikes}";
            }
            shiftAccess.AddNewShift(_mapper.Map<ShiftDAO>(newShift));
            return null;

        }

        private void AddStrike(Employee employee)
        {
            employee.Strikes += 1;
            dataAccess.UpdateEmployee(_mapper.Map<EmployeeDAO>(employee));
        }        
        public string ShiftEnds(Employee employee)
        {
            var currentShift = shiftAccess.GetShift(employee.Id);
            currentShift.ShiftEnds = DateTime.Now;
            currentShift.Hours = currentShift.ShiftEnds.Hour - currentShift.ShiftStarts.Hour;
            if (employee.Title == "Tester" && currentShift.Hours < 12)
            {
                AddStrike(employee);
                shiftAccess.UpdateShift(currentShift);
                return $"GET YOUR ASS BACK TO WORK! Strike added! Current Strikes - {employee.Strikes}";
            }
            if (currentShift.ShiftEnds.Hour < 18)
            {
                AddStrike(employee);
                shiftAccess.UpdateShift(currentShift);
                return $"You are leaving too early! Strike added! Current Strikes - {employee.Strikes}";
            }
            shiftAccess.UpdateShift(currentShift);
            return null;
        }

        public Shift GetShift(int id) 
        {
            return _mapper.Map<Shift>(shiftAccess.GetShift(id));
        }
        #endregion
    }
}
