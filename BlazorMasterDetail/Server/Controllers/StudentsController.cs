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
    public class StudentsController: ControllerBase
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

        [HttpPost]
        public async Task<ActionResult<int>> Post(Student student)
        {
            context.Students.Add(student);
            await context.SaveChangesAsync();
            return student.Id;
        }
    }
}


