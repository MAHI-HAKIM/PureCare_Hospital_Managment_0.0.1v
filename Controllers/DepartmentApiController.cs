using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.Models;

namespace PureCareHub_HospitalCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentApiController : Controller
    {
        private ApplicationDBContext _dbContext;
        public DepartmentApiController(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpGet]
        public List<Department> Get()
        {
            var departments = _dbContext.Departments.ToList();
            return departments;
        }

        [HttpGet("{id}")]
        public ActionResult<Department> Get(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var department = _dbContext.Departments.FirstOrDefault(z => z.Id == id);
            if (department == null)
            {
                return NotFound(); 
            }
            return department;
        }


		[HttpPost]
		public IActionResult Post([FromBody] Department dep)
		{
			_dbContext.Departments.Add(dep);
			_dbContext.SaveChanges();
			return Ok(dep);
		}


		[HttpPut("{id}")]
		public IActionResult Put(int? id, [FromBody] Department updatedDepartment)
		{
			if (id is null)
			{
				return NotFound();
			}

			var existingDepartment = _dbContext.Departments.FirstOrDefault(d => d.Id == id);

			if (existingDepartment == null)
			{
				return NotFound($"Department with ID {id} not found");
			}

			// Update properties of the existing department
			existingDepartment.DepartmentName = updatedDepartment.DepartmentName;
			existingDepartment.DepartmentDescription = updatedDepartment.DepartmentDescription;
			existingDepartment.PhotoPath = updatedDepartment.PhotoPath;

			_dbContext.Update(existingDepartment);
			_dbContext.SaveChanges();
			_dbContext.SaveChanges();

			return Ok(existingDepartment);
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int? id)
		{
			if (id is null)
			{
				return NotFound();
			}
			var department = _dbContext.Departments.FirstOrDefault(z => z.Id == id);
			if (department == null)
			{
				return NotFound();
			}
		
			_dbContext.Departments.Remove(department);
			_dbContext.SaveChanges();
			return Ok(department);
		}
	}
}
