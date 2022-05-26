using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ToDoManage.Models.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<ToDoTask> ToDoTask { get; set; }
        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}
