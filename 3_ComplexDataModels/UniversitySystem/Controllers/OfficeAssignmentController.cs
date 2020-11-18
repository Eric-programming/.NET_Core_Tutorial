using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Data;
using UniversitySystem.Models;

namespace UniversitySystem.Controllers
{
    public class OfficeAssignmentController : Controller
    {
        private readonly UniversityContext _context;

        public OfficeAssignmentController(UniversityContext context)
        {
            _context = context;
        }

        // GET: OfficeAssignment
        public async Task<IActionResult> Index()
        {
            var universityContext = _context.OfficeAssignments.Include(o => o.Instructor);
            return View(await universityContext.ToListAsync());
        }

        // GET: OfficeAssignment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeAssignment = await _context.OfficeAssignments
                .Include(o => o.Instructor)
                .FirstOrDefaultAsync(m => m.InstructorID == id);
            if (officeAssignment == null)
            {
                return NotFound();
            }

            return View(officeAssignment);
        }

        // GET: OfficeAssignment/Create
        public IActionResult Create()
        {
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstMidName");
            return View();
        }

        // POST: OfficeAssignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorID,Location")] OfficeAssignment officeAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(officeAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstMidName", officeAssignment.InstructorID);
            return View(officeAssignment);
        }

        // GET: OfficeAssignment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeAssignment = await _context.OfficeAssignments.FindAsync(id);
            if (officeAssignment == null)
            {
                return NotFound();
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstMidName", officeAssignment.InstructorID);
            return View(officeAssignment);
        }

        // POST: OfficeAssignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorID,Location")] OfficeAssignment officeAssignment)
        {
            if (id != officeAssignment.InstructorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(officeAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfficeAssignmentExists(officeAssignment.InstructorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstMidName", officeAssignment.InstructorID);
            return View(officeAssignment);
        }

        // GET: OfficeAssignment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeAssignment = await _context.OfficeAssignments
                .Include(o => o.Instructor)
                .FirstOrDefaultAsync(m => m.InstructorID == id);
            if (officeAssignment == null)
            {
                return NotFound();
            }

            return View(officeAssignment);
        }

        // POST: OfficeAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var officeAssignment = await _context.OfficeAssignments.FindAsync(id);
            _context.OfficeAssignments.Remove(officeAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfficeAssignmentExists(int id)
        {
            return _context.OfficeAssignments.Any(e => e.InstructorID == id);
        }
    }
}
