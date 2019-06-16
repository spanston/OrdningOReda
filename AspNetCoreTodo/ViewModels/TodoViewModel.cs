using System;
using System.Collections.Generic;
using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.ViewModels
{

    //This model represents several items of TodoItems to view
    public class TodoViewModel
    {
        public TodoItem[] Items { get; set; }
        public IEnumerable<ItemCategory> PriorityTagsList { get; set; }
        public TodoList List { get; set; }


    }
}