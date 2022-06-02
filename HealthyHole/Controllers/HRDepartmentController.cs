using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuisnessLogicLayer.Services;
using BuisnessLogicLayer.Models;
using HealthyHole.ViewModels;
using HealthyHole.Titles;
using System;
using System.Linq;

namespace HealthyHole.Controllers
{
    [Route("api/HumanRelations")]
    [ApiController]
    public class HRDepartmentController : ControllerBase
    {
        private readonly IHumanResources resources;
        IMapper _mapper;
        MapperConfiguration config;
        /**
        * Automapper also allows us to ignore some of the fields when mapped in one of the direction if you want to hide some info
        */
        public HRDepartmentController(IHumanResources resources)
        {
            this.resources = resources;
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeView>();
                cfg.CreateMap<EmployeeView, Employee>().ForMember(x=>x.Shifts, opt => opt.Ignore());
            });
            _mapper = new Mapper(config);
        }

        /// <summary>
        /// To Get a Single Employee
        /// </summary>
        /// <param name="id"></param>        
        [HttpGet, Route("getEmployee/{id}")]
        public IActionResult Get(int id)
        {           
            var employee = resources.GetEmployee(id);
            if (employee == null)
            {
                return new NotFoundObjectResult(
                    new { StatusCode = 400, Message = "There is no employee with this id!" });
            }
            var result = _mapper.Map<EmployeeView>(employee);
            return Ok(new { StatusCode = 200, result});
        }

        ///<summary>
        ///   Swagger making field on UI *required only
        ///   to use null parameter and to show all employees just use browser request /api/HRDepartment/getEmployees/.
        ///<summary>
        [HttpGet, Route("getEmployees/{title?}")]
        public async Task<IActionResult> GetAllEmployees(string? title)
        {
            var existingTitles = Enum.GetNames(typeof(TitlesEnum));
            if (existingTitles.Any(x => x == title) || title == null)
            {
                var result = await resources.GetEmployeesAsync(title);
                return Ok(new { StatusCode = 200, result });
            }
            return new BadRequestObjectResult(
                 new { StatusCode = 400, Message = "No such Title exists!" });

        }
        /// <summary>
        /// To Get All Titles That Assigned to Employees atm.
        /// </summary>
        [HttpGet, Route("allTitles")]
        public async Task<IActionResult> GetAllTitlesAsync()
        {
            var result = await resources.GetAllTitlesAsync();
            return Ok(new { StatusCode = 200, result});
        }

        /// <summary>
        /// Add single new Employee
        /// @id field as well as @shifts can be discarded
        /// </summary>
        /// <param name="model"></param>   
        [HttpPost,Route("addEmployee")]
        public async Task<IActionResult> PostAsync([FromBody] EmployeeView model)
        {
            if (model.Name == null || model.Surname == null || model.Title == null)
            {
                return new BadRequestObjectResult(
                    new { StatusCode = 400, Message = "Fill Required Fields!" });
            }                
            var employee = _mapper.Map<Employee>(model);
            var added = await resources.AddEmployeeAsync(employee);
            var result = _mapper.Map<EmployeeView>(added);

            return Ok(new { StatusCode = 200, result});
        }
        /// <summary>
        /// @shifts fiedl can be discarded
        /// </summary>
        /// <param name="model"></param>
        [HttpPost, Route("updateEmployee")]
        public IActionResult Update([FromBody] EmployeeView model)
        {
            var product = resources.GetEmployee(model.Id);
            if (product == null)
            {
                return new BadRequestObjectResult(
                    new { StatusCode = 400, Message = "There is no employee with this id!" });
            }
            var employee = _mapper.Map<Employee>(model);
            var updatedEmployee = resources.UpdateEmployee(employee);
            var result = _mapper.Map<EmployeeView>(updatedEmployee);
            return Ok(new { StatusCode = 200, result});
        }
        
        /// <summary>
        /// Delete Employee entry
        /// </summary>
        /// <param name="id"></param>  
        [HttpDelete,Route("deleteEmployee/{id?}")]
        public IActionResult Delete(int id)
        {
            if(id < 0 || id == default)
            {
                return new BadRequestObjectResult(
                    new { StatusCode = 400, Message = "There is no employee with this id!" }); ;
            }
            var employee =  resources.GetEmployee(id);
            if (employee == null)
            {
                return new BadRequestObjectResult(
                    new { StatusCode = 400, Message = "There is no employee with this id!" });
            }            
            resources.RemoveEmployee(id);

            return Ok(new { StatusCode = 200, Message = $"Successfully deleted user with id - {employee.Id}!" });            
        }

    }
}
