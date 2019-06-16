using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Services
{
    public interface ITodoListService
    {
        //Get all the tables that belongs to user
        Task<IEnumerable<TodoList>> GetAllTodoListForUser(IdentityUser user);
        Task<TodoList> GetTodoListById(IdentityUser user, Guid id);
        Task<bool> RemoveTodoListForUser(IdentityUser user, Guid itemList);

        Task<bool> AddTodoListForUser(IdentityUser user, TodoList list);

        //create a table for the user

    }
}