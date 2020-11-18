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
    public class CourseAssignmentController : Controller
    {
        private readonly UniversityContext _context;

        public CourseAssignmentController(UniversityContext context)
        {
            _context = context;
        }

        // GET: CourseAssignment
        public async Task<IActionResult> Index()
        {
            var universityContext = _context.CourseAssignments.Include(c => c.Course).Include(c => c.Instructor);
            return View(await universityContext.ToListAsync());
        }

        // GET: CourseAssignment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAssignment = await _context.CourseAssignments
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (courseAssignment == null)
            {
                return NotFound();
            }

            return View(courseAssignment);
        }

        // GET: CourseAssignment/Create
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseID");
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstMidName");
            return View();
        }

        // POST: CourseAssignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorID,CourseID")] CourseAssignment courseAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseID", courseAssignment.CourseID);
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstMidName", courseAssignment.InstructorID);
            return View(courseAssignment);
        }

        // GET: CourseAssignment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAssignment = await _context.CourseAssignments.FindAsync(id);
            if (courseAssignment == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseID", courseAssignment.CourseID);
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstMidName", courseAssignment.InstructorID);
            return View(courseAssignment);
        }

        // POST: CourseAssignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorID,CourseID")] CourseAssignment courseAssignment)
        {
            if (id != courseAssignment.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseAssignmentExists(courseAssignment.CourseID))
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
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseID", courseAssignment.CourseID);
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstMidName", courseAssignment.InstructorID);
            return View(courseAssignment);
        }

        // GET: CourseAssignment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAssignment = await _context.CourseAssignments
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (courseAssignment == null)
            {
                return NotFound();
            }

            return View(courseAssignment);
        }

        // POST: CourseAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseAssignment = await _context.CourseAssignments.FindAsync(id);
            _context.CourseAssignments.Remove(courseAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseAssignmentExists(int id)
        {
            return _context.CourseAssignments.Any(e => e.CourseID == id);
        }
    }
}
