using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public static class MyDbContextOptions
    {
        public static string ConnectionString { get;} = "Server=localhost;Port=3306;Database=EmployeeTimeSheetDB;Uid=root;Pwd=root1122";
        public static DbContextOptionsBuilder<employeetimesheetdbContext> optionsBuilder { get; } = new DbContextOptionsBuilder<employeetimesheetdbContext>().UseMySql(MyDbContextOptions.ConnectionString);
    }
}
