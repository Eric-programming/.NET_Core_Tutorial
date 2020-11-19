using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Data;
using UniversitySystem.Models;

namespace UniversitySystem.Controllers
{
    public class InstructorController : Controller
    {
        private readonly UniversityContext _context;

        public InstructorController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Instructor
        public async Task<IActionResult> Index()
        {
            return View(await _context.Instructors.Include(i => i.OfficeAssignment)
          .Include(i => i.CourseAssignments)
            .ThenInclude(i => i.Course).ToListAsync());
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id, int? courseID)
        {
            if (id == null)
            {
                return NotFound();
            }
            var viewModel = new InstructorDetails();

            //Get the instructor
            var instructor = await _context.Instructors
                 .Include(i => i.CourseAssignments)//We need to get the course and department 
                     .ThenInclude(i => i.Course)
                     .ThenInclude(i => i.Department)
                 .Include(x => x.OfficeAssignment)//Because we need to get the location
                 .FirstOrDefaultAsync(x => x.ID == id);

            if (instructor == null)
            {
                return NotFound();
            }


            viewModel.CourseTaughtByCurrentInstructor = instructor.CourseAssignments.Select(x => x.Course); //All Courses taught by this instructor
            if (courseID != null)
            {
                ViewData["CourseID"] = courseID;
                //use explicit loading so we load the enrollment data only if it's requested
                var selectedCourse = viewModel.CourseTaughtByCurrentInstructor.Where(x => x.CourseID == courseID).Single();
                await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                viewModel.AllEnrollmentFromCurrentCourse = selectedCourse.Enrollments;//Get Enrollments related to this course
            }
            viewModel.CurrentInstructor = instructor;//Get This instructor


            return View(viewModel);
        }

        // GET: Instructor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstMidName,HireDate")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments).ThenInclude(i => i.Course)
                .AsNoTracking().FirstOrDefaultAsync(s => s.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            PopulateCoursesData(instructor);
            return View(instructor);
        }
        private void PopulateCoursesData(Instructor instructor)
        {
            var allCourses = _context.Courses;
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            IList<SelectCourse> viewOptions = new List<SelectCourse>();
            foreach (var course in allCourses)
            {
                viewOptions.Add(new SelectCourse
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    isSelected = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewBag.courseOptions = viewOptions;
        }
        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedCourses)
        {
            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructorToUpdate == null)
            {
                return NotFound();
            }
            var res = await TryUpdateModelAsync<Instructor>(
         instructorToUpdate,
         "",
         i => i.FirstMidName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment);
            if (res)
            {
                if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;
                }
                UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(id))
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
            UpdateInstructorCourses(selectedCourses, instructorToUpdate);
            PopulateCoursesData(instructorToUpdate);
            return RedirectToAction(nameof(Index));
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            //Empty list if courses are empty
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var instructorCourses = new HashSet<int>
                (instructorToUpdate.CourseAssignments.Select(c => c.Course.CourseID));//All the courseId that current instructor have
            foreach (var course in _context.Courses)
            {
                //If current course is inside the selected course list but current instuctor doesn't have it, then we add it
                if (selectedCourses.Contains(course.CourseID.ToString()) && !instructorCourses.Contains(course.CourseID))
                {

                    instructorToUpdate.CourseAssignments.Add(new CourseAssignment { InstructorID = instructorToUpdate.ID, CourseID = course.CourseID });
                }
                //Else if current course is not inside the selected course list but current instructor has it, then we remove it
                else if (!selectedCourses.Contains(course.CourseID.ToString()) && instructorCourses.Contains(course.CourseID))
                {
                    CourseAssignment courseToRemove = instructorToUpdate.CourseAssignments.FirstOrDefault(i => i.CourseID == course.CourseID);
                    _context.Remove(courseToRemove);
                }
            }
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
    }
}
