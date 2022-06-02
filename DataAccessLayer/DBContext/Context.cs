using Microsoft.EntityFrameworkCore;
using System;
using DataAccessLayer.Models;

namespace DataAccessLayer.DBContext
{
    public class Context : DbContext
    {
        public DbSet<EmployeeDAO> Employees { get; set; }
        public DbSet<ShiftDAO> Shifts { get; set; }
        public Context(DbContextOptions<Context> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeDAO>().HasData(new EmployeeDAO[] {
            new EmployeeDAO{ Id=1, Name="Artur",Surname="Kupchinsky", Fatherhood="Catz", Title="Manager", Strikes=0},
            new EmployeeDAO{ Id=2, Name="Vasya",Surname="Pupkin", Fatherhood="Galerovich", Title="Engineer",Strikes=0},
            new EmployeeDAO{ Id=3, Name="Grisha",Surname="Zadneprivodniy", Fatherhood="Massonovich", Title="Tester",Strikes=1}
            });
            modelBuilder.Entity<ShiftDAO>().HasData(new ShiftDAO[]
            {
                new ShiftDAO{ Id=1, ShiftStarts= new DateTime(2022,5,31,10,0,0), ShiftEnds=new DateTime(2022,5,31,18,0,0), Hours=8, EmployeeId=1 },
                new ShiftDAO{ Id=2, ShiftStarts= new DateTime(2022,5,31,10,0,0), ShiftEnds=new DateTime(2022,5,31,18,0,0), Hours=8, EmployeeId=2 },
                new ShiftDAO{ Id=3, ShiftStarts= new DateTime(2022,5,31,17,0,0), ShiftEnds=new DateTime(2022,5,31,18,0,0), Hours=1, EmployeeId=3 }
            });

        }
    }
}
