using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Model.AppDbContext;

namespace Minimal_Services
{
    public class Services
    {
        private readonly AppDbContext _context;

        public Services(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> Get()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> Get(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> Post(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> Put(int id, Student student)
        {
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return null;
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;

            await _context.SaveChangesAsync();
            return existingStudent;
        }

        public async Task<Student> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return null;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return student;
        }
    }
}