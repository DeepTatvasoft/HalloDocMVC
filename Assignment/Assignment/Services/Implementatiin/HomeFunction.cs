using Data.DataContext;
using Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementatiin
{
    public class HomeFunction : IHomeFunction
    {
        private readonly ApplicationDbContext _context;
        public HomeFunction(ApplicationDbContext context)
        {
            _context = context;
        }
        public void SubmitData(StudentFormModal modal, string gender)
        {
            var checkcourse = _context.Courses.FirstOrDefault(c => c.Name.ToLower() == modal.course.ToLower());
            Student student = new Student();
            if (checkcourse == null)
            {
                Course course = new Course();
                course.Name = modal.course;
                _context.Courses.Add(course);
                _context.SaveChanges();
                student.CourseId = course.Id;
            }
            else
            {
                student.CourseId = checkcourse.Id;
            }
            student.FirstName = modal.firstname;
            student.LastName = modal.lastname;
            student.Email = modal.email;
            student.Age = DateTime.Now.Year - modal.DOB.Year;
            student.Gender = gender;
            student.Course = modal.course;
            student.Grade = modal.grade;
            student.Dob = modal.DOB;
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public StudentFormModal Index()
        {
            StudentFormModal modal = new StudentFormModal();
            var students = _context.Students.Count();
            if (students != 0)
            {
                modal.students = _context.Students.ToList();
            }
            return modal;
        }
        public StudentFormModal EditStudent(int id)
        {
            var student = _context.Students.FirstOrDefault(u => u.Id == id);
            StudentFormModal modal = new StudentFormModal();
            modal.firstname = student!.FirstName;
            modal.lastname = student.LastName;
            modal.email = student.Email;
            modal.gender = student.Gender;
            modal.grade = student.Grade;
            modal.course = student.Course;
            modal.studentid = id;
            modal.DOB = student.Dob;
            return modal;
        }

        public void EditData(StudentFormModal modal, string gender)
        {
            var student = _context.Students.FirstOrDefault(u => u.Id == modal.studentid);
            var checkcourse = _context.Courses.FirstOrDefault(c => c.Name.ToLower() == modal.course.ToLower());
            if (checkcourse == null)
            {
                Course course = new Course();
                course.Name = modal.course;
                _context.Courses.Add(course);
                _context.SaveChanges();
                student.CourseId = course.Id;
            }
            else
            {
                student.CourseId = checkcourse.Id;
            }
            student.FirstName = modal.firstname;
            student.LastName = modal.lastname;
            student.Email = modal.email;
            student.Age = DateTime.Now.Year - modal.DOB.Year;
            student.Gender = gender;
            student.Course = modal.course;
            student.Grade = modal.grade;
            student.Dob = modal.DOB;
            _context.Students.Update(student);
            _context.SaveChanges();
        }
        public void DeleteData(int id)
        {
            var student = _context.Students.FirstOrDefault(u => u.Id == id);
            _context.Students.Remove(student);
            _context.SaveChanges();
        }
    }
}
