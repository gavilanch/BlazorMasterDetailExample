using BlazorMasterDetail.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMasterDetail.Server.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public StudentsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> Get()
        {
            return await context.Students.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var student = await context.Students
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (student == null) { return NotFound(); }

            return student;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Student student)
        {
            context.Students.Add(student);
            await context.SaveChangesAsync();
            return student.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Student student)
        {
            context.Entry(student).State = EntityState.Modified;

            foreach (var address in student.Addresses)
            {
                if (address.Id != 0)
                {
                    context.Entry(address).State = EntityState.Modified;
                }
                else
                {
                    context.Entry(address).State = EntityState.Added;
                }
            }

            var idsOfAddresses = student.Addresses.Select(x => x.Id).ToList();
            var addressesToDelete = await context
                .Addresses
                .Where(x => !idsOfAddresses.Contains(x.Id) && x.StudentId == student.Id)
                .ToListAsync();

            context.RemoveRange(addressesToDelete);

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}


