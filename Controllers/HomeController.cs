﻿using Microsoft.AspNetCore.Mvc;
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


        public async Task<List<ToDoTask>> GetTasks()
        {
            List<ToDoTask> list = await _context.ToDoTask.OrderBy(x => x.taskId).ToListAsync();
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
