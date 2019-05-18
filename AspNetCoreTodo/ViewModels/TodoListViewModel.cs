using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.ViewModels
{
    public class TodoListViewModel
    {
        public IEnumerable<TodoItemList> TodoItemLists { get; set; }

    }
}
