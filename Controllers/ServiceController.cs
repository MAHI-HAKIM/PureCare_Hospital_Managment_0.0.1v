using Microsoft.AspNetCore.Mvc;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.ViewModels;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;


namespace PureCareHub_HospitalCare.Controllers
{
    public class ServiceController : Controller
    {
		//private readonly IDocRepository _docRepository;
		private readonly ApplicationDBContext _dbContext;
		private readonly IWebHostEnvironment _webHostEnvironment;
        public ServiceController (ApplicationDBContext dbContext,
                                    IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

		public IActionResult Index()
        {
            var departments = _dbContext.Departments.ToList();

            return View(departments);
        }
        public IActionResult List(int departmentID)
        {
            var doctors = _dbContext.doctors.Where(d=> d.depID == departmentID).ToList();
            var department = _dbContext.Departments.Find(departmentID);

            ServiceViewModel viewModel = new ServiceViewModel()
            {
                doctorsList = doctors,
                PhotoPath = department.PhotoPath,
                DepartmentName = department.DepartmentName,
                DepartmentDescription = department.DepartmentDescription
            };

            return View(viewModel);
        }
    }
}
