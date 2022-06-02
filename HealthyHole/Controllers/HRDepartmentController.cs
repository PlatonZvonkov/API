using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuisnessLogicLayer.Services;
using BuisnessLogicLayer.Models;
using HealthyHole.ViewModels;
using HealthyHole.Enums;
using System;
using System.Linq;

namespace HealthyHole.Controllers
{
    [Route("api/HumanRelations")]
    [ApiController]

    public class HRDepartmentController : ControllerBase
    {
        private readonly IResources resources;
        IMapper _mapper;
        MapperConfiguration config;
        /**
        * Automapper also allows us to ignore some of the fields when mapped in one of the direction if you want to hide some info
        */
        public HRDepartmentController(IResources resources)
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
            return Ok(_mapper.Map<EmployeeView>(employee));
        }

        ///<summary>
        ///   Swagger making field on UI *required only
        ///   to use null parameter and to show all employees just use browser request /api/HRDepartment/getEmployees/.
        ///<summary>
        [HttpGet, Route("getEmployees/{title?}")]
        public IActionResult GetAllEmployees(string? title)
        {
            var existingTitles = Enum.GetNames(typeof(TitlesEnum));
            if (existingTitles.Any(x => x == title) || title == null)
            {
                var result = resources.GetEmployees(title);
                return Ok(result);
            }
            return new BadRequestObjectResult(
                 new { StatusCode = 400, Message = "No such Title exists!" });

        }
        /// <summary>
        /// To Get All Titles That Assigned to Employees atm.
        /// </summary>
        [HttpGet, Route("allTitles")]
        public async Task<List<string>> GetAllTitlesAsync()
        {
            var result = await resources.GetAllTitlesAsync();
            return result;
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
            var result = await resources.AddEmployeeAsync(employee);
            return Ok(_mapper.Map<EmployeeView>(result));
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
            var result = resources.UpdateEmployee(employee);
            return Ok(_mapper.Map<EmployeeView>(result));
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

            return Ok();            
        }

    }
}
