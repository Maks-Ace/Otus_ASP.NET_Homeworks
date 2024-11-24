using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.WebHost.Models.Employee;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles?.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать нового сотрудника
        /// </summary>
        /// <returns>id созданного сотрудника</returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync(CreatingEmployeeModel creatingModel)
        {

            var newEmployee = new Employee()
            {
                FirstName = creatingModel.FirstName,
                LastName = creatingModel.LastName,
                Email = creatingModel.Email
            };

            
            return Ok(await _employeeRepository.CreateAsync(newEmployee));
        }


        /// <summary>
        /// Обновить почтку указанного по id сотрудника 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newEmail"></param>
        /// <returns></returns>
        [HttpPatch("update-email")]
        public async Task<IActionResult> UpdateEmployeeEmailByIdAsync(Guid id, string newEmail)
        {
            var employeeToUpdate = await _employeeRepository.GetByIdAsync(id);


            employeeToUpdate.Email = newEmail;
            
            return Ok(await _employeeRepository.UpdateAsync(employeeToUpdate));
        }


        /// <summary>
        /// Удалить сотрудника с указанным id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeebyIdAsync(Guid id)
        {
            return Ok(await _employeeRepository.DeleteByIdAsync(id));
        }

    }
}