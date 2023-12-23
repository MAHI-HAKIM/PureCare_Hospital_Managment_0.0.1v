//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using PureCareHub_HospitalCare.Data;
//using PureCareHub_HospitalCare.Models;

//namespace PureCareHub_HospitalCare.Controllers
//{
//    public class DController : Controller
//    {
//        private readonly ApplicationDBContext _context;

//        public DController(ApplicationDBContext context)
//        {
//            _context = context;
//        }

//        // GET: D
//        public async Task<IActionResult> Index()
//        {
//            var applicationDBContext = _context.doctors.Include(d => d.department);
//            return View(await applicationDBContext.ToListAsync());
//        }

//        // GET: D/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null || _context.doctors == null)
//            {
//                return NotFound();
//            }

//            var doctor = await _context.doctors
//                .Include(d => d.department)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (doctor == null)
//            {
//                return NotFound();
//            }

//            return View(doctor);
//        }

//        // GET: D/Create
//        public IActionResult Create()
//        {
//            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName");
//            return View();
//        }

//        // POST: D/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,Email,PhotoPath,WorkingShift,DepartmentID,DoctorGender")] Doctor doctor)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(doctor);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName", doctor.DepartmentID);
//            return View(doctor);
//        }

//        // GET: D/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null || _context.doctors == null)
//            {
//                return NotFound();
//            }

//            var doctor = await _context.doctors.FindAsync(id);
//            if (doctor == null)
//            {
//                return NotFound();
//            }
//            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName", doctor.DepartmentID);
//            return View(doctor);
//        }

//        // POST: D/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,Email,PhotoPath,WorkingShift,DepartmentID,DoctorGender")] Doctor doctor)
//        {
//            if (id != doctor.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(doctor);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!DoctorExists(doctor.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName", doctor.DepartmentID);
//            return View(doctor);
//        }

//        // GET: D/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null || _context.doctors == null)
//            {
//                return NotFound();
//            }

//            var doctor = await _context.doctors
//                .Include(d => d.department)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (doctor == null)
//            {
//                return NotFound();
//            }

//            return View(doctor);
//        }

//        // POST: D/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (_context.doctors == null)
//            {
//                return Problem("Entity set 'ApplicationDBContext.doctors'  is null.");
//            }
//            var doctor = await _context.doctors.FindAsync(id);
//            if (doctor != null)
//            {
//                _context.doctors.Remove(doctor);
//            }
            
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool DoctorExists(int id)
//        {
//          return (_context.doctors?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}
