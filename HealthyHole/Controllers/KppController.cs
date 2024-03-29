﻿using Microsoft.AspNetCore.Mvc;
using System;
using BuisnessLogicLayer.Services;
using BuisnessLogicLayer.Models;

namespace HealthyHole.Controllers
{
    [ApiController]
    [Route("api/Checkpoint")]
    public class KppController : ControllerBase
    {
        private readonly IHumanResources _resources;
        private readonly IShift _shift;        
        public KppController(IHumanResources resources, IShift shift)
        {
            _resources = resources;
            _shift = shift;
        }
        /// <summary>
        /// Entrance at the Facility, grants access only to the initiated
        /// </summary>
        /// <param name="id"></param>
        [HttpGet, Route("ShiftStarts/{id}")]
        public IActionResult ShiftStarts(int id)
        {
            if (id<0)
            {
                return BadRequest(
                    new { Message = "ACCESS DENIED!!!" });
            }
            Employee employee = _resources.GetEmployee(id);
            if (employee == null)
            {
                return BadRequest(
                    new { Message = "ACCESS DENIED!!!" });
            }
            string message = _shift.ShiftStarts(employee);
            if (message != null)
            {
                return Ok(new
                { msg =$"ACCESS GRANTED! Welcome {employee.Name} {employee.Surname}!" +
                    $"Warning! {message}"
                });
            }
            return Ok( new { msg = $"ACCESS GRANTED! Welcome {employee.Name} {employee.Surname}!" });
        }
        /// <summary>
        /// Exit at the Facility, grants access only to those who worked their ass of
        /// </summary>
        /// <param name="id"></param>
        [HttpGet, Route("ShiftEnds/{id}")]
        public IActionResult ShiftEnds(int id)
        {
            if (id < 0)
            {
                return BadRequest(
                    new { Message = "ACCESS DENIED!!!" });
            }
            Employee employee = _resources.GetEmployee(id);
            if (employee == null)
            {
                return BadRequest(
                    new { Message = "ACCESS DENIED!!!" });
            }
            Shift check = _shift.GetShift(id);
            if (check.ShiftStarts.Date != DateTime.Now.Date)
            {
                return BadRequest(
                    new {  Message = "ACCESS DENIED! YOU HAVE NOT CHECKED IN YET!" });
            }
            string message = _shift.ShiftEnds(employee);
            if (message != null)
            {
                return Ok(new
                {
                    msg = $"ACCESS GRANTED!" +
                    $"Warning! {message}"
                });
            }
            return Ok(new
            { msg = $"ACCESS GRANTED! Have a great evening {employee.Name}!" });
        }
    }
}
