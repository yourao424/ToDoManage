using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ToDoManage.Models.Data
{
    public interface IDataContext
    {
        DbSet<ToDoTask> ToDoTask { get; set; }

        Task<int> SaveChanges();
    }
}