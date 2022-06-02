using Microsoft.AspNetCore.Mvc;
using System;
using BuisnessLogicLayer.Services;


namespace HealthyHole.Controllers
{
    [ApiController]
    [Route("api/Checkpoint")]
    public class KppController : ControllerBase
    {
        private readonly IResources resources;
        private readonly INewShift newShift;        
        public KppController(IResources resources, INewShift newShift)
        {
            this.resources = resources;
            this.newShift = newShift;            
        }
        /// <summary>
        /// Entrance at the Facility, grants access only to the initiated
        /// </summary>
        /// <param name="id"></param>
        [HttpPost, Route("ShiftStarts")]
        public IActionResult ShiftStarts(int id)
        {
            var employee = resources.GetEmployee(id);
            if (employee == null)
            {
                return new BadRequestObjectResult(
                    new { StatusCode = 400, Message = "ACCESS DENIED!!!" });
            }
            newShift.ShiftStarts(id);
            return Ok($"ACCESS GRANTED! Welcome {employee.Name} {employee.Surname}!");
        }
        /// <summary>
        /// Exit at the Facility, grants access only to those who worked their ass of
        /// </summary>
        /// <param name="id"></param>
        [HttpPost, Route("ShiftEnds")]
        public IActionResult ShiftEnds(int id)
        {
            var employee = resources.GetEmployee(id);
            if (employee == null)
            {
                return new BadRequestObjectResult(
                    new { StatusCode = 400, Message = "ACCESS DENIED!!!" });
            }
            var check = newShift.GetShift(id);
            if (check.ShiftStarts.Date != DateTime.Now.Date)
            {
                return new BadRequestObjectResult(
                    new { StatusCode = 400, Message = "ACCESS DENIED! YOU HAVE NOT CHECKED IN YET!" });
            }
            newShift.ShiftEnds(id);
            return Ok($"ACCESS GRANTED! Have a great evening {employee.Name}!");
        }
    }
}
