using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Services
{
    public interface ITodoItemListService
    {
        //Get all the tables that belongs to user
        Task<IEnumerable<TodoItemList>> GetAllItemListForUser(IdentityUser user);
        Task<TodoItemList> GetItemListById(IdentityUser user, Guid id);
        Task<bool> RemoveItemListForUser(IdentityUser user, Guid itemList);

        Task<bool> AddItemListForUser(IdentityUser user, TodoItemList itemList);

        //create a table for the user

    }
}