using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crud_operation.Models
{
    public class StudentContext: DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options):base(options)
        {

        }
        
        public DbSet<Students> Students { get; set; }

        public DbSet<Departments> Departments { get; set; }
    }
}
