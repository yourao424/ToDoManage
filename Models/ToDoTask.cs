using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoManage.Models.Data
{
    public class ToDoTask
    {
        public ToDoTask(string title, string description)
        {
            this.title = title;
            this.description = description;
            this.isDone = false;
        }

        [Key]
        public int taskId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool isDone { get; set; }

        public void Update(string title, string description)
        {
            this.title = title;
            this.description = description;
        }

        public void Done()
        {
            this.isDone = !this.isDone;
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }
    }
}
