using Microsoft.AspNetCore.Mvc;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.Models;

namespace PureCareHub_HospitalCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceAPIController : Controller
    {
        private ApplicationDBContext _dbContext;
        public ServiceAPIController(ApplicationDBContext dBContext) { 
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

    }
}
