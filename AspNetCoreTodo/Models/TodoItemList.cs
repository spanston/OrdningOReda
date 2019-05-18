using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Models
{
    public class TodoItemList
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string TodoItemListName { get; set; }
    }
}
