using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoManage.Models;
using ToDoManage.Models.Data;

namespace ToDoManage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataContext _context;

        public HomeController(ILogger<HomeController> logger, IDataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            ViewData["pageNumber"] = pageNumber ?? 1;

            List<ToDoTask> tasks = await GetTasks();

            return View(PaginatedList<ToDoTask>.Create(tasks, pageNumber ?? 1));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/AddTask")]
        [HttpPost]
        public async Task<IActionResult> AddTask(string title, string description, int pageIndex = 1)
        {
            var model = new ToDoTask(title, description);
            _context.ToDoTask.Add(model);
            await _context.SaveChanges();

            List<ToDoTask> tasks = await GetTasks();

            return PartialView("~/Views/partial/_TaskView.partial.cshtml", PaginatedList<ToDoTask>.Create(tasks, pageIndex));
            
        }

        [Route("/UpdateTask")]
        public async Task<IActionResult> UpdateTask(int id, string title, string description, int pageIndex)
        {
            ToDoTask model = _context.ToDoTask.FirstOrDefault(x => x.taskId == id);

            if (model == null)
            {
                return NotFound();
            }

            model.Update(title, description);
            await _context.SaveChanges();

            return PartialView("~/Views/partial/_TaskView.partial.cshtml", PaginatedList<ToDoTask>.Create(await GetTasks(), pageIndex));
        }

        [Route("/DoneTask")]
        [HttpPost]
        public async Task<IActionResult> DoneTask(int id, int pageIndex)
        {
            ToDoTask model = _context.ToDoTask.Where(x => x.taskId == id).FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }

            model.Done();
            await _context.SaveChanges();

            return PartialView("~/Views/partial/_TaskView.partial.cshtml", PaginatedList<ToDoTask>.Create(await GetTasks(), pageIndex));

        }

        [Route("/DelTask")]
        [HttpPost]
        public async Task<IActionResult> DelTask(int id, int pageIndex)
        {
            ToDoTask model = _context.ToDoTask.Where(x => x.taskId == id).FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }

            _context.ToDoTask.Remove(model);

            await _context.SaveChanges();

            return PartialView("~/Views/partial/_TaskView.partial.cshtml", PaginatedList<ToDoTask>.Create(await GetTasks(), pageIndex)); 

        }

        [Route("/SearchTask")]
        [HttpPost]
        public async Task<IActionResult> SearchTask(string searchText)
        {
            return PartialView("~/Views/partial/_TaskView.partial.cshtml", PaginatedList<ToDoTask>.Create(await GetTasks(searchText), 1));
        }

        [Route("/PaginateChange")]
        [HttpPost]
        public async Task<IActionResult> PaginateChange(string searchText, int pageIndex)
        {
            return PartialView("~/Views/partial/_TaskView.partial.cshtml", PaginatedList<ToDoTask>.Create(await GetTasks(searchText), pageIndex));
        }

        public async Task<List<ToDoTask>> GetTasks(string searchText = "")
        {
            List<ToDoTask> list = null;
            if (string.IsNullOrEmpty(searchText))
            {
                list = await _context.ToDoTask.AsNoTracking().OrderBy(x => x.taskId).ToListAsync();
            }
            else
            {
                list = await _context.ToDoTask.AsNoTracking().Where(x => x.title.Contains(searchText)).OrderBy(x => x.taskId).ToListAsync();
            }

            if (list == null)
            {
                list = new List<ToDoTask>();
            }
            return list;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
