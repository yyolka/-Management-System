using Core.Converters;
using Core.Models.Business;
using DatabaseContext;
using DatabaseModels.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class Facade
    {
        private readonly UniversityDbContext _context;

        public Facade(UniversityDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateGroupAsync(string name)
        {
            var group = new GroupDb
            {
                Name = name,
                CreationDate = DateTime.UtcNow
            };
            
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group.Id;
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            var groups = await _context.Groups.ToListAsync();
            return groups.Select(g => g.ToBusiness()).ToList();
        }

        public async Task<bool> UpdateGroupAsync(int id, string newName)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return false;

            group.Name = newName;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return false;

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreateStudentAsync(int groupId, string name, int age)
        {
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == groupId);
            if (!groupExists)
                throw new ArgumentException($"Group with ID {groupId} does not exist");

            var student = new StudentDb
            {
                GroupId = groupId,
                Name = name,
                Age = age
            };
            
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var students = await _context.Students.ToListAsync();
            return students.Select(s => s.ToBusiness()).ToList();
        }

        public async Task<bool> UpdateStudentAsync(int id, string newName, int newAge)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            student.Name = newName;
            student.Age = newAge;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreateCuratorAsync(int groupId, string name, string email)
        {
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == groupId);
            if (!groupExists)
                throw new ArgumentException($"Group with ID {groupId} does not exist");

            var curatorExists = await _context.Curators.AnyAsync(c => c.GroupId == groupId);
            if (curatorExists)
                throw new ArgumentException($"Group with ID {groupId} already has a curator");

            var curator = new CuratorDb
            {
                GroupId = groupId,
                Name = name,
                Email = email
            };
            
            _context.Curators.Add(curator);
            await _context.SaveChangesAsync();
            return curator.Id;
        }

        public async Task<List<Curator>> GetAllCuratorsAsync()
        {
            var curators = await _context.Curators.ToListAsync();
            return curators.Select(c => c.ToBusiness()).ToList();
        }

        public async Task<bool> UpdateCuratorAsync(int id, string newName, string newEmail)
        {
            var curator = await _context.Curators.FindAsync(id);
            if (curator == null) return false;

            curator.Name = newName;
            curator.Email = newEmail;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCuratorAsync(int id)
        {
            var curator = await _context.Curators.FindAsync(id);
            if (curator == null) return false;

            _context.Curators.Remove(curator);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetStudentCountInGroupAsync(int groupId)
        {
            return await _context.Students
                .Where(s => s.GroupId == groupId)
                .CountAsync();
        }

        public async Task<string?> GetCuratorNameForStudentAsync(int studentId)
        {
            return await _context.Students
                .Where(s => s.Id == studentId)
                .Join(
                    _context.Curators,
                    student => student.GroupId,
                    curator => curator.GroupId,
                    (student, curator) => curator.Name
                )
                .FirstOrDefaultAsync();
        }

        public async Task<double?> GetAverageAgeByCuratorAsync(int curatorId)
        {
            var averageAge = await _context.Curators
                .Where(c => c.Id == curatorId)
                .Join(
                    _context.Groups,
                    curator => curator.GroupId,
                    group => group.Id,
                    (curator, group) => group
                )
                .SelectMany(group => group.Students)
                .Select(student => (double?)student.Age)
                .AverageAsync();
            
            return averageAge;
        }
    }
}