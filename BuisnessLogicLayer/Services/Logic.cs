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
        public void ShiftStarts(int id)
        {
            Shift newShift = new Shift
                {
                ShiftStarts = DateTime.Now,
                EmployeeId = id,
                };

            shiftAccess.AddNewShift(_mapper.Map<ShiftDAO>(newShift));
        }

        public void ShiftEnds(int id)
        {
            var currentShift = shiftAccess.GetShift(id);
            currentShift.ShiftEnds = DateTime.Now;
            currentShift.Hours = currentShift.ShiftEnds.Hour - currentShift.ShiftStarts.Hour;
            shiftAccess.UpdateShift(currentShift);
        }
        public Shift GetShift(int id) 
        {
            return _mapper.Map<Shift>(shiftAccess.GetShift(id));
        }
        #endregion
    }
}
